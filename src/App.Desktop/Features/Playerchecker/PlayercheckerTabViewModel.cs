using System.Collections.Generic;
using System.IO;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;
using App.Core.Abstractions;
using App.Core.Models.Entities;
using App.Desktop.Infrastructure;
using App.Desktop.Modules;
using App.Desktop.Services;
using App.Desktop.ViewModels;
using ReactiveUI;

namespace App.Desktop.Features.Playerchecker;

/// <summary>
/// Playerchecker tab: live (never cached) character search with SQL-side filtering. The selected
/// character's inventory / warehouse opens in a separate pop-out window (with optional item icons).
/// </summary>
public sealed class PlayercheckerTabViewModel : TabModuleViewModel
{
    private const string Title_ = "Playerchecker";

    private readonly IGameDataRepository _repo;
    private readonly ConnectionStringResolver _connection;
    private readonly IDialogService _dlg;
    private readonly IAppSettingsHolder _settings;
    private readonly IInventoryWindowService _inventoryWindow;

    private PlayerRecord? _selectedPlayer;

    public override string Title => Title_;

    public override string IconKey => "fa-solid fa-users";

    public override int Order => 10;

    public EntityBrowserViewModel<PlayerRecord> Browser { get; }

    public ReactiveCommand<Unit, Unit> LoadAllCharacters { get; }

    public ReactiveCommand<Unit, Unit> LoadOnlineCharacters { get; }

    public ReactiveCommand<Unit, Unit> LoadInventory { get; }

    public ReactiveCommand<Unit, Unit> LoadWarehouse { get; }

    public ReactiveCommand<Unit, Unit> OpenInfos { get; }

    public PlayercheckerTabViewModel(
        IGameDataRepository repo,
        INameNormalizer norm,
        IAppSettingsHolder settings,
        ConnectionStringResolver connection,
        IDialogService dlg,
        IInventoryWindowService inventoryWindow)
    {
        _repo = repo;
        _connection = connection;
        _dlg = dlg;
        _settings = settings;
        _inventoryWindow = inventoryWindow;

        Browser = new EntityBrowserViewModel<PlayerRecord>(
            loadAllAsync: ct => _repo.GetPlayersAsync(connection.Provider, connection.Resolve(), connection.Tokens(), ct),
            idSelector: x => x.PlayerId,
            nameSelector: x => x.PlayerName,
            rowValuesSelector: x => new object?[]
            {
                x.PlayerId,
                x.PlayerName,
                x.Account,
                x.Level,
                x.JobName ?? string.Empty,
                x.OnlineStatus,
            },
            normalizer: norm,
            sqlSearchAsync: (term, mode, ct) => _repo.GetCharactersBySearchAsync(
                connection.Provider,
                connection.Resolve(),
                term,
                searchByAccount: mode == SearchMode.ById,
                connection.Tokens(),
                ct),
            maxRowsSelector: () => settings.Current.LimitSelectQueries ? 1000 : (int?)null)
        {
            Columns =
            [
                new BrowserColumn("ID", 80),
                new BrowserColumn("Name", 230, Fill: true),
                new BrowserColumn("Account", 200, Fill: true),
                new BrowserColumn("Level", 80),
                new BrowserColumn("Job", 160, Fill: true),
                new BrowserColumn("Status", 90),
            ],
            ByIdLabel = "by Account",
            ByNameLabel = "by Char Name",
            LoadAllVisible = false,
            RealtimeVisible = false,
            RealtimeEnabled = false,
            DebounceMs = 350,
        };

        Browser.ErrorOccurred += async (_, ex) => await _dlg.ShowErrorAsync("Data operation failed.", ex.Message);

        Browser.WhenSelectedRecordChanged.Subscribe(r => _selectedPlayer = r);

        LoadAllCharacters = ReactiveCommand.CreateFromTask(LoadAllCharactersAsync);
        LoadOnlineCharacters = ReactiveCommand.CreateFromTask(LoadOnlineCharactersAsync);
        LoadInventory = ReactiveCommand.CreateFromTask(LoadInventoryAsync);
        LoadWarehouse = ReactiveCommand.CreateFromTask(LoadWarehouseAsync);
        OpenInfos = ReactiveCommand.CreateFromTask(OpenInfosAsync);

        LoadAllCharacters.ThrownExceptions.Subscribe(async ex => await _dlg.ShowErrorAsync("Failed to load all characters.", ex.Message));
        LoadOnlineCharacters.ThrownExceptions.Subscribe(async ex => await _dlg.ShowErrorAsync("Failed to load online characters.", ex.Message));
        LoadInventory.ThrownExceptions.Subscribe(async ex => await _dlg.ShowErrorAsync("Failed to load inventory.", ex.Message));
        LoadWarehouse.ThrownExceptions.Subscribe(async ex => await _dlg.ShowErrorAsync("Failed to load warehouse.", ex.Message));
    }

    private async Task LoadAllCharactersAsync()
        => await Browser.LoadExternalAsync(
            ct => _repo.GetAllCharactersAsync(_connection.Provider, _connection.Resolve(), _connection.Tokens(), ct));

    private async Task LoadOnlineCharactersAsync()
        => await Browser.LoadExternalAsync(
            ct => _repo.GetOnlineCharactersAsync(_connection.Provider, _connection.Resolve(), _connection.Tokens(), ct));

    private async Task LoadInventoryAsync()
    {
        if (_selectedPlayer is null)
        {
            await _dlg.ShowWarningAsync("Load Inventory", "Select a player first.");
            return;
        }

        var items = await _repo.GetInventoryAsync(
            _connection.Provider,
            _connection.Resolve(),
            _selectedPlayer.PlayerId,
            _connection.Tokens(),
            CancellationToken.None);

        _inventoryWindow.Show($"Inventory — {_selectedPlayer.PlayerName} ({items.Count} item(s))", items, IconsEnabled());
    }

    private async Task LoadWarehouseAsync()
    {
        if (_selectedPlayer is null)
        {
            await _dlg.ShowWarningAsync("Load Warehouse", "Select a player first.");
            return;
        }

        var items = await _repo.GetWarehouseAsync(
            _connection.Provider,
            _connection.Resolve(),
            _selectedPlayer.Account,
            _connection.Tokens(),
            CancellationToken.None);

        _inventoryWindow.Show($"Warehouse — {_selectedPlayer.Account} ({items.Count} item(s))", items, IconsEnabled());
    }

    private async Task OpenInfosAsync()
    {
        if (_selectedPlayer is null)
        {
            await _dlg.ShowWarningAsync("Player Info", "Select a player first.");
            return;
        }

        var r = _selectedPlayer;
        await _dlg.ShowInfoAsync(
            "Player Info",
            $"Name: {r.PlayerName}\nAccount: {r.Account}\nLevel: {r.Level}\nJob: {r.JobName}\nStatus: {r.OnlineStatus}");
    }

    private bool IconsEnabled()
        => _settings.Current.EnableEntityIcons
            && !string.IsNullOrWhiteSpace(_settings.Current.EntityIconsPath)
            && Directory.Exists(_settings.Current.EntityIconsPath);
}
