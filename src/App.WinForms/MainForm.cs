using App.Core.Enums;
using App.Core.Interfaces;
using App.Core.Models;
using App.Core.Models.Entities;
using App.WinForms.Controls;
using App.WinForms.Forms;
using App.WinForms.Models;
using App.WinForms.Presenters;
using Serilog;
using System.ComponentModel;
using System.Drawing;

namespace App.WinForms;

public partial class MainForm : Form
{
    private const string DbProviderEnvKey = "YSM_DB_PROVIDER";
    private const string DbConnectionStringEnvKey = "YSM_DB_CONNECTION_STRING";

    private readonly IGameDataRepository _repository;
    private readonly IAppSettingsService _settingsService;
    private readonly IConnectionStringBuilderService _connectionStringBuilder;
    private readonly ILuaCommandBuilder _luaCommandBuilder;
    private readonly ICommandHistoryService _commandHistoryService;
    private readonly INameNormalizer _nameNormalizer;
    private readonly ILocalCacheService _localCacheService;

    private readonly PlayerCheckerActionsControl _playerCheckerActions = new();
    private readonly MonsterActionsControl _monsterActions = new();
    private readonly ItemsActionsControl _itemsActions = new();
    private readonly SkillsActionsControl _skillsActions = new();
    private readonly BuffsActionsControl _buffsActions = new();
    private readonly NpcsActionsControl _npcsActions = new();
    private readonly SummonsActionsControl _summonsActions = new();

    private EntityBrowserPresenter<PlayerRecord>? _playerPresenter;
    private EntityBrowserPresenter<MonsterRecord>? _monsterPresenter;
    private EntityBrowserPresenter<ItemRecord>? _itemsPresenter;
    private EntityBrowserPresenter<SkillRecord>? _skillsPresenter;
    private EntityBrowserPresenter<StateRecord>? _buffsPresenter;
    private EntityBrowserPresenter<NpcRecord>? _npcsPresenter;
    private EntityBrowserPresenter<SummonRecord>? _summonsPresenter;

    private PlayerRecord? _selectedPlayerRecord;
    private MonsterRecord? _selectedMonster;
    private ItemRecord? _selectedItem;
    private SkillRecord? _selectedSkill;
    private StateRecord? _selectedState;
    private NpcRecord? _selectedNpc;
    private SummonRecord? _selectedSummon;

    private AppSettings _settings = new();
    private int _inventorySortColumnIndex = -1;
    private SortOrder _inventorySortOrder = SortOrder.None;

    public MainForm(
        IGameDataRepository repository,
        IAppSettingsService settingsService,
        IConnectionStringBuilderService connectionStringBuilder,
        ILuaCommandBuilder luaCommandBuilder,
        ICommandHistoryService commandHistoryService,
        INameNormalizer nameNormalizer,
        ILocalCacheService localCacheService)
    {
        _repository = repository;
        _settingsService = settingsService;
        _connectionStringBuilder = connectionStringBuilder;
        _luaCommandBuilder = luaCommandBuilder;
        _commandHistoryService = commandHistoryService;
        _nameNormalizer = nameNormalizer;
        _localCacheService = localCacheService;

        InitializeComponent();
        ApplyReadabilityPalette();
        ApplyBrandingIcon();

        AttachActionControls();
        ConfigureBrowserColumns();
        InitializePresenters();
        WireActionEvents();
    }

    private void ApplyBrandingIcon()
    {
        try
        {
            using var exeIcon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            if (exeIcon is not null)
            {
                Icon = (Icon)exeIcon.Clone();
            }
        }
        catch
        {
            // Fallbacks handled below.
        }

        try
        {
            using var stream = typeof(MainForm).Assembly.GetManifestResourceStream("App.WinForms.Assets.Heaven_logo1.png");
            if (stream is not null)
            {
                using var resourceImage = Image.FromStream(stream);
                picAvatar.Image = new Bitmap(resourceImage);
                return;
            }
        }
        catch
        {
            // Fallback to icon/avatar below.
        }

        if (Icon is not null)
        {
            using var avatarIcon = new Icon(Icon, new Size(128, 128));
            picAvatar.Image = avatarIcon.ToBitmap();
            return;
        }

        picAvatar.Image = SystemIcons.Application.ToBitmap();
    }

    private void ApplyReadabilityPalette()
    {
        var page = Color.FromArgb(24, 27, 33);
        var side = Color.FromArgb(30, 34, 42);
        var panelAlt = Color.FromArgb(36, 41, 50);
        var text = Color.FromArgb(235, 238, 245);
        var muted = Color.FromArgb(188, 196, 208);

        BackColor = page;
        tlpRoot.BackColor = page;
        tabMain.BackColor = page;

        tlpSidebar.BackColor = side;
        lblPlayer.ForeColor = muted;
        lblNewPlayer.ForeColor = muted;
        chkAppendCommands.ForeColor = text;

        lstCommands.BackColor = panelAlt;
        lstCommands.ForeColor = text;
        txtNewPlayer.BackColor = panelAlt;
        txtNewPlayer.ForeColor = text;
        cmbPlayers.BackColor = panelAlt;
        cmbPlayers.ForeColor = text;

        var border = Color.FromArgb(68, 78, 95);
        var header = Color.FromArgb(48, 55, 67);
        var accent = Color.FromArgb(72, 118, 196);
        gridInventory.BackgroundColor = Color.FromArgb(30, 34, 41);
        gridInventory.GridColor = border;
        gridInventory.BorderStyle = BorderStyle.FixedSingle;
        gridInventory.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        gridInventory.DefaultCellStyle.BackColor = Color.FromArgb(30, 34, 41);
        gridInventory.DefaultCellStyle.ForeColor = text;
        gridInventory.DefaultCellStyle.SelectionBackColor = accent;
        gridInventory.DefaultCellStyle.SelectionForeColor = Color.White;
        gridInventory.AlternatingRowsDefaultCellStyle.BackColor = panelAlt;
        gridInventory.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
        gridInventory.ColumnHeadersDefaultCellStyle.BackColor = header;
        gridInventory.ColumnHeadersDefaultCellStyle.ForeColor = text;
        gridInventory.ColumnHeadersDefaultCellStyle.SelectionBackColor = header;
        gridInventory.ColumnHeadersDefaultCellStyle.SelectionForeColor = text;
        gridInventory.CellDoubleClick += GenericGrid_CellDoubleClick;
        gridInventory.ColumnHeaderMouseClick += GridInventory_ColumnHeaderMouseClick;
    }

    private void GridInventory_ColumnHeaderMouseClick(object? sender, DataGridViewCellMouseEventArgs e)
    {
        if (gridInventory.Rows.Count == 0)
        {
            return;
        }

        var columnIndex = e.ColumnIndex;
        if (_inventorySortColumnIndex == columnIndex)
        {
            _inventorySortOrder = _inventorySortOrder == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
        }
        else
        {
            _inventorySortColumnIndex = columnIndex;
            _inventorySortOrder = SortOrder.Ascending;
        }

        var direction = _inventorySortOrder == SortOrder.Ascending
            ? ListSortDirection.Ascending
            : ListSortDirection.Descending;

        gridInventory.Sort(gridInventory.Columns[columnIndex], direction);

        foreach (DataGridViewColumn col in gridInventory.Columns)
        {
            col.HeaderCell.SortGlyphDirection = col.Index == columnIndex ? _inventorySortOrder : SortOrder.None;
        }
    }

    private void GenericGrid_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (sender is DataGridView grid && e.RowIndex >= 0 && e.ColumnIndex >= 0)
        {
            var value = grid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            if (value != null && !string.IsNullOrWhiteSpace(value.ToString()))
            {
                Clipboard.SetText(value.ToString()!);
            }
        }
    }

    private void AttachActionControls()
    {
        AddActionControl(browserPlayerchecker, _playerCheckerActions);
        AddActionControl(browserMonster, _monsterActions);
        AddActionControl(browserItems, _itemsActions);
        AddActionControl(browserSkills, _skillsActions);
        AddActionControl(browserBuffs, _buffsActions);
        AddActionControl(browserNpcs, _npcsActions);
        AddActionControl(browserSummons, _summonsActions);
    }

    private static void AddActionControl(EntityBrowserControl browser, Control control)
    {
        control.Dock = DockStyle.None;
        control.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        browser.ActionsHostPanel.Controls.Add(control);

        ApplyActionControlWidth(browser.ActionsHostPanel, control);
        browser.ActionsHostPanel.Resize += (_, _) => ApplyActionControlWidth(browser.ActionsHostPanel, control);
    }

    private static void ApplyActionControlWidth(Control host, Control control)
    {
        if (control.IsDisposed || host.ClientSize.Width <= 0)
        {
            return;
        }

        // 97% of parent width, centered
        var targetWidth = (int)(host.ClientSize.Width * 0.97);
        targetWidth = Math.Max(120, targetWidth);
        int leftPad = (host.ClientSize.Width - targetWidth) / 2;

        control.Location = new Point(leftPad, control.Location.Y);
        control.Width = targetWidth;
    }

    private void ConfigureBrowserColumns()
    {
        browserPlayerchecker.ConfigureColumns(
        [
            new BrowserColumnDefinition("playerId", "ID", 80),
            new BrowserColumnDefinition("playerName", "Name", 230, true),
            new BrowserColumnDefinition("account", "Account", 200, true),
            new BrowserColumnDefinition("level", "Level", 80),
            new BrowserColumnDefinition("job", "Job", 160, true),
            new BrowserColumnDefinition("status", "Status", 90)
        ]);
        browserPlayerchecker.ConfigureSearchLabels("by Account", "by Char Name");
        browserPlayerchecker.HideLoadAllButton();
        browserPlayerchecker.SetDebounceDelay(350);
        browserPlayerchecker.RealtimeFilteringVisible = false;
        browserPlayerchecker.RealtimeFilteringEnabled = false;

        browserMonster.ConfigureColumns(
        [
            new BrowserColumnDefinition("id", "ID", 80),
            new BrowserColumnDefinition("name", "Name", 340, true),
            new BrowserColumnDefinition("level", "Level", 90),
            new BrowserColumnDefinition("location", "Location", 300, true)
        ]);

        browserItems.ConfigureColumns(
        [
            new BrowserColumnDefinition("itemId", "ID", 80),
            new BrowserColumnDefinition("name", "Name", 460, true)
        ]);

        browserSkills.ConfigureColumns(
        [
            new BrowserColumnDefinition("skillId", "ID", 80),
            new BrowserColumnDefinition("skillName", "Name", 460, true)
        ]);

        browserBuffs.ConfigureColumns(
        [
            new BrowserColumnDefinition("stateId", "State ID", 100),
            new BrowserColumnDefinition("buffName", "Buff name", 460, true)
        ]);

        browserNpcs.ConfigureColumns(
        [
            new BrowserColumnDefinition("npcId", "NPC ID", 90),
            new BrowserColumnDefinition("npcTitle", "Name", 420, true),
            new BrowserColumnDefinition("x", "X", 90),
            new BrowserColumnDefinition("y", "Y", 90),
            new BrowserColumnDefinition("contactScript", "Contact script", 280, true)
        ]);
        browserNpcs.ConfigureSecondarySearch(true, "for Contact script");

        browserSummons.ConfigureColumns(
        [
            new BrowserColumnDefinition("summonId", "Summon ID", 100),
            new BrowserColumnDefinition("summonName", "Summon Name", 320, true),
            new BrowserColumnDefinition("cardName", "Card Name", 320, true)
        ]);
    }

    private void InitializePresenters()
    {
        _playerPresenter = new EntityBrowserPresenter<PlayerRecord>(
            browserPlayerchecker,
            ct => _repository.GetPlayersAsync(_settings.Provider, GetConfiguredConnectionString(), GetQueryTokens(), ct),
            x => x.PlayerId,
            x => x.PlayerName,
            x =>
            [
                x.PlayerId,
                x.PlayerName,
                x.Account,
                x.Level,
                x.JobName ?? string.Empty,
                x.OnlineStatus
            ],
            _nameNormalizer,
            sqlSearchAsync: (term, mode, ct) => _repository.GetCharactersBySearchAsync(
                _settings.Provider,
                GetConfiguredConnectionString(),
                term,
                searchByAccount: mode == SearchMode.ById,
                GetQueryTokens(),
                ct),
            maxRowsSelector: () => _settings.LimitSelectQueries ? 1000 : null);

        _monsterPresenter = new EntityBrowserPresenter<MonsterRecord>(
            browserMonster,
            ct => _settings.UseLocalCache
                ? _localCacheService.LoadAsync<MonsterRecord>("monsters", ct)
                : _repository.GetMonstersAsync(_settings.Provider, GetConfiguredConnectionString(), GetQueryTokens(), ct),
            x => x.Id,
            x => x.Name,
            x =>
            [
                x.Id,
                x.Name,
                x.Level ?? 0,
                x.Location ?? string.Empty
            ],
            _nameNormalizer,
            x =>
            [
                x.Name,
                x.Location
            ], maxRowsSelector: () => _settings.LimitSelectQueries ? 1000 : null);

        _itemsPresenter = new EntityBrowserPresenter<ItemRecord>(
            browserItems,
            ct => _settings.UseLocalCache
                ? _localCacheService.LoadAsync<ItemRecord>("items", ct)
                : _repository.GetItemsAsync(_settings.Provider, GetConfiguredConnectionString(), GetQueryTokens(), ct),
            x => x.ItemId,
            x => x.NameEn,
            x =>
            [
                x.ItemId,
                x.NameEn
            ],
            _nameNormalizer, maxRowsSelector: () => _settings.LimitSelectQueries ? 1000 : null);

        _skillsPresenter = new EntityBrowserPresenter<SkillRecord>(
            browserSkills,
            ct => _settings.UseLocalCache
                ? _localCacheService.LoadAsync<SkillRecord>("skills", ct)
                : _repository.GetSkillsAsync(_settings.Provider, GetConfiguredConnectionString(), GetQueryTokens(), ct),
            x => x.SkillId,
            x => x.Skillname,
            x =>
            [
                x.SkillId,
                x.Skillname
            ],
            _nameNormalizer, maxRowsSelector: () => _settings.LimitSelectQueries ? 1000 : null);

        _buffsPresenter = new EntityBrowserPresenter<StateRecord>(
            browserBuffs,
            ct => _settings.UseLocalCache
                ? _localCacheService.LoadAsync<StateRecord>("states", ct)
                : _repository.GetStatesAsync(_settings.Provider, GetConfiguredConnectionString(), GetQueryTokens(), ct),
            x => x.StateId,
            x => x.BuffName,
            x =>
            [
                x.StateId,
                x.BuffName
            ],
            _nameNormalizer, maxRowsSelector: () => _settings.LimitSelectQueries ? 1000 : null);

        _npcsPresenter = new EntityBrowserPresenter<NpcRecord>(
            browserNpcs,
            ct => _settings.UseLocalCache
                ? _localCacheService.LoadAsync<NpcRecord>("npcs", ct)
                : _repository.GetNpcsAsync(_settings.Provider, GetConfiguredConnectionString(), GetQueryTokens(), ct),
            x => x.NpcId,
            x => x.NpcTitle,
            x =>
            [
                x.NpcId,
                x.NpcTitle,
                x.X?.ToString("0.###") ?? string.Empty,
                x.Y?.ToString("0.###") ?? string.Empty,
                x.ContactScript ?? string.Empty
            ],
            _nameNormalizer,
            x =>
            [
                x.NpcTitle
            ],
            x => x.ContactScript,
            maxRowsSelector: () => _settings.LimitSelectQueries ? 1000 : null);

        _summonsPresenter = new EntityBrowserPresenter<SummonRecord>(
            browserSummons,
            ct => _settings.UseLocalCache
                ? _localCacheService.LoadAsync<SummonRecord>("summons", ct)
                : _repository.GetSummonsAsync(_settings.Provider, GetConfiguredConnectionString(), GetQueryTokens(), ct),
            x => x.SummonId,
            x => x.SummonName,
            x =>
            [
                x.SummonId,
                x.SummonName,
                x.CardName ?? string.Empty
            ],
            _nameNormalizer,
            x =>
            [
                x.SummonName,
                x.CardName
            ], maxRowsSelector: () => _settings.LimitSelectQueries ? 1000 : null);

        var allPresenters = new object?[]
        {
            _playerPresenter,
            _monsterPresenter,
            _itemsPresenter,
            _skillsPresenter,
            _buffsPresenter,
            _npcsPresenter,
            _summonsPresenter
        };

        foreach (var presenter in allPresenters)
        {
            if (presenter is null)
            {
                continue;
            }

            switch (presenter)
            {
                case EntityBrowserPresenter<PlayerRecord> p:
                    p.SelectedRecordChanged += (_, selected) => _selectedPlayerRecord = selected;
                    p.ErrorOccurred += OnPresenterError;
                    break;
                case EntityBrowserPresenter<MonsterRecord> p:
                    p.SelectedRecordChanged += (_, selected) =>
                    {
                        _selectedMonster = selected;
                        if (selected is not null)
                        {
                            _monsterActions.MonsterId = selected.Id;
                        }
                    };
                    p.ErrorOccurred += OnPresenterError;
                    break;
                case EntityBrowserPresenter<ItemRecord> p:
                    p.SelectedRecordChanged += (_, selected) =>
                    {
                        _selectedItem = selected;
                        if (selected is not null)
                        {
                            _itemsActions.ItemId = selected.ItemId;
                            _itemsActions.ItemName = selected.NameEn;
                            _itemsActions.ModifyItemCode = selected.ItemId;
                        }
                    };
                    p.ErrorOccurred += OnPresenterError;
                    break;
                case EntityBrowserPresenter<SkillRecord> p:
                    p.SelectedRecordChanged += (_, selected) =>
                    {
                        _selectedSkill = selected;
                        if (selected is not null)
                        {
                            _skillsActions.SkillId = selected.SkillId;
                        }
                    };
                    p.ErrorOccurred += OnPresenterError;
                    break;
                case EntityBrowserPresenter<StateRecord> p:
                    p.SelectedRecordChanged += (_, selected) =>
                    {
                        _selectedState = selected;
                        if (selected is not null)
                        {
                            _buffsActions.StateId = selected.StateId;
                            _buffsActions.BuffName = selected.BuffName;
                        }
                    };
                    p.ErrorOccurred += OnPresenterError;
                    break;
                case EntityBrowserPresenter<NpcRecord> p:
                    p.SelectedRecordChanged += (_, selected) =>
                    {
                        _selectedNpc = selected;
                        if (selected is not null)
                        {
                            _npcsActions.NpcId = selected.NpcId;
                            _npcsActions.NpcName = selected.NpcTitle;
                            _npcsActions.ContactScript = selected.ContactScript ?? string.Empty;
                            _npcsActions.NpcX = (int)Math.Round(selected.X ?? 0);
                            _npcsActions.NpcY = (int)Math.Round(selected.Y ?? 0);
                        }
                    };
                    p.ErrorOccurred += OnPresenterError;
                    break;
                case EntityBrowserPresenter<SummonRecord> p:
                    p.SelectedRecordChanged += (_, selected) =>
                    {
                        _selectedSummon = selected;
                        if (selected is not null)
                        {
                            _summonsActions.SummonId = selected.SummonId;
                        }
                    };
                    p.ErrorOccurred += OnPresenterError;
                    break;
            }
        }
    }

    private void WireActionEvents()
    {
        _playerCheckerActions.LoadInventoryRequested += PlayerCheckerActions_LoadInventoryRequested;
        _playerCheckerActions.LoadWarehouseRequested += PlayerCheckerActions_LoadWarehouseRequested;
        _playerCheckerActions.OpenInfosRequested += PlayerCheckerActions_OpenInfosRequested;
        _playerCheckerActions.LoadAllCharactersRequested += PlayerCheckerActions_LoadAllCharactersRequested;
        _playerCheckerActions.LoadOnlineCharactersRequested += PlayerCheckerActions_LoadOnlineCharactersRequested;
        _monsterActions.CreateCommandRequested += MonsterActions_CreateCommandRequested;
        _itemsActions.AddYourselfRequested += ItemsActions_AddYourselfRequested;
        _itemsActions.GiveOtherPlayerRequested += ItemsActions_GiveOtherPlayerRequested;
        _itemsActions.EditLevelRequested += ItemsActions_EditLevelRequested;
        _itemsActions.EditEnhanceRequested += ItemsActions_EditEnhanceRequested;
        _itemsActions.ChangeAppearanceRequested += ItemsActions_ChangeAppearanceRequested;
        _itemsActions.ChangeItemCodeRequested += ItemsActions_ChangeItemCodeRequested;
        _skillsActions.LearnSkillRequested += SkillsActions_LearnSkillRequested;
        _skillsActions.LearnAllJobSkillsRequested += SkillsActions_LearnAllJobSkillsRequested;
        _skillsActions.LearnCreatureSkillRequested += SkillsActions_LearnCreatureSkillRequested;
        _skillsActions.SetSkillRequested += SkillsActions_SetSkillRequested;
        _skillsActions.RemoveSkillRequested += SkillsActions_RemoveSkillRequested;
        _skillsActions.LearnCreatureAllSkillRequested += SkillsActions_LearnCreatureAllSkillRequested;
        _buffsActions.AddTimedWorldStateRequested += BuffsActions_AddTimedWorldStateRequested;
        _buffsActions.AddEventStateRequested += BuffsActions_AddEventStateRequested;
        _buffsActions.RemoveEventStateRequested += BuffsActions_RemoveEventStateRequested;
        _buffsActions.AddPlayerOrCreatureBuffRequested += BuffsActions_AddPlayerOrCreatureBuffRequested;
        _buffsActions.RemovePlayerOrCreatureBuffRequested += BuffsActions_RemovePlayerOrCreatureBuffRequested;
        _npcsActions.CreateAddNpcToWorldCommandRequested += NpcsActions_CreateAddNpcToWorldCommandRequested;
        _npcsActions.CreateShowNpcCommandRequested += NpcsActions_CreateShowNpcCommandRequested;
        _npcsActions.CreateWarpToNpcCommandRequested += NpcsActions_CreateWarpToNpcCommandRequested;
        _summonsActions.AddSummonRequested += SummonsActions_AddSummonRequested;
        _summonsActions.StageSummonRequested += SummonsActions_StageSummonRequested;
    }

    private async void MainForm_Load(object? sender, EventArgs e)
    {
        try
        {
            _settings = await _settingsService.LoadAsync();
            EnsureSettingsDefaults();
            ApplyEnvironmentDefaults();
            ApplySettingsToUi();
        }
        catch (Exception ex)
        {
            ShowError("Failed to load app settings.", ex);
        }
    }

    private void ApplyEnvironmentDefaults()
    {
        var providerRaw = Environment.GetEnvironmentVariable(DbProviderEnvKey);
        if (!string.IsNullOrWhiteSpace(providerRaw)
            && Enum.TryParse<DatabaseProvider>(providerRaw, ignoreCase: true, out var provider))
        {
            _settings.Provider = provider;
        }

        var connectionString = Environment.GetEnvironmentVariable(DbConnectionStringEnvKey);
        if (!string.IsNullOrWhiteSpace(connectionString))
        {
            _settings.ConnectionString = connectionString.Trim();
            if (_connectionStringBuilder.TryParse(_settings.Provider, _settings.ConnectionString, out var parsed))
            {
                _settings.Connection = parsed;
            }
        }
    }

    private void EnsureSettingsDefaults()
    {
        _settings.Connection ??= new();
        _settings.TableNames ??= new();
        _settings.Players ??= [];
    }

    private void ApplySettingsToUi()
    {
        chkAppendCommands.Checked = _settings.AppendGeneratedCommands;

        cmbPlayers.BeginUpdate();
        cmbPlayers.Items.Clear();
        foreach (var player in _settings.Players)
        {
            cmbPlayers.Items.Add(player);
        }

        if (!string.IsNullOrWhiteSpace(_settings.SelectedPlayer))
        {
            cmbPlayers.SelectedItem = _settings.SelectedPlayer;
        }
        cmbPlayers.EndUpdate();
    }

    private string GetConfiguredConnectionString()
    {
        if (!string.IsNullOrWhiteSpace(_settings.Connection.Server)
            && !string.IsNullOrWhiteSpace(_settings.Connection.Database))
        {
            var built = _connectionStringBuilder.Build(_settings.Provider, _settings.Connection);
            _settings.ConnectionString = built;
            return built;
        }

        if (string.IsNullOrWhiteSpace(_settings.ConnectionString))
        {
            throw new InvalidOperationException("Connection string is empty. Open Settings and configure the database provider/connection.");
        }

        return _settings.ConnectionString;
    }

    private IReadOnlyDictionary<string, string> GetQueryTokens()
    {
        return _settings.TableNames.ToTokenMap();
    }

    private void OnPresenterError(object? sender, Exception ex)
    {
        ShowError("Data operation failed.", ex);
    }

    private async void PlayerCheckerActions_LoadInventoryRequested(object? sender, EventArgs e)
    {
        if (_selectedPlayerRecord is null)
        {
            MessageBox.Show(this, "Select a player first.", "Load Inventory", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            var items = await _repository.GetInventoryAsync(
                _settings.Provider,
                GetConfiguredConnectionString(),
                _selectedPlayerRecord.PlayerId,
                GetQueryTokens());

            PopulateInventoryGrid(items, $"Inventory — {_selectedPlayerRecord.PlayerName} ({items.Count} item(s))");
        }
        catch (Exception ex)
        {
            ShowError("Failed to load inventory.", ex);
        }
    }

    private async void PlayerCheckerActions_LoadWarehouseRequested(object? sender, EventArgs e)
    {
        if (_selectedPlayerRecord is null)
        {
            MessageBox.Show(this, "Select a player first.", "Load Warehouse", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            var items = await _repository.GetWarehouseAsync(
                _settings.Provider,
                GetConfiguredConnectionString(),
                _selectedPlayerRecord.Account,
                GetQueryTokens());

            PopulateInventoryGrid(items, $"Warehouse — {_selectedPlayerRecord.Account} ({items.Count} item(s))");
        }
        catch (Exception ex)
        {
            ShowError("Failed to load warehouse.", ex);
        }
    }

    private void PlayerCheckerActions_OpenInfosRequested(object? sender, EventArgs e)
    {
        if (_selectedPlayerRecord is null)
        {
            MessageBox.Show(this, "Select a player first.", "Player Info", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var r = _selectedPlayerRecord;
        MessageBox.Show(
            this,
            $"Name: {r.PlayerName}\nAccount: {r.Account}\nLevel: {r.Level}\nJob: {r.JobName}\nStatus: {r.OnlineStatus}",
            "Player Info",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }

    private async void PlayerCheckerActions_LoadAllCharactersRequested(object? sender, EventArgs e)
    {
        if (_playerPresenter is null)
        {
            return;
        }

        try
        {
            await _playerPresenter.LoadExternalAsync(
                ct => _repository.GetAllCharactersAsync(_settings.Provider, GetConfiguredConnectionString(), GetQueryTokens(), ct));
        }
        catch (Exception ex)
        {
            ShowError("Failed to load all characters.", ex);
        }
    }

    private async void PlayerCheckerActions_LoadOnlineCharactersRequested(object? sender, EventArgs e)
    {
        if (_playerPresenter is null)
        {
            return;
        }

        try
        {
            await _playerPresenter.LoadExternalAsync(
                ct => _repository.GetOnlineCharactersAsync(_settings.Provider, GetConfiguredConnectionString(), GetQueryTokens(), ct));
        }
        catch (Exception ex)
        {
            ShowError("Failed to load online characters.", ex);
        }
    }

    private void PopulateInventoryGrid(IReadOnlyList<InventoryItemRecord> items, string headerLabel)
    {
        _inventorySortColumnIndex = -1;
        _inventorySortOrder = SortOrder.None;

        gridInventory.SuspendLayout();
        gridInventory.Rows.Clear();
        gridInventory.Columns.Clear();

        gridInventory.Columns.Add(NewCol("itemId", "Item ID", 90));
        gridInventory.Columns.Add(NewCol("itemName", "Name", 300, fill: true));
        gridInventory.Columns.Add(NewCol("count", "Count", 70));
        gridInventory.Columns.Add(NewCol("level", "Level", 70));
        gridInventory.Columns.Add(NewCol("enhance", "Enhance", 75));
        gridInventory.Columns.Add(NewCol("wearInfo", "Wear", 60));

        foreach (var item in items)
        {
            gridInventory.Rows.Add(item.ItemId, item.ItemName, item.Count, item.Level, item.Enhance, item.WearInfo);
        }

        gridInventory.ResumeLayout();
    }

    private static DataGridViewTextBoxColumn NewCol(string name, string header, int width, bool fill = false)
        => new()
        {
            Name = name,
            HeaderText = header,
            Width = width,
            ReadOnly = true,
            SortMode = DataGridViewColumnSortMode.Programmatic,
            AutoSizeMode = fill ? DataGridViewAutoSizeColumnMode.Fill : DataGridViewAutoSizeColumnMode.None,
            MinimumWidth = width / 2
        };

    private void MonsterActions_CreateCommandRequested(object? sender, EventArgs e)
    {
        var monsterId = _monsterActions.MonsterId;
        if (monsterId <= 0)
        {
            if (_selectedMonster is null)
            {
                MessageBox.Show(this, "Select a monster or enter Monster ID first.", "Monster", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            monsterId = _selectedMonster.Id;
        }

        var count = _monsterActions.Amount;
        var minutesLifetime = _monsterActions.UseLifetime ? _monsterActions.MinutesLifetime : -1;

        if (_monsterActions.SpawnMode.Equals("At your place", StringComparison.OrdinalIgnoreCase))
        {
            BuildAndDispatchCommand("monsterRegenerate", new Dictionary<string, object?>
            {
                ["monsterId"] = monsterId,
                ["count"] = count
            });
            return;
        }

        if (_monsterActions.SpawnMode.Equals("At selected player place", StringComparison.OrdinalIgnoreCase))
        {
            var selectedPlayer = ResolveTargetPlayer();
            if (selectedPlayer.Equals("self", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(this, "Select player in the right sidebar for 'At selected player place'.", "Monster", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            BuildAndDispatchCommand("monsterAddNpcAtPlayer", new Dictionary<string, object?>
            {
                ["playerName"] = EscapeLuaSingleQuotedString(selectedPlayer),
                ["monsterId"] = monsterId,
                ["count"] = count,
                ["minutesLifetime"] = minutesLifetime
            });
            return;
        }

        BuildAndDispatchCommand("monsterAddNpcAtCoords", new Dictionary<string, object?>
        {
            ["x"] = _monsterActions.X,
            ["y"] = _monsterActions.Y,
            ["monsterId"] = monsterId,
            ["count"] = count,
            ["minutesLifetime"] = minutesLifetime,
            ["layer"] = _monsterActions.Layer
        });
    }

    private void ItemsActions_AddYourselfRequested(object? sender, EventArgs e)
    {
        var itemId = ResolveItemId();
        if (itemId <= 0)
        {
            return;
        }

        BuildAndDispatchCommand("insertItemSelf", new Dictionary<string, object?>
        {
            ["itemId"] = itemId,
            ["amount"] = _itemsActions.Amount,
            ["enhance"] = _itemsActions.Enhance,
            ["level"] = _itemsActions.Level,
            ["statusFlag"] = _itemsActions.StatusFlag
        });
    }

    private void ItemsActions_GiveOtherPlayerRequested(object? sender, EventArgs e)
    {
        var itemId = ResolveItemId();
        if (itemId <= 0)
        {
            return;
        }

        var targetPlayer = ResolveTargetPlayer();
        if (targetPlayer.Equals("self", StringComparison.OrdinalIgnoreCase))
        {
            MessageBox.Show(this, "Select player in the right sidebar for 'Give other player'.", "Items", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        BuildAndDispatchCommand("insertItemPlayer", new Dictionary<string, object?>
        {
            ["itemId"] = itemId,
            ["amount"] = _itemsActions.Amount,
            ["enhance"] = _itemsActions.Enhance,
            ["level"] = _itemsActions.Level,
            ["statusFlag"] = _itemsActions.StatusFlag,
            ["playerName"] = EscapeLuaSingleQuotedString(targetPlayer)
        });
    }

    private void ItemsActions_EditLevelRequested(object? sender, EventArgs e)
    {
        if (_itemsActions.ApplyToOtherPlayer)
        {
            var targetPlayer = ResolveTargetPlayer();
            if (targetPlayer.Equals("self", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(this, "Select player in the right sidebar for 'Other' mode.", "Items", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            BuildAndDispatchCommand("setWearItemLevelPlayer", new Dictionary<string, object?>
            {
                ["wearSlot"] = _itemsActions.WearSlotIndex,
                ["playerName"] = EscapeLuaSingleQuotedString(targetPlayer),
                ["level"] = _itemsActions.ModifyLevel
            });
            return;
        }

        BuildAndDispatchCommand("setWearItemLevelOwn", new Dictionary<string, object?>
        {
            ["wearSlot"] = _itemsActions.WearSlotIndex,
            ["level"] = _itemsActions.ModifyLevel
        });
    }

    private void ItemsActions_EditEnhanceRequested(object? sender, EventArgs e)
    {
        if (_itemsActions.ApplyToOtherPlayer)
        {
            var targetPlayer = ResolveTargetPlayer();
            if (targetPlayer.Equals("self", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(this, "Select player in the right sidebar for 'Other' mode.", "Items", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            BuildAndDispatchCommand("setWearItemEnhancePlayer", new Dictionary<string, object?>
            {
                ["wearSlot"] = _itemsActions.WearSlotIndex,
                ["playerName"] = EscapeLuaSingleQuotedString(targetPlayer),
                ["enhance"] = _itemsActions.ModifyEnhance
            });
            return;
        }

        BuildAndDispatchCommand("setWearItemEnhanceOwn", new Dictionary<string, object?>
        {
            ["wearSlot"] = _itemsActions.WearSlotIndex,
            ["enhance"] = _itemsActions.ModifyEnhance
        });
    }

    private void ItemsActions_ChangeAppearanceRequested(object? sender, EventArgs e)
    {
        var itemCode = _itemsActions.ModifyItemCode;
        if (itemCode <= 0)
        {
            MessageBox.Show(this, "Itemcode must be greater than 0.", "Items", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (_itemsActions.ApplyToOtherPlayer)
        {
            var targetPlayer = ResolveTargetPlayer();
            if (targetPlayer.Equals("self", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(this, "Select player in the right sidebar for 'Other' mode.", "Items", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            BuildAndDispatchCommand("setWearItemAppearancePlayer", new Dictionary<string, object?>
            {
                ["wearSlot"] = _itemsActions.WearSlotIndex,
                ["playerName"] = EscapeLuaSingleQuotedString(targetPlayer),
                ["itemCode"] = itemCode
            });
            return;
        }

        BuildAndDispatchCommand("setWearItemAppearanceOwn", new Dictionary<string, object?>
        {
            ["wearSlot"] = _itemsActions.WearSlotIndex,
            ["itemCode"] = itemCode
        });
    }

    private void ItemsActions_ChangeItemCodeRequested(object? sender, EventArgs e)
    {
        var itemCode = _itemsActions.ModifyItemCode;
        if (itemCode <= 0)
        {
            MessageBox.Show(this, "Itemcode must be greater than 0.", "Items", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        if (_itemsActions.ApplyToOtherPlayer)
        {
            var targetPlayer = ResolveTargetPlayer();
            if (targetPlayer.Equals("self", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(this, "Select player in the right sidebar for 'Other' mode.", "Items", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            BuildAndDispatchCommand("changeWearItemCodePlayer", new Dictionary<string, object?>
            {
                ["wearSlot"] = _itemsActions.WearSlotIndex,
                ["playerName"] = EscapeLuaSingleQuotedString(targetPlayer),
                ["itemCode"] = itemCode
            });
            return;
        }

        BuildAndDispatchCommand("changeWearItemCodeOwn", new Dictionary<string, object?>
        {
            ["wearSlot"] = _itemsActions.WearSlotIndex,
            ["itemCode"] = itemCode
        });
    }

    private int ResolveItemId()
    {
        var itemId = _itemsActions.ItemId;
        if (itemId > 0)
        {
            return itemId;
        }

        if (_selectedItem is not null)
        {
            return _selectedItem.ItemId;
        }

        MessageBox.Show(this, "Select item or enter Item ID first.", "Items", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return 0;
    }

    private int ResolveSkillId()
    {
        var skillId = _skillsActions.SkillId;
        if (skillId > 0)
        {
            return skillId;
        }

        if (_selectedSkill is not null)
        {
            return _selectedSkill.SkillId;
        }

        MessageBox.Show(this, "Select skill or enter Skill ID first.", "Skills", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return 0;
    }

    private int ResolveStateId()
    {
        var stateId = _buffsActions.StateId;
        if (stateId > 0)
        {
            return stateId;
        }

        if (_selectedState is not null)
        {
            return _selectedState.StateId;
        }

        MessageBox.Show(this, "Select buff/state or enter Buff-ID first.", "Buffs", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return 0;
    }

    private int ResolveNpcId()
    {
        var npcId = _npcsActions.NpcId;
        if (npcId > 0)
        {
            return npcId;
        }

        if (_selectedNpc is not null)
        {
            return _selectedNpc.NpcId;
        }

        MessageBox.Show(this, "Select npc or enter NPC ID first.", "NPC", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return 0;
    }

    private int ResolveSummonId()
    {
        var summonId = _summonsActions.SummonId;
        if (summonId > 0)
        {
            return summonId;
        }

        if (_selectedSummon is not null)
        {
            return _selectedSummon.SummonId;
        }

        MessageBox.Show(this, "Select summon or enter Summon ID first.", "Summons", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return 0;
    }

    private void SkillsActions_LearnSkillRequested(object? sender, EventArgs e)
    {
        var skillId = ResolveSkillId();
        if (skillId <= 0)
        {
            return;
        }

        var targetPlayer = ResolveTargetPlayer();
        if (targetPlayer.Equals("self", StringComparison.OrdinalIgnoreCase))
        {
            BuildAndDispatchCommand("learnSkill", new Dictionary<string, object?>
            {
                ["skillId"] = skillId
            });
            return;
        }

        BuildAndDispatchCommand("learnSkillForPlayer", new Dictionary<string, object?>
        {
            ["skillId"] = skillId,
            ["playerName"] = EscapeLuaSingleQuotedString(targetPlayer)
        });
    }

    private void SkillsActions_LearnAllJobSkillsRequested(object? sender, EventArgs e)
    {
        var targetPlayer = ResolveTargetPlayer();
        if (targetPlayer.Equals("self", StringComparison.OrdinalIgnoreCase))
        {
            MessageBox.Show(this, "Select player in the right sidebar for Learn all skill.", "Skills", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        BuildAndDispatchCommand("learnAllSkill", new Dictionary<string, object?>
        {
            ["playerName"] = EscapeLuaSingleQuotedString(targetPlayer)
        });
    }

    private void SkillsActions_LearnCreatureSkillRequested(object? sender, EventArgs e)
    {
        var skillId = ResolveSkillId();
        if (skillId <= 0)
        {
            return;
        }

        var targetPlayer = ResolveTargetPlayer();
        if (targetPlayer.Equals("self", StringComparison.OrdinalIgnoreCase))
        {
            BuildAndDispatchCommand("learnCreatureSkillSelf", new Dictionary<string, object?>
            {
                ["skillId"] = skillId
            });
            return;
        }

        BuildAndDispatchCommand("learnCreatureSkill", new Dictionary<string, object?>
        {
            ["skillId"] = skillId,
            ["playerName"] = EscapeLuaSingleQuotedString(targetPlayer)
        });
    }

    private void SkillsActions_SetSkillRequested(object? sender, EventArgs e)
    {
        var skillId = ResolveSkillId();
        if (skillId <= 0)
        {
            return;
        }

        var targetPlayer = ResolveTargetPlayer();
        if (targetPlayer.Equals("self", StringComparison.OrdinalIgnoreCase))
        {
            BuildAndDispatchCommand("setSkill", new Dictionary<string, object?>
            {
                ["skillId"] = skillId,
                ["level"] = _skillsActions.SkillLevel
            });
            return;
        }

        BuildAndDispatchCommand("setSkillForPlayer", new Dictionary<string, object?>
        {
            ["skillId"] = skillId,
            ["level"] = _skillsActions.SkillLevel,
            ["playerName"] = EscapeLuaSingleQuotedString(targetPlayer)
        });
    }

    private void SkillsActions_RemoveSkillRequested(object? sender, EventArgs e)
    {
        var skillId = ResolveSkillId();
        if (skillId <= 0)
        {
            return;
        }

        var targetPlayer = ResolveTargetPlayer();
        if (targetPlayer.Equals("self", StringComparison.OrdinalIgnoreCase))
        {
            BuildAndDispatchCommand("removeSkill", new Dictionary<string, object?>
            {
                ["skillId"] = skillId
            });
            return;
        }

        BuildAndDispatchCommand("removeSkillForPlayer", new Dictionary<string, object?>
        {
            ["skillId"] = skillId,
            ["playerName"] = EscapeLuaSingleQuotedString(targetPlayer)
        });
    }

    private void SkillsActions_LearnCreatureAllSkillRequested(object? sender, EventArgs e)
    {
        var slotIndex = _skillsActions.CreatureSlotIndex;
        var targetPlayer = ResolveTargetPlayer();
        if (targetPlayer.Equals("self", StringComparison.OrdinalIgnoreCase))
        {
            BuildAndDispatchCommand("learnCreatureAllSkill", new Dictionary<string, object?>
            {
                ["slotIndex"] = slotIndex
            });
            return;
        }

        BuildAndDispatchCommand("learnCreatureAllSkillForPlayer", new Dictionary<string, object?>
        {
            ["slotIndex"] = slotIndex,
            ["playerName"] = EscapeLuaSingleQuotedString(targetPlayer)
        });
    }

    private void BuffsActions_AddTimedWorldStateRequested(object? sender, EventArgs e)
    {
        var stateId = ResolveStateId();
        if (stateId <= 0)
        {
            return;
        }

        BuildAndDispatchCommand("castWorldState", new Dictionary<string, object?>
        {
            ["stateId"] = stateId,
            ["level"] = _buffsActions.BuffLevel,
            ["durationMinutes"] = _buffsActions.DurationMinutes
        });
    }

    private void BuffsActions_AddEventStateRequested(object? sender, EventArgs e)
    {
        var stateId = ResolveStateId();
        if (stateId <= 0)
        {
            return;
        }

        BuildAndDispatchCommand("addEventState", new Dictionary<string, object?>
        {
            ["stateId"] = stateId,
            ["level"] = _buffsActions.BuffLevel
        });
    }

    private void BuffsActions_RemoveEventStateRequested(object? sender, EventArgs e)
    {
        var stateId = ResolveStateId();
        if (stateId <= 0)
        {
            return;
        }

        BuildAndDispatchCommand("removeEventState", new Dictionary<string, object?>
        {
            ["stateId"] = stateId
        });
    }

    private void BuffsActions_AddPlayerOrCreatureBuffRequested(object? sender, EventArgs e)
    {
        var stateId = ResolveStateId();
        if (stateId <= 0)
        {
            return;
        }

        var targetPlayer = ResolveTargetPlayer();
        if (targetPlayer.Equals("self", StringComparison.OrdinalIgnoreCase))
        {
            MessageBox.Show(this, "Select player in the right sidebar.", "Buffs", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var template = _buffsActions.IsSummonTarget ? "addCreatureState" : "addPlayerState";
        BuildAndDispatchCommand(template, new Dictionary<string, object?>
        {
            ["stateId"] = stateId,
            ["level"] = _buffsActions.BuffLevel,
            ["durationMinutes"] = _buffsActions.DurationMinutes,
            ["playerName"] = EscapeLuaSingleQuotedString(targetPlayer)
        });
    }

    private void BuffsActions_RemovePlayerOrCreatureBuffRequested(object? sender, EventArgs e)
    {
        var stateId = ResolveStateId();
        if (stateId <= 0)
        {
            return;
        }

        var targetPlayer = ResolveTargetPlayer();
        if (targetPlayer.Equals("self", StringComparison.OrdinalIgnoreCase))
        {
            MessageBox.Show(this, "Select player in the right sidebar.", "Buffs", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var template = _buffsActions.IsSummonTarget ? "removeCreatureState" : "removePlayerState";
        BuildAndDispatchCommand(template, new Dictionary<string, object?>
        {
            ["stateId"] = stateId,
            ["playerName"] = EscapeLuaSingleQuotedString(targetPlayer)
        });
    }

    private void NpcsActions_CreateAddNpcToWorldCommandRequested(object? sender, EventArgs e)
    {
        var npcId = ResolveNpcId();
        if (npcId <= 0)
        {
            return;
        }

        var targetPlayer = ResolveTargetPlayer();
        if (targetPlayer.Equals("self", StringComparison.OrdinalIgnoreCase))
        {
            BuildAndDispatchCommand("addNpcToWorld", new Dictionary<string, object?>
            {
                ["x"] = _npcsActions.NpcX,
                ["y"] = _npcsActions.NpcY,
                ["layer"] = _npcsActions.Layer,
                ["npcId"] = npcId
            });
            return;
        }

        BuildAndDispatchCommand("addNpcToWorldForPlayer", new Dictionary<string, object?>
        {
            ["x"] = _npcsActions.NpcX,
            ["y"] = _npcsActions.NpcY,
            ["layer"] = _npcsActions.Layer,
            ["playerName"] = EscapeLuaSingleQuotedString(targetPlayer),
            ["npcId"] = npcId
        });
    }

    private void NpcsActions_CreateShowNpcCommandRequested(object? sender, EventArgs e)
    {
        var npcId = ResolveNpcId();
        if (npcId <= 0)
        {
            return;
        }

        BuildAndDispatchCommand("showNpc", new Dictionary<string, object?>
        {
            ["x"] = _npcsActions.NpcX,
            ["y"] = _npcsActions.NpcY,
            ["npcId"] = npcId,
            ["layer"] = _npcsActions.Layer,
            ["visible"] = _npcsActions.VisibleFlag
        });
    }

    private void NpcsActions_CreateWarpToNpcCommandRequested(object? sender, EventArgs e)
    {
        var targetPlayer = ResolveTargetPlayer();
        if (targetPlayer.Equals("self", StringComparison.OrdinalIgnoreCase))
        {
            MessageBox.Show(this, "Select player in the right sidebar for warp to NPC.", "NPC", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        BuildAndDispatchCommand("warpToNpcCoordinates", new Dictionary<string, object?>
        {
            ["x"] = _npcsActions.NpcX,
            ["y"] = _npcsActions.NpcY,
            ["playerName"] = EscapeLuaDoubleQuotedString(targetPlayer)
        });
    }

    private void SummonsActions_AddSummonRequested(object? sender, EventArgs e)
    {
        var summonId = ResolveSummonId();
        if (summonId <= 0)
        {
            return;
        }

        if (_summonsActions.UseAddSummonStage)
        {
            BuildAndDispatchCommand("insertSummonByIdWithStage", new Dictionary<string, object?>
            {
                ["summonId"] = summonId,
                ["stage"] = _summonsActions.AddSummonStage
            });
            return;
        }

        BuildAndDispatchCommand("insertSummonById", new Dictionary<string, object?>
        {
            ["summonId"] = summonId
        });
    }

    private void SummonsActions_StageSummonRequested(object? sender, EventArgs e)
    {
        BuildAndDispatchCommand("stageSummon", new Dictionary<string, object?>
        {
            ["slot"] = _summonsActions.Slot,
            ["stage"] = _summonsActions.Stage
        });
    }

    private void BuildAndDispatchCommand(string templateKey, IReadOnlyDictionary<string, object?> values)
    {
        try
        {
            var command = _luaCommandBuilder.Build(templateKey, values);
            command = ApplyRunPrefixIfEnabled(command);
            Clipboard.SetText(command);
        }
        catch (Exception ex)
        {
            ShowError("Failed to generate command.", ex);
        }
    }

    private string ApplyRunPrefixIfEnabled(string command)
    {
        if (!chkAppendCommands.Checked)
        {
            return command;
        }

        var trimmed = command.TrimStart();
        if (trimmed.StartsWith("//", StringComparison.Ordinal)
            || trimmed.StartsWith("/run ", StringComparison.OrdinalIgnoreCase))
        {
            return command;
        }

        return $"/run {command}";
    }

    private static string EscapeLuaSingleQuotedString(string value)
        => value.Replace("\\", "\\\\", StringComparison.Ordinal).Replace("'", "\\'", StringComparison.Ordinal);

    private static string EscapeLuaDoubleQuotedString(string value)
        => value.Replace("\\", "\\\\", StringComparison.Ordinal).Replace("\"", "\\\"", StringComparison.Ordinal);

    private static bool RequireSelection<T>(T? selected, string entityName) where T : class
    {
        if (selected is not null)
        {
            return true;
        }

        MessageBox.Show($"Select a {entityName} record first.", "Selection required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        return false;
    }

    private string ResolveTargetPlayer(string? explicitTarget = null)
    {
        if (!string.IsNullOrWhiteSpace(explicitTarget))
        {
            return explicitTarget.Trim();
        }

        if (cmbPlayers.SelectedItem is string selected && !string.IsNullOrWhiteSpace(selected))
        {
            return selected.Trim();
        }

        return "self";
    }

    private void CommandHistoryService_CommandsChanged(object? sender, EventArgs e)
    {
        if (InvokeRequired)
        {
            BeginInvoke(RefreshCommandList);
            return;
        }

        RefreshCommandList();
    }

    private void RefreshCommandList()
    {
        lstCommands.BeginUpdate();
        lstCommands.Items.Clear();

        foreach (var command in _commandHistoryService.Commands)
        {
            lstCommands.Items.Add(command);
        }

        lstCommands.EndUpdate();
    }

    private void btnCopySelected_Click(object sender, EventArgs e)
    {
        if (lstCommands.SelectedItem is not string command || string.IsNullOrWhiteSpace(command))
        {
            return;
        }

        Clipboard.SetText(command);
    }

    private void btnCopyAll_Click(object sender, EventArgs e)
    {
        if (_commandHistoryService.Commands.Count == 0)
        {
            return;
        }

        Clipboard.SetText(string.Join(Environment.NewLine, _commandHistoryService.Commands));
    }

    private void btnClearCommands_Click(object sender, EventArgs e)
    {
        _commandHistoryService.Clear();
    }

    private async void btnSettings_Click(object sender, EventArgs e)
    {
        using var settingsForm = new SettingsForm(_repository, _connectionStringBuilder, _localCacheService, _settings);
        settingsForm.Icon = Icon;
        if (settingsForm.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        _settings = settingsForm.UpdatedSettings.Clone();

        await SaveSettingsSafeAsync();
    }

    private void btnAbout_Click(object sender, EventArgs e)
    {
        using var about = new AboutForm();
        about.ShowDialog(this);
    }

    private void btnAddPlayer_Click(object sender, EventArgs e)
    {
        var value = txtNewPlayer.Text.Trim();
        if (string.IsNullOrWhiteSpace(value))
        {
            return;
        }

        if (_settings.Players.All(player => !player.Equals(value, StringComparison.OrdinalIgnoreCase)))
        {
            _settings.Players.Add(value);
            cmbPlayers.Items.Add(value);
        }

        cmbPlayers.SelectedItem = value;
        txtNewPlayer.Clear();
        _ = SaveSettingsSafeAsync();
    }

    private void btnRemovePlayer_Click(object sender, EventArgs e)
    {
        if (cmbPlayers.SelectedItem is not string selected)
        {
            return;
        }

        _settings.Players.RemoveAll(player => player.Equals(selected, StringComparison.OrdinalIgnoreCase));
        cmbPlayers.Items.Remove(selected);
        _settings.SelectedPlayer = cmbPlayers.SelectedItem as string;

        _ = SaveSettingsSafeAsync();
    }

    private void cmbPlayers_SelectedIndexChanged(object sender, EventArgs e)
    {
        _settings.SelectedPlayer = cmbPlayers.SelectedItem as string;
        _ = SaveSettingsSafeAsync();
    }

    private void chkAppendCommands_CheckedChanged(object sender, EventArgs e)
    {
        _settings.AppendGeneratedCommands = chkAppendCommands.Checked;
        _ = SaveSettingsSafeAsync();
    }

    private async Task SaveSettingsSafeAsync()
    {
        try
        {
            _settings.SelectedPlayer = cmbPlayers.SelectedItem as string;
            _settings.AppendGeneratedCommands = chkAppendCommands.Checked;
            await _settingsService.SaveAsync(_settings).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to save settings.");
        }
    }

    private void MainForm_FormClosing(object? sender, FormClosingEventArgs e)
    {
        try
        {
            _settings.SelectedPlayer = cmbPlayers.SelectedItem as string;
            _settings.AppendGeneratedCommands = chkAppendCommands.Checked;

            // Save on a worker thread with timeout to avoid UI deadlocks during close.
            var persisted = Task.Run(() => _settingsService.SaveAsync(_settings)).Wait(TimeSpan.FromSeconds(2));
            if (!persisted)
            {
                Log.Warning("Settings save timed out during shutdown.");
            }
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to persist settings during shutdown.");
        }
    }

    private void ShowError(string message, Exception ex)
    {
        Log.Error(ex, message);
        MessageBox.Show(this, $"{message}{Environment.NewLine}{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
