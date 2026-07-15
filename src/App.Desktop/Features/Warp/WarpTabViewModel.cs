using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Threading.Tasks;
using System.Threading;
using System.Threading.Tasks;
using App.Core.Abstractions;
using App.Core.Commands;
using App.Core.Models;
using App.Desktop.Infrastructure;
using App.Desktop.Modules;
using App.Desktop.Services;
using App.Desktop.ViewModels;
using ReactiveUI;
using Serilog;

namespace App.Desktop.Features.Warp;

/// <summary>
/// Warp tab: a settings-backed (not DB/cache) location list with warp commands and add/remove
/// management.
/// </summary>
public sealed class WarpTabViewModel : TabModuleViewModel
{
    private const string Title_ = "Warp";

    private readonly IAppSettingsHolder _settings;
    private readonly IAppSettingsService _settingsService;
    private readonly ICommandDispatcher _cmd;
    private readonly IPlayerContext _player;
    private readonly IDialogService _dlg;

    private WarpLocationSettings? _selectedWarp;

    public override string Title => Title_;

    public override string IconKey => "fa-solid fa-location-dot";

    public override int Order => 80;

    public EntityBrowserViewModel<WarpLocationSettings> Browser { get; }

    public ReactiveCommand<Unit, Unit> Warp { get; }

    public ReactiveCommand<Unit, Unit> WarpToYou { get; }

    public ReactiveCommand<Unit, Unit> WarpToSomeone { get; }

    public ReactiveCommand<Unit, Unit> Add { get; }

    public ReactiveCommand<Unit, Unit> RemoveSelected { get; }

    public WarpTabViewModel(
        INameNormalizer norm,
        IAppSettingsHolder settings,
        IAppSettingsService settingsService,
        ICommandDispatcher cmd,
        IPlayerContext player,
        IDialogService dlg)
    {
        _settings = settings;
        _settingsService = settingsService;
        _cmd = cmd;
        _player = player;
        _dlg = dlg;

        // Seed the 31 defaults when no warp locations are stored yet.
        if (_settings.Current.WarpLocations is null || _settings.Current.WarpLocations.Count == 0)
        {
            _settings.Current.WarpLocations = WarpDefaults.Create();
            QueueSave();
        }

        Browser = new EntityBrowserViewModel<WarpLocationSettings>(
            loadAllAsync: _ => Task.FromResult<IReadOnlyList<WarpLocationSettings>>(CurrentWarps()),
            idSelector: x => x.X,
            nameSelector: x => x.Name,
            rowValuesSelector: x => new object?[] { x.X, x.Y, x.Name },
            normalizer: norm)
        {
            Columns =
            [
                new BrowserColumn("X", 120),
                new BrowserColumn("Y", 120),
                new BrowserColumn("Name", 420, Fill: true),
            ],
            ByNameLabel = "by Name",
            IdSearchEnabled = false,
            RealtimeVisible = false,
        };

        Browser.ErrorOccurred += async (_, ex) => await _dlg.ShowErrorAsync(Title_, ex.Message);

        Browser.WhenSelectedRecordChanged.Subscribe(OnSelectionChanged);

        Warp = ReactiveCommand.CreateFromTask(WarpAsync);
        WarpToYou = ReactiveCommand.CreateFromTask(WarpToYouAsync);
        WarpToSomeone = ReactiveCommand.CreateFromTask(WarpToSomeoneAsync);
        Add = ReactiveCommand.CreateFromTask(AddAsync);
        RemoveSelected = ReactiveCommand.CreateFromTask(RemoveSelectedAsync);

        // Populate the list immediately on init.
        Browser.LoadAll.Execute().Subscribe();
    }

    // --- Inputs. ---

    private int _selectedX;

    public int SelectedX
    {
        get => _selectedX;
        set => this.RaiseAndSetIfChanged(ref _selectedX, value);
    }

    private int _selectedY;

    public int SelectedY
    {
        get => _selectedY;
        set => this.RaiseAndSetIfChanged(ref _selectedY, value);
    }

    private int _addX;

    public int AddX
    {
        get => _addX;
        set => this.RaiseAndSetIfChanged(ref _addX, value);
    }

    private int _addY;

    public int AddY
    {
        get => _addY;
        set => this.RaiseAndSetIfChanged(ref _addY, value);
    }

    private string _locationName = string.Empty;

    public string LocationName
    {
        get => _locationName;
        set => this.RaiseAndSetIfChanged(ref _locationName, value);
    }

    private void OnSelectionChanged(WarpLocationSettings? warp)
    {
        if (warp is null)
        {
            _selectedWarp = null;
            return;
        }

        _selectedWarp = warp;
        SelectedX = warp.X;
        SelectedY = warp.Y;
        AddX = warp.X;
        AddY = warp.Y;
    }

    private async Task WarpAsync()
    {
        if (_selectedWarp is null)
        {
            await _dlg.ShowWarningAsync(Title_, "Select warp first.");
            return;
        }

        if (!_player.TryResolveRequired(out var p))
        {
            await _dlg.ShowWarningAsync(Title_, "Select player in the right sidebar.");
            return;
        }

        await _cmd.DispatchAsync(LuaCommands.WarpToLocationForPlayer(_selectedWarp.X, _selectedWarp.Y, p));
    }

    private async Task WarpToYouAsync()
    {
        if (!_player.TryResolveRequired(out var p))
        {
            await _dlg.ShowWarningAsync(Title_, "Select player in the right sidebar.");
            return;
        }

        await _cmd.DispatchAsync(LuaCommands.WarpPlayerToYou(p));
    }

    private async Task WarpToSomeoneAsync()
    {
        if (!_player.TryResolveRequired(out var p))
        {
            await _dlg.ShowWarningAsync(Title_, "Select player in the right sidebar.");
            return;
        }

        await _cmd.DispatchAsync(LuaCommands.WarpYouToPlayer(p));
    }

    private async Task AddAsync()
    {
        var name = LocationName.Trim();
        if (string.IsNullOrWhiteSpace(name))
        {
            await _dlg.ShowWarningAsync(Title_, "Location name is required.");
            return;
        }

        CurrentWarps().Add(new WarpLocationSettings { X = AddX, Y = AddY, Name = name });

        LocationName = string.Empty;
        await ReloadAsync();
        QueueSave();
    }

    private async Task RemoveSelectedAsync()
    {
        if (_selectedWarp is null)
        {
            await _dlg.ShowWarningAsync(Title_, "Select warp first.");
            return;
        }

        var warps = CurrentWarps();
        var removed = warps.Remove(_selectedWarp);
        if (!removed)
        {
            removed = warps.RemoveAll(x =>
                x.X == _selectedWarp.X
                && x.Y == _selectedWarp.Y
                && x.Name.Equals(_selectedWarp.Name, StringComparison.Ordinal)) > 0;
        }

        if (!removed)
        {
            await _dlg.ShowWarningAsync(Title_, "Selected warp was not found.");
            return;
        }

        _selectedWarp = null;
        await ReloadAsync();
        QueueSave();
    }

    private async Task ReloadAsync() => await Browser.LoadAll.Execute().ToTask();

    private List<WarpLocationSettings> CurrentWarps() => _settings.Current.WarpLocations ??= [];

    private void QueueSave()
    {
        _ = SaveSafeAsync();
    }

    private async Task SaveSafeAsync()
    {
        try
        {
            await _settingsService.SaveAsync(_settings.Current, CancellationToken.None).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "Failed to persist warp locations.");
        }
    }
}
