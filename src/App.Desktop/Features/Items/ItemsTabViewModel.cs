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

namespace App.Desktop.Features.Items;

/// <summary>
/// The reference per-tab pattern: a <see cref="TabModuleViewModel"/> wrapping an
/// <see cref="EntityBrowserViewModel{TRecord}"/>, input properties, and <see cref="ReactiveCommand"/>s that
/// build a Lua string via <see cref="LuaCommands"/> and funnel it through <see cref="ICommandDispatcher"/>.
/// </summary>
public sealed class ItemsTabViewModel : TabModuleViewModel
{
    private readonly ICommandDispatcher _cmd;
    private readonly IPlayerContext _player;
    private readonly IDialogService _dlg;

    public override string Title => "Items";

    public override string IconKey => "fa-solid fa-box";

    public override int Order => 30;

    public EntityBrowserViewModel<ItemRecord> Browser { get; }

    public ReactiveCommand<Unit, Unit> AddYourself { get; }

    public ReactiveCommand<Unit, Unit> GiveOtherPlayer { get; }

    public ReactiveCommand<Unit, Unit> EditLevel { get; }

    public ReactiveCommand<Unit, Unit> EditEnhance { get; }

    public ReactiveCommand<Unit, Unit> ChangeAppearance { get; }

    public ReactiveCommand<Unit, Unit> ChangeItemCode { get; }

    public ItemsTabViewModel(
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

        bool Icons() => settings.Current.EnableEntityIcons
            && !string.IsNullOrWhiteSpace(settings.Current.EntityIconsPath)
            && Directory.Exists(settings.Current.EntityIconsPath);

        // Single icons-on snapshot: columns and row-values must share the same shape (#3).
        var iconsOn = Icons();

        Browser = new EntityBrowserViewModel<ItemRecord>(
            loadAllAsync: ct => settings.Current.UseLocalCache
                ? cache.LoadAsync<ItemRecord>("items", ct)
                : repo.GetItemsAsync(connection.Provider, connection.Resolve(), connection.Tokens(), ct),
            idSelector: x => x.ItemId,
            nameSelector: x => x.NameEn,
            rowValuesSelector: x => iconsOn
                ? new object?[] { x.IconFileName ?? string.Empty, x.ItemId, x.NameEn }
                : new object?[] { x.ItemId, x.NameEn },
            normalizer: norm,
            maxRowsSelector: () => settings.Current.LimitSelectQueries ? 1000 : (int?)null)
        {
            Columns = iconsOn
                ?
                [
                    new BrowserColumn("Icon", 44, IsImage: true, ImageSize: 36),
                    new BrowserColumn("ID", 80),
                    new BrowserColumn("Name", 460, Fill: true),
                ]
                :
                [
                    new BrowserColumn("ID", 80),
                    new BrowserColumn("Name", 460, Fill: true),
                ],
            ByIdLabel = "Search by ID",
            ByNameLabel = "Search by Name",
        };

        Browser.ErrorOccurred += async (_, ex) => await _dlg.ShowErrorAsync("Items", ex.Message);

        // Auto-populate inputs from the selected record.
        Browser.WhenSelectedRecordChanged
            .Where(r => r is not null)
            .Subscribe(r =>
            {
                ItemId = r!.ItemId;
                ModifyItemCode = r.ItemId;
            });

        AddYourself = ReactiveCommand.CreateFromTask(async () =>
        {
            if (ItemId <= 0)
            {
                await _dlg.ShowWarningAsync("Items", "Select item or enter Item ID first.");
                return;
            }

            await _cmd.DispatchAsync(LuaCommands.InsertItemSelf(ItemId, Amount, Enhance, Level, StatusFlag));
        });

        GiveOtherPlayer = ReactiveCommand.CreateFromTask(async () =>
        {
            if (ItemId <= 0)
            {
                await _dlg.ShowWarningAsync("Items", "Select item or enter Item ID first.");
                return;
            }

            if (!_player.TryResolveRequired(out var p))
            {
                await _dlg.ShowWarningAsync("Items", "Select player in the right sidebar for 'Give other player'.");
                return;
            }

            await _cmd.DispatchAsync(LuaCommands.InsertItemPlayer(ItemId, Amount, Enhance, Level, StatusFlag, p));
        });

        EditLevel = ReactiveCommand.CreateFromTask(() => ModifyAsync(
            self: () => LuaCommands.SetWearItemLevelOwn(WearSlotIndex, ModifyLevel),
            other: p => LuaCommands.SetWearItemLevelPlayer(WearSlotIndex, p, ModifyLevel),
            requireItemCode: false));

        EditEnhance = ReactiveCommand.CreateFromTask(() => ModifyAsync(
            self: () => LuaCommands.SetWearItemEnhanceOwn(WearSlotIndex, ModifyEnhance),
            other: p => LuaCommands.SetWearItemEnhancePlayer(WearSlotIndex, p, ModifyEnhance),
            requireItemCode: false));

        ChangeAppearance = ReactiveCommand.CreateFromTask(() => ModifyAsync(
            self: () => LuaCommands.SetWearItemAppearanceOwn(WearSlotIndex, ModifyItemCode),
            other: p => LuaCommands.SetWearItemAppearancePlayer(WearSlotIndex, p, ModifyItemCode),
            requireItemCode: true));

        ChangeItemCode = ReactiveCommand.CreateFromTask(() => ModifyAsync(
            self: () => LuaCommands.ChangeWearItemCodeOwn(WearSlotIndex, ModifyItemCode),
            other: p => LuaCommands.ChangeWearItemCodePlayer(WearSlotIndex, p, ModifyItemCode),
            requireItemCode: true));

        // Auto-load game data at startup (failures are silent — see AutoLoadAsync).
        _ = Browser.AutoLoadAsync();
    }

    // --- Insert inputs (defaults per parity inventory). ---

    private int _itemId = 1;

    public int ItemId
    {
        get => _itemId;
        set => this.RaiseAndSetIfChanged(ref _itemId, value);
    }

    private int _amount = 1;

    public int Amount
    {
        get => _amount;
        set => this.RaiseAndSetIfChanged(ref _amount, value);
    }

    private int _enhance;

    public int Enhance
    {
        get => _enhance;
        set => this.RaiseAndSetIfChanged(ref _enhance, value);
    }

    private int _level = 1;

    public int Level
    {
        get => _level;
        set => this.RaiseAndSetIfChanged(ref _level, value);
    }

    private bool _useStatusFlag;

    public bool UseStatusFlag
    {
        get => _useStatusFlag;
        set => this.RaiseAndSetIfChanged(ref _useStatusFlag, value);
    }

    private int _statusFlagValue;

    public int StatusFlagValue
    {
        get => _statusFlagValue;
        set => this.RaiseAndSetIfChanged(ref _statusFlagValue, value);
    }

    public int StatusFlag => UseStatusFlag ? StatusFlagValue : 0;

    // --- Modify inputs. ---

    public string[] WearSlots { get; } = ItemsWearSlots.All;

    private string _wearSlot = ItemsWearSlots.Default;

    public string WearSlot
    {
        get => _wearSlot;
        set => this.RaiseAndSetIfChanged(ref _wearSlot, value);
    }

    public int WearSlotIndex => ItemsWearSlots.ParseIndex(WearSlot);

    private bool _applyToOther;

    public bool ApplyToOther
    {
        get => _applyToOther;
        set => this.RaiseAndSetIfChanged(ref _applyToOther, value);
    }

    private int _modifyLevel = 1;

    public int ModifyLevel
    {
        get => _modifyLevel;
        set => this.RaiseAndSetIfChanged(ref _modifyLevel, value);
    }

    private int _modifyEnhance;

    public int ModifyEnhance
    {
        get => _modifyEnhance;
        set => this.RaiseAndSetIfChanged(ref _modifyEnhance, value);
    }

    private int _modifyItemCode = 1;

    public int ModifyItemCode
    {
        get => _modifyItemCode;
        set => this.RaiseAndSetIfChanged(ref _modifyItemCode, value);
    }

    private async Task ModifyAsync(Func<string> self, Func<string, string> other, bool requireItemCode)
    {
        if (requireItemCode && ModifyItemCode <= 0)
        {
            await _dlg.ShowWarningAsync("Items", "Itemcode must be greater than 0.");
            return;
        }

        if (ApplyToOther)
        {
            if (!_player.TryResolveRequired(out var p))
            {
                await _dlg.ShowWarningAsync("Items", "Select player in the right sidebar for 'Other' mode.");
                return;
            }

            await _cmd.DispatchAsync(other(p));
            return;
        }

        await _cmd.DispatchAsync(self());
    }
}
