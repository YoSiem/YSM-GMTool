using App.Core.Interfaces;
using App.Core.Models;
using App.Core.Models.Entities;
using App.WinForms.Controls;
using App.WinForms.Forms;
using App.WinForms.Models;
using App.WinForms.Presenters;
using Serilog;

namespace App.WinForms;

public partial class MainForm : Form
{
    private readonly IGameDataRepository _repository;
    private readonly IAppSettingsService _settingsService;
    private readonly ILuaCommandBuilder _luaCommandBuilder;
    private readonly ICommandHistoryService _commandHistoryService;
    private readonly INameNormalizer _nameNormalizer;

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

    public MainForm(
        IGameDataRepository repository,
        IAppSettingsService settingsService,
        ILuaCommandBuilder luaCommandBuilder,
        ICommandHistoryService commandHistoryService,
        INameNormalizer nameNormalizer)
    {
        _repository = repository;
        _settingsService = settingsService;
        _luaCommandBuilder = luaCommandBuilder;
        _commandHistoryService = commandHistoryService;
        _nameNormalizer = nameNormalizer;

        InitializeComponent();
        picAvatar.Image = SystemIcons.Shield.ToBitmap();

        AttachActionControls();
        ConfigureBrowserColumns();
        InitializePresenters();
        WireActionEvents();

        _commandHistoryService.CommandsChanged += CommandHistoryService_CommandsChanged;
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
        control.Dock = DockStyle.Top;
        browser.ActionsHostPanel.Controls.Add(control);
    }

    private void ConfigureBrowserColumns()
    {
        browserPlayerchecker.ConfigureColumns(
        [
            new BrowserColumnDefinition("playerId", "ID", 80),
            new BrowserColumnDefinition("playerName", "Name", 220, true),
            new BrowserColumnDefinition("level", "Level", 90),
            new BrowserColumnDefinition("job", "Job", 180)
        ]);

        browserMonster.ConfigureColumns(
        [
            new BrowserColumnDefinition("id", "ID", 80),
            new BrowserColumnDefinition("name", "Name", 260, true),
            new BrowserColumnDefinition("level", "Level", 90)
        ]);

        browserItems.ConfigureColumns(
        [
            new BrowserColumnDefinition("itemId", "ID", 80),
            new BrowserColumnDefinition("name", "Name", 320, true)
        ]);

        browserSkills.ConfigureColumns(
        [
            new BrowserColumnDefinition("skillId", "ID", 80),
            new BrowserColumnDefinition("skillName", "Name", 320, true)
        ]);

        browserBuffs.ConfigureColumns(
        [
            new BrowserColumnDefinition("stateId", "State ID", 100),
            new BrowserColumnDefinition("buffName", "Buff name", 320, true)
        ]);

        browserNpcs.ConfigureColumns(
        [
            new BrowserColumnDefinition("npcId", "NPC ID", 90),
            new BrowserColumnDefinition("npcTitle", "Title", 220, true),
            new BrowserColumnDefinition("x", "X", 90),
            new BrowserColumnDefinition("y", "Y", 90),
            new BrowserColumnDefinition("localFlag", "Flag", 90)
        ]);

        browserSummons.ConfigureColumns(
        [
            new BrowserColumnDefinition("summonId", "Summon ID", 100),
            new BrowserColumnDefinition("summonName", "Summon Name", 240, true),
            new BrowserColumnDefinition("cardName", "Card Name", 220, true)
        ]);
    }

    private void InitializePresenters()
    {
        _playerPresenter = new EntityBrowserPresenter<PlayerRecord>(
            browserPlayerchecker,
            ct => _repository.GetPlayersAsync(_settings.Provider, GetConfiguredConnectionString(), ct),
            x => x.PlayerId,
            x => x.PlayerName,
            x =>
            [
                x.PlayerId,
                x.PlayerName,
                x.Level,
                x.JobName ?? string.Empty
            ],
            _nameNormalizer);

        _monsterPresenter = new EntityBrowserPresenter<MonsterRecord>(
            browserMonster,
            ct => _repository.GetMonstersAsync(_settings.Provider, GetConfiguredConnectionString(), ct),
            x => x.Id,
            x => x.Name,
            x =>
            [
                x.Id,
                x.Name,
                x.Level ?? 0
            ],
            _nameNormalizer);

        _itemsPresenter = new EntityBrowserPresenter<ItemRecord>(
            browserItems,
            ct => _repository.GetItemsAsync(_settings.Provider, GetConfiguredConnectionString(), ct),
            x => x.ItemId,
            x => x.NameEn,
            x =>
            [
                x.ItemId,
                x.NameEn
            ],
            _nameNormalizer);

        _skillsPresenter = new EntityBrowserPresenter<SkillRecord>(
            browserSkills,
            ct => _repository.GetSkillsAsync(_settings.Provider, GetConfiguredConnectionString(), ct),
            x => x.SkillId,
            x => x.Skillname,
            x =>
            [
                x.SkillId,
                x.Skillname
            ],
            _nameNormalizer);

        _buffsPresenter = new EntityBrowserPresenter<StateRecord>(
            browserBuffs,
            ct => _repository.GetStatesAsync(_settings.Provider, GetConfiguredConnectionString(), ct),
            x => x.StateId,
            x => x.BuffName,
            x =>
            [
                x.StateId,
                x.BuffName
            ],
            _nameNormalizer);

        _npcsPresenter = new EntityBrowserPresenter<NpcRecord>(
            browserNpcs,
            ct => _repository.GetNpcsAsync(_settings.Provider, GetConfiguredConnectionString(), ct),
            x => x.NpcId,
            x => x.NpcTitle,
            x =>
            [
                x.NpcId,
                x.NpcTitle,
                x.X?.ToString("0.###") ?? string.Empty,
                x.Y?.ToString("0.###") ?? string.Empty,
                x.LocalFlag ?? 0
            ],
            _nameNormalizer);

        _summonsPresenter = new EntityBrowserPresenter<SummonRecord>(
            browserSummons,
            ct => _repository.GetSummonsAsync(_settings.Provider, GetConfiguredConnectionString(), ct),
            x => x.SummonId,
            x => x.SummonName,
            x =>
            [
                x.SummonId,
                x.SummonName,
                x.CardName ?? string.Empty
            ],
            _nameNormalizer);

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
                    p.SelectedRecordChanged += (_, selected) => _selectedMonster = selected;
                    p.ErrorOccurred += OnPresenterError;
                    break;
                case EntityBrowserPresenter<ItemRecord> p:
                    p.SelectedRecordChanged += (_, selected) => _selectedItem = selected;
                    p.ErrorOccurred += OnPresenterError;
                    break;
                case EntityBrowserPresenter<SkillRecord> p:
                    p.SelectedRecordChanged += (_, selected) => _selectedSkill = selected;
                    p.ErrorOccurred += OnPresenterError;
                    break;
                case EntityBrowserPresenter<StateRecord> p:
                    p.SelectedRecordChanged += (_, selected) => _selectedState = selected;
                    p.ErrorOccurred += OnPresenterError;
                    break;
                case EntityBrowserPresenter<NpcRecord> p:
                    p.SelectedRecordChanged += (_, selected) => _selectedNpc = selected;
                    p.ErrorOccurred += OnPresenterError;
                    break;
                case EntityBrowserPresenter<SummonRecord> p:
                    p.SelectedRecordChanged += (_, selected) => _selectedSummon = selected;
                    p.ErrorOccurred += OnPresenterError;
                    break;
            }
        }
    }

    private void WireActionEvents()
    {
        _playerCheckerActions.CreateCheckCommandRequested += PlayerCheckerActions_CreateCheckCommandRequested;
        _monsterActions.CreateCommandRequested += MonsterActions_CreateCommandRequested;
        _itemsActions.CreateGiveCommandRequested += ItemsActions_CreateGiveCommandRequested;
        _skillsActions.LearnSkillRequested += SkillsActions_LearnSkillRequested;
        _skillsActions.LearnAllJobSkillsRequested += SkillsActions_LearnAllJobSkillsRequested;
        _skillsActions.GetBasicSkillLevelRequested += SkillsActions_GetBasicSkillLevelRequested;
        _buffsActions.AddBuffRequested += BuffsActions_AddBuffRequested;
        _buffsActions.RemoveBuffRequested += BuffsActions_RemoveBuffRequested;
        _buffsActions.SetTimedWorldStateRequested += BuffsActions_SetTimedWorldStateRequested;
        _npcsActions.CreateNpcMoveCommandRequested += NpcsActions_CreateNpcMoveCommandRequested;
        _summonsActions.CreateSummonCommandRequested += SummonsActions_CreateSummonCommandRequested;
    }

    private async void MainForm_Load(object? sender, EventArgs e)
    {
        try
        {
            _settings = await _settingsService.LoadAsync();
            ApplySettingsToUi();
            RefreshCommandList();
        }
        catch (Exception ex)
        {
            ShowError("Failed to load app settings.", ex);
        }
    }

    private void ApplySettingsToUi()
    {
        lblProviderValue.Text = _settings.Provider.ToString();
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
        if (string.IsNullOrWhiteSpace(_settings.ConnectionString))
        {
            throw new InvalidOperationException("Connection string is empty. Open Settings and configure the database provider/connection.");
        }

        return _settings.ConnectionString;
    }

    private void OnPresenterError(object? sender, Exception ex)
    {
        ShowError("Data operation failed.", ex);
    }

    private void PlayerCheckerActions_CreateCheckCommandRequested(object? sender, EventArgs e)
    {
        var playerName = _playerCheckerActions.PlayerName;
        if (string.IsNullOrWhiteSpace(playerName))
        {
            playerName = _selectedPlayerRecord?.PlayerName ?? ResolveTargetPlayer();
        }

        if (string.IsNullOrWhiteSpace(playerName) || playerName.Equals("self", StringComparison.OrdinalIgnoreCase))
        {
            MessageBox.Show(this, "Select or type a player name first.", "Playerchecker", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        BuildAndDispatchCommand("playerCheck", new Dictionary<string, object?>
        {
            ["name"] = playerName
        });
    }

    private void MonsterActions_CreateCommandRequested(object? sender, EventArgs e)
    {
        if (!RequireSelection(_selectedMonster, "monster"))
        {
            return;
        }

        if (_monsterActions.SpawnMode.Equals("Coordinates", StringComparison.OrdinalIgnoreCase))
        {
            BuildAndDispatchCommand("spawnMonsterAtCoords", new Dictionary<string, object?>
            {
                ["id"] = _selectedMonster!.Id,
                ["amount"] = _monsterActions.Amount,
                ["x"] = _monsterActions.X,
                ["y"] = _monsterActions.Y,
                ["layer"] = _monsterActions.Layer
            });
        }
        else
        {
            BuildAndDispatchCommand("spawnMonsterAtPlayer", new Dictionary<string, object?>
            {
                ["id"] = _selectedMonster!.Id,
                ["amount"] = _monsterActions.Amount
            });
        }
    }

    private void ItemsActions_CreateGiveCommandRequested(object? sender, EventArgs e)
    {
        if (!RequireSelection(_selectedItem, "item"))
        {
            return;
        }

        if (_itemsActions.GiveToOtherPlayer)
        {
            var targetPlayer = ResolveTargetPlayer(_itemsActions.TargetPlayer);
            if (targetPlayer.Equals("self", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show(this, "Target player is required for 'Other player' mode.", "Items", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            BuildAndDispatchCommand("giveItemToPlayer", new Dictionary<string, object?>
            {
                ["target"] = targetPlayer,
                ["itemId"] = _selectedItem!.ItemId,
                ["amount"] = _itemsActions.Amount,
                ["level"] = _itemsActions.Level,
                ["enhance"] = _itemsActions.Enhance,
                ["itmcode"] = _itemsActions.ItmCode
            });
        }
        else
        {
            BuildAndDispatchCommand("addItemToSelf", new Dictionary<string, object?>
            {
                ["itemId"] = _selectedItem!.ItemId,
                ["amount"] = _itemsActions.Amount,
                ["level"] = _itemsActions.Level,
                ["enhance"] = _itemsActions.Enhance,
                ["itmcode"] = _itemsActions.ItmCode
            });
        }
    }

    private void SkillsActions_LearnSkillRequested(object? sender, EventArgs e)
    {
        if (!RequireSelection(_selectedSkill, "skill"))
        {
            return;
        }

        BuildAndDispatchCommand("learnSkill", new Dictionary<string, object?>
        {
            ["target"] = ResolveTargetPlayer(_skillsActions.TargetPlayer),
            ["skillId"] = _selectedSkill!.SkillId
        });
    }

    private void SkillsActions_LearnAllJobSkillsRequested(object? sender, EventArgs e)
    {
        BuildAndDispatchCommand("learnAllJobSkills", new Dictionary<string, object?>
        {
            ["target"] = ResolveTargetPlayer(_skillsActions.TargetPlayer)
        });
    }

    private void SkillsActions_GetBasicSkillLevelRequested(object? sender, EventArgs e)
    {
        if (!RequireSelection(_selectedSkill, "skill"))
        {
            return;
        }

        BuildAndDispatchCommand("getBasicSkillLevel", new Dictionary<string, object?>
        {
            ["target"] = ResolveTargetPlayer(_skillsActions.TargetPlayer),
            ["skillId"] = _selectedSkill!.SkillId
        });
    }

    private void BuffsActions_AddBuffRequested(object? sender, EventArgs e)
    {
        if (!RequireSelection(_selectedState, "state/buff"))
        {
            return;
        }

        BuildAndDispatchCommand("addBuff", new Dictionary<string, object?>
        {
            ["target"] = ResolveTargetPlayer(),
            ["stateId"] = _selectedState!.StateId,
            ["duration"] = _buffsActions.DurationSeconds
        });
    }

    private void BuffsActions_RemoveBuffRequested(object? sender, EventArgs e)
    {
        if (!RequireSelection(_selectedState, "state/buff"))
        {
            return;
        }

        BuildAndDispatchCommand("removeBuff", new Dictionary<string, object?>
        {
            ["target"] = ResolveTargetPlayer(),
            ["stateId"] = _selectedState!.StateId
        });
    }

    private void BuffsActions_SetTimedWorldStateRequested(object? sender, EventArgs e)
    {
        if (!RequireSelection(_selectedState, "state/buff"))
        {
            return;
        }

        BuildAndDispatchCommand("setTimedWorldState", new Dictionary<string, object?>
        {
            ["stateId"] = _selectedState!.StateId,
            ["value"] = string.IsNullOrWhiteSpace(_buffsActions.WorldStateValue) ? "1" : _buffsActions.WorldStateValue,
            ["duration"] = _buffsActions.DurationSeconds
        });
    }

    private void NpcsActions_CreateNpcMoveCommandRequested(object? sender, EventArgs e)
    {
        if (!RequireSelection(_selectedNpc, "npc"))
        {
            return;
        }

        BuildAndDispatchCommand("moveToNpc", new Dictionary<string, object?>
        {
            ["npcId"] = _selectedNpc!.NpcId,
            ["x"] = _selectedNpc.X ?? 0,
            ["y"] = _selectedNpc.Y ?? 0,
            ["layer"] = _npcsActions.Layer
        });
    }

    private void SummonsActions_CreateSummonCommandRequested(object? sender, EventArgs e)
    {
        if (!RequireSelection(_selectedSummon, "summon"))
        {
            return;
        }

        BuildAndDispatchCommand("spawnSummon", new Dictionary<string, object?>
        {
            ["summonId"] = _selectedSummon!.SummonId,
            ["amount"] = _summonsActions.Amount
        });
    }

    private void BuildAndDispatchCommand(string templateKey, IReadOnlyDictionary<string, object?> values)
    {
        try
        {
            var command = _luaCommandBuilder.Build(templateKey, values);
            Clipboard.SetText(command);

            if (chkAppendCommands.Checked)
            {
                _commandHistoryService.Add(command);
            }
        }
        catch (Exception ex)
        {
            ShowError("Failed to generate command.", ex);
        }
    }

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
        using var settingsForm = new SettingsForm(_repository, _settings.Provider, _settings.ConnectionString);
        if (settingsForm.ShowDialog(this) != DialogResult.OK)
        {
            return;
        }

        _settings.Provider = settingsForm.SelectedProvider;
        _settings.ConnectionString = settingsForm.ConnectionString;
        lblProviderValue.Text = _settings.Provider.ToString();

        await SaveSettingsSafeAsync();
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
            await _settingsService.SaveAsync(_settings);
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
            _settingsService.SaveAsync(_settings).GetAwaiter().GetResult();
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
