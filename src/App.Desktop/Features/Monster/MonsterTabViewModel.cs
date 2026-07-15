using System;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using App.Core.Abstractions;
using App.Core.Commands;
using App.Core.Models.Entities;
using App.Desktop.Infrastructure;
using App.Desktop.Modules;
using App.Desktop.Services;
using App.Desktop.ViewModels;
using ReactiveUI;

namespace App.Desktop.Features.Monster;

/// <summary>
/// Monster spawn tab: a single "Create Command" that builds a regenerate / add_npc Lua string,
/// with mode-driven input enablement.
/// </summary>
public sealed class MonsterTabViewModel : TabModuleViewModel
{
    private const string SpawnModeAtYourPlace = "At your place";
    private const string SpawnModeAtSelectedPlayer = "At selected player place";
    private const string SpawnModeAtSpecificCoordinates = "At specific coordinates";

    private readonly ICommandDispatcher _cmd;
    private readonly IPlayerContext _player;
    private readonly IDialogService _dlg;

    public override string Title => "Monster";

    public override string IconKey => "fa-solid fa-dragon";

    public override int Order => 20;

    public EntityBrowserViewModel<MonsterRecord> Browser { get; }

    public string[] SpawnModes { get; } =
    [
        SpawnModeAtYourPlace,
        SpawnModeAtSelectedPlayer,
        SpawnModeAtSpecificCoordinates,
    ];

    public ReactiveCommand<Unit, Unit> CreateCommand { get; }

    public MonsterTabViewModel(
        IGameDataRepository repo,
        ILocalCacheService cache,
        INameNormalizer norm,
        IAppSettingsHolder settings,
        ConnectionStringResolver connection,
        ICommandDispatcher cmd,
        IPlayerContext player,
        IDialogService dlg)
    {
        _cmd = cmd;
        _player = player;
        _dlg = dlg;

        Browser = new EntityBrowserViewModel<MonsterRecord>(
            loadAllAsync: ct => settings.Current.UseLocalCache
                ? cache.LoadAsync<MonsterRecord>("monsters", ct)
                : repo.GetMonstersAsync(connection.Provider, connection.Resolve(), connection.Tokens(), ct),
            idSelector: x => x.Id,
            nameSelector: x => x.Name,
            rowValuesSelector: x => new object?[]
            {
                x.Id,
                x.Name,
                x.Level,
                x.Location ?? string.Empty,
            },
            normalizer: norm,
            searchableTextSelector: x => new[] { x.Name, x.Location },
            maxRowsSelector: () => settings.Current.LimitSelectQueries ? 1000 : (int?)null)
        {
            Columns =
            [
                new BrowserColumn("ID", 80),
                new BrowserColumn("Name", 340, Fill: true),
                new BrowserColumn("Level", 90),
                new BrowserColumn("Location", 300, Fill: true),
            ],
            ByIdLabel = "Search by ID",
            ByNameLabel = "Search by Name",
        };

        Browser.ErrorOccurred += async (_, ex) => await _dlg.ShowErrorAsync("Monster", ex.Message);

        Browser.WhenSelectedRecordChanged
            .Where(r => r is not null)
            .Subscribe(r => MonsterId = r!.Id);

        // Recompute enable rules whenever the relevant inputs change.
        this.WhenAnyValue(x => x.SpawnMode, x => x.UseLifetime)
            .Subscribe(_ =>
            {
                this.RaisePropertyChanged(nameof(IsCoordinatesEnabled));
                this.RaisePropertyChanged(nameof(IsUseLifetimeEnabled));
                this.RaisePropertyChanged(nameof(IsMinutesLifetimeEnabled));
            });

        CreateCommand = ReactiveCommand.CreateFromTask(CreateCommandAsync);

        // Auto-load game data at startup (failures are silent — see AutoLoadAsync).
        _ = Browser.AutoLoadAsync();
    }

    // --- Inputs (defaults per parity inventory). ---

    private string _spawnMode = SpawnModeAtYourPlace;

    public string SpawnMode
    {
        get => _spawnMode;
        set
        {
            this.RaiseAndSetIfChanged(ref _spawnMode, value);

            // Mode change can disable lifetime; reset accordingly.
            if (!IsUseLifetimeEnabled && UseLifetime)
            {
                UseLifetime = false;
            }
        }
    }

    private int _monsterId = 1;

    public int MonsterId
    {
        get => _monsterId;
        set => this.RaiseAndSetIfChanged(ref _monsterId, value);
    }

    private int _amount = 1;

    public int Amount
    {
        get => _amount;
        set => this.RaiseAndSetIfChanged(ref _amount, value);
    }

    private int _x;

    public int X
    {
        get => _x;
        set => this.RaiseAndSetIfChanged(ref _x, value);
    }

    private int _y;

    public int Y
    {
        get => _y;
        set => this.RaiseAndSetIfChanged(ref _y, value);
    }

    private int _layer = 1;

    public int Layer
    {
        get => _layer;
        set => this.RaiseAndSetIfChanged(ref _layer, value);
    }

    private bool _useLifetime;

    public bool UseLifetime
    {
        get => _useLifetime;
        set
        {
            this.RaiseAndSetIfChanged(ref _useLifetime, value);

            // Checking with minutes < 1 bumps to 1; unchecking resets to -1 (parity).
            if (value)
            {
                if (MinutesLifetime < 1)
                {
                    MinutesLifetime = 1;
                }
            }
            else
            {
                MinutesLifetime = -1;
            }
        }
    }

    private int _minutesLifetime = -1;

    public int MinutesLifetime
    {
        get => _minutesLifetime;
        set => this.RaiseAndSetIfChanged(ref _minutesLifetime, value);
    }

    // --- Enable rules (ported from ToggleInputsByMode). ---

    public bool IsCoordinatesEnabled =>
        string.Equals(SpawnMode, SpawnModeAtSpecificCoordinates, StringComparison.OrdinalIgnoreCase);

    public bool IsUseLifetimeEnabled =>
        IsCoordinatesEnabled
        || string.Equals(SpawnMode, SpawnModeAtSelectedPlayer, StringComparison.OrdinalIgnoreCase);

    public bool IsMinutesLifetimeEnabled => IsUseLifetimeEnabled && UseLifetime;

    private async Task CreateCommandAsync()
    {
        if (MonsterId <= 0)
        {
            await _dlg.ShowWarningAsync("Monster", "Select a monster or enter Monster ID first.");
            return;
        }

        var minutesLifetime = UseLifetime ? Math.Max(1, MinutesLifetime) : -1;

        switch (SpawnMode)
        {
            case SpawnModeAtYourPlace:
                await _cmd.DispatchAsync(LuaCommands.MonsterRegenerate(MonsterId, Amount));
                break;

            case SpawnModeAtSelectedPlayer:
                if (!_player.TryResolveRequired(out var p))
                {
                    await _dlg.ShowWarningAsync("Monster", "Select player in the right sidebar for 'At selected player place'.");
                    return;
                }

                await _cmd.DispatchAsync(LuaCommands.MonsterAddNpcAtPlayer(p, MonsterId, Amount, minutesLifetime));
                break;

            default:
                await _cmd.DispatchAsync(LuaCommands.MonsterAddNpcAtCoords(X, Y, MonsterId, Amount, minutesLifetime, Layer));
                break;
        }
    }
}
