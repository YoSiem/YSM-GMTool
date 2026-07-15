using System;
using System.Globalization;
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

namespace App.Desktop.Features.Npc;

/// <summary>
/// NPC tab: add/show/warp-to NPCs. The browser supports a secondary "Contact script" search.
/// </summary>
public sealed class NpcTabViewModel : TabModuleViewModel
{
    private const string SelfTarget = "self";

    private readonly ICommandDispatcher _cmd;
    private readonly IPlayerContext _player;
    private readonly IDialogService _dlg;

    public override string Title => "NPC";

    public override string IconKey => "fa-solid fa-person";

    public override int Order => 60;

    public EntityBrowserViewModel<NpcRecord> Browser { get; }

    public ReactiveCommand<Unit, Unit> AddNpcToWorld { get; }

    public ReactiveCommand<Unit, Unit> ShowHideNpc { get; }

    public ReactiveCommand<Unit, Unit> WarpToNpc { get; }

    public NpcTabViewModel(
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

        Browser = new EntityBrowserViewModel<NpcRecord>(
            loadAllAsync: ct => settings.Current.UseLocalCache
                ? cache.LoadAsync<NpcRecord>("npcs", ct)
                : repo.GetNpcsAsync(connection.Provider, connection.Resolve(), connection.Tokens(), ct),
            idSelector: x => x.NpcId,
            nameSelector: x => x.NpcTitle,
            rowValuesSelector: x => new object?[]
            {
                x.NpcId,
                x.NpcTitle,
                x.X?.ToString("0.###", CultureInfo.InvariantCulture) ?? string.Empty,
                x.Y?.ToString("0.###", CultureInfo.InvariantCulture) ?? string.Empty,
                x.ContactScript ?? string.Empty,
            },
            normalizer: norm,
            searchableTextSelector: x => new[] { x.NpcTitle },
            secondarySearchTextSelector: x => x.ContactScript,
            maxRowsSelector: () => settings.Current.LimitSelectQueries ? 1000 : (int?)null)
        {
            Columns =
            [
                new BrowserColumn("NPC ID", 90),
                new BrowserColumn("Name", 420, Fill: true),
                new BrowserColumn("X", 90),
                new BrowserColumn("Y", 90),
                new BrowserColumn("Contact script", 280, Fill: true),
            ],
            ByIdLabel = "Search by ID",
            ByNameLabel = "Search by Name",
            SecondarySearchEnabled = true,
            SecondaryLabel = "for Contact script",
        };

        Browser.ErrorOccurred += async (_, ex) => await _dlg.ShowErrorAsync("NPC", ex.Message);

        Browser.WhenSelectedRecordChanged
            .Where(r => r is not null)
            .Subscribe(r =>
            {
                NpcId = r!.NpcId;
                NpcX = (int)Math.Round(r.X ?? 0);
                NpcY = (int)Math.Round(r.Y ?? 0);
            });

        AddNpcToWorld = ReactiveCommand.CreateFromTask(async () =>
        {
            if (!await GuardNpcAsync())
            {
                return;
            }

            var target = _player.Resolve();
            await _cmd.DispatchAsync(string.Equals(target, SelfTarget, StringComparison.OrdinalIgnoreCase)
                ? LuaCommands.AddNpcToWorld(NpcX, NpcY, Layer, NpcId)
                : LuaCommands.AddNpcToWorldForPlayer(NpcX, NpcY, Layer, target, NpcId));
        });

        ShowHideNpc = ReactiveCommand.CreateFromTask(async () =>
        {
            if (!await GuardNpcAsync())
            {
                return;
            }

            await _cmd.DispatchAsync(LuaCommands.ShowNpc(NpcX, NpcY, NpcId, Layer, VisibleFlag));
        });

        WarpToNpc = ReactiveCommand.CreateFromTask(async () =>
        {
            if (!_player.TryResolveRequired(out var p))
            {
                await _dlg.ShowWarningAsync("NPC", "Select player in the right sidebar for warp to NPC.");
                return;
            }

            await _cmd.DispatchAsync(LuaCommands.WarpToNpcCoordinates(NpcX, NpcY, p));
        });

        // Auto-load game data at startup (failures are silent — see AutoLoadAsync).
        _ = Browser.AutoLoadAsync();
    }

    // --- Inputs. ---

    private int _npcId;

    public int NpcId
    {
        get => _npcId;
        set => this.RaiseAndSetIfChanged(ref _npcId, value);
    }

    private int _npcX;

    public int NpcX
    {
        get => _npcX;
        set => this.RaiseAndSetIfChanged(ref _npcX, value);
    }

    private int _npcY;

    public int NpcY
    {
        get => _npcY;
        set => this.RaiseAndSetIfChanged(ref _npcY, value);
    }

    private int _layer;

    public int Layer
    {
        get => _layer;
        set => this.RaiseAndSetIfChanged(ref _layer, value);
    }

    private bool _hideNpc;

    public bool HideNpc
    {
        get => _hideNpc;
        set => this.RaiseAndSetIfChanged(ref _hideNpc, value);
    }

    public int VisibleFlag => HideNpc ? 1 : 0;

    private async Task<bool> GuardNpcAsync()
    {
        if (NpcId <= 0)
        {
            await _dlg.ShowWarningAsync("NPC", "Select npc or enter NPC ID first.");
            return false;
        }

        return true;
    }
}
