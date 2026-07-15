using System;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;
using App.Core.Abstractions;
using App.Core.Enums;
using App.Core.Models;
using App.Desktop.Infrastructure;
using App.Desktop.Services;
using ReactiveUI;
using Serilog;

namespace App.Desktop.Features.Settings;

/// <summary>
/// View model for the Settings dialog. Edits a working <see cref="AppSettings.Clone"/>; on Save it
/// validates, builds the connection string, writes the <c>.env</c>, applies the result to the live
/// settings holder (re-seeding player context + icon cache), and persists.
/// </summary>
public sealed class SettingsViewModel : ReactiveObject
{
    private static readonly (string Key, string Label)[] CacheEntities =
    [
        ("monsters", "Monsters"),
        ("items", "Items"),
        ("skills", "Skills"),
        ("states", "Buffs/States"),
        ("npcs", "NPCs"),
        ("summons", "Summons"),
    ];

    private readonly IGameDataRepository _repository;
    private readonly IConnectionStringBuilderService _connectionStringBuilder;
    private readonly ILocalCacheService _localCacheService;
    private readonly IAppSettingsService _settingsService;
    private readonly IAppSettingsHolder _holder;
    private readonly IDialogService _dlg;
    private readonly AppSettings _working;

    public SettingsViewModel(
        IGameDataRepository repository,
        IConnectionStringBuilderService connectionStringBuilder,
        ILocalCacheService localCacheService,
        IAppSettingsService settingsService,
        IAppSettingsHolder holder,
        IDialogService dlg)
    {
        _repository = repository;
        _connectionStringBuilder = connectionStringBuilder;
        _localCacheService = localCacheService;
        _settingsService = settingsService;
        _holder = holder;
        _dlg = dlg;
        _working = holder.Current.Clone();

        TestConnection = ReactiveCommand.CreateFromTask(TestConnectionAsync);
        Save = ReactiveCommand.CreateFromTask(SaveAsync);
        Cancel = ReactiveCommand.Create(() => CloseRequested?.Invoke(this, false));
        ExportCache = ReactiveCommand.CreateFromTask(ExportCacheAsync);
        Browse = ReactiveCommand.CreateFromTask(BrowseAsync);

        TestConnection.ThrownExceptions.Subscribe(ex => Log.Warning(ex, "Settings: test connection failed."));
        Save.ThrownExceptions.Subscribe(ex => Log.Warning(ex, "Settings: save failed."));
        ExportCache.ThrownExceptions.Subscribe(ex => Log.Warning(ex, "Settings: cache export failed."));
        Browse.ThrownExceptions.Subscribe(ex => Log.Warning(ex, "Settings: browse failed."));

        LoadFromWorking();
        RefreshCacheStatus();
    }

    /// <summary>Raised when the dialog should close. Argument: whether settings were saved.</summary>
    public event EventHandler<bool>? CloseRequested;

    /// <summary>Supplies a folder picker (set by the view, which has access to the storage provider).</summary>
    public Func<string?, Task<string?>>? FolderPicker { get; set; }

    public DatabaseProvider[] Providers { get; } = Enum.GetValues<DatabaseProvider>();

    public ReactiveCommand<Unit, Unit> TestConnection { get; }

    public ReactiveCommand<Unit, Unit> Save { get; }

    public ReactiveCommand<Unit, Unit> Cancel { get; }

    public ReactiveCommand<Unit, Unit> ExportCache { get; }

    public ReactiveCommand<Unit, Unit> Browse { get; }

    // --- Connection. ---

    private DatabaseProvider _provider;

    public DatabaseProvider Provider
    {
        get => _provider;
        set
        {
            if (_provider == value)
            {
                return;
            }

            this.RaiseAndSetIfChanged(ref _provider, value);
            OnProviderChanged();
        }
    }

    private string _server = string.Empty;

    public string Server
    {
        get => _server;
        set => this.RaiseAndSetIfChanged(ref _server, value);
    }

    private int _port = 1433;

    public int Port
    {
        get => _port;
        set => this.RaiseAndSetIfChanged(ref _port, value);
    }

    private string _database = string.Empty;

    public string Database
    {
        get => _database;
        set => this.RaiseAndSetIfChanged(ref _database, value);
    }

    private string _userId = string.Empty;

    public string UserId
    {
        get => _userId;
        set => this.RaiseAndSetIfChanged(ref _userId, value);
    }

    private string _password = string.Empty;

    public string Password
    {
        get => _password;
        set => this.RaiseAndSetIfChanged(ref _password, value);
    }

    private bool _integratedSecurity;

    public bool IntegratedSecurity
    {
        get => _integratedSecurity;
        set
        {
            this.RaiseAndSetIfChanged(ref _integratedSecurity, value);
            this.RaisePropertyChanged(nameof(SqlAuthEnabled));
        }
    }

    private bool _encrypt = true;

    public bool Encrypt
    {
        get => _encrypt;
        set => this.RaiseAndSetIfChanged(ref _encrypt, value);
    }

    private bool _trustServerCertificate = true;

    public bool TrustServerCertificate
    {
        get => _trustServerCertificate;
        set => this.RaiseAndSetIfChanged(ref _trustServerCertificate, value);
    }

    /// <summary>MSSQL-only options (integrated security / encrypt / trust certificate).</summary>
    public bool IsMsSql => Provider == DatabaseProvider.MSSQL;

    /// <summary>User ID/password are used unless MSSQL integrated security is on.</summary>
    public bool SqlAuthEnabled => !IsMsSql || !IntegratedSecurity;

    // --- Table names. ---

    private string _arcadiaName = string.Empty;
    public string ArcadiaName { get => _arcadiaName; set => this.RaiseAndSetIfChanged(ref _arcadiaName, value); }

    private string _telecasterName = string.Empty;
    public string TelecasterName { get => _telecasterName; set => this.RaiseAndSetIfChanged(ref _telecasterName, value); }

    private string _authName = string.Empty;
    public string AuthName { get => _authName; set => this.RaiseAndSetIfChanged(ref _authName, value); }

    private string _accountsName = string.Empty;
    public string AccountsName { get => _accountsName; set => this.RaiseAndSetIfChanged(ref _accountsName, value); }

    private string _characterResource = string.Empty;
    public string CharacterResource { get => _characterResource; set => this.RaiseAndSetIfChanged(ref _characterResource, value); }

    private string _monsterResource = string.Empty;
    public string MonsterResource { get => _monsterResource; set => this.RaiseAndSetIfChanged(ref _monsterResource, value); }

    private string _stringResource = string.Empty;
    public string StringResource { get => _stringResource; set => this.RaiseAndSetIfChanged(ref _stringResource, value); }

    private string _itemResource = string.Empty;
    public string ItemResource { get => _itemResource; set => this.RaiseAndSetIfChanged(ref _itemResource, value); }

    private string _skillResource = string.Empty;
    public string SkillResource { get => _skillResource; set => this.RaiseAndSetIfChanged(ref _skillResource, value); }

    private string _stateResource = string.Empty;
    public string StateResource { get => _stateResource; set => this.RaiseAndSetIfChanged(ref _stateResource, value); }

    private string _npcResource = string.Empty;
    public string NpcResource { get => _npcResource; set => this.RaiseAndSetIfChanged(ref _npcResource, value); }

    private string _summonResource = string.Empty;
    public string SummonResource { get => _summonResource; set => this.RaiseAndSetIfChanged(ref _summonResource, value); }

    // --- General. ---

    private bool _limitSelectQueries = true;
    public bool LimitSelectQueries { get => _limitSelectQueries; set => this.RaiseAndSetIfChanged(ref _limitSelectQueries, value); }

    private bool _useLocalCache;
    public bool UseLocalCache { get => _useLocalCache; set => this.RaiseAndSetIfChanged(ref _useLocalCache, value); }

    private bool _enableEntityIcons;

    public bool EnableEntityIcons
    {
        get => _enableEntityIcons;
        set => this.RaiseAndSetIfChanged(ref _enableEntityIcons, value);
    }

    private string _entityIconsPath = string.Empty;
    public string EntityIconsPath { get => _entityIconsPath; set => this.RaiseAndSetIfChanged(ref _entityIconsPath, value); }

    private string _cacheStatus = string.Empty;
    public string CacheStatus { get => _cacheStatus; set => this.RaiseAndSetIfChanged(ref _cacheStatus, value); }

    private string _status = string.Empty;
    public string Status { get => _status; set => this.RaiseAndSetIfChanged(ref _status, value); }

    private void LoadFromWorking()
    {
        if (string.IsNullOrWhiteSpace(_working.Connection.Server)
            && !string.IsNullOrWhiteSpace(_working.ConnectionString)
            && _connectionStringBuilder.TryParse(_working.Provider, _working.ConnectionString, out var parsed))
        {
            _working.Connection = parsed;
        }

        _provider = _working.Provider;
        this.RaisePropertyChanged(nameof(Provider));
        this.RaisePropertyChanged(nameof(IsMsSql));

        Server = _working.Connection.Server;
        Port = _working.Connection.Port > 0
            ? _working.Connection.Port
            : DefaultPort(_working.Provider);
        Database = _working.Connection.Database;
        UserId = _working.Connection.UserId;
        Password = _working.Connection.Password;
        IntegratedSecurity = _working.Connection.IntegratedSecurity;
        Encrypt = _working.Connection.Encrypt;
        TrustServerCertificate = _working.Connection.TrustServerCertificate;

        ArcadiaName = _working.TableNames.ArcadiaName;
        TelecasterName = _working.TableNames.TelecasterName;
        AuthName = _working.TableNames.AuthName;
        AccountsName = _working.TableNames.AccountsName;
        CharacterResource = _working.TableNames.CharacterResource;
        MonsterResource = _working.TableNames.MonsterResource;
        StringResource = _working.TableNames.StringResource;
        ItemResource = _working.TableNames.ItemResource;
        SkillResource = _working.TableNames.SkillResource;
        StateResource = _working.TableNames.StateResource;
        NpcResource = _working.TableNames.NpcResource;
        SummonResource = _working.TableNames.SummonResource;

        LimitSelectQueries = _working.LimitSelectQueries;
        UseLocalCache = _working.UseLocalCache;
        EnableEntityIcons = _working.EnableEntityIcons;
        EntityIconsPath = _working.EntityIconsPath ?? string.Empty;
    }

    private void WriteToWorking()
    {
        _working.Provider = Provider;
        _working.Connection.Server = Server.Trim();
        _working.Connection.Port = Port;
        _working.Connection.Database = Database.Trim();
        _working.Connection.UserId = UserId.Trim();
        _working.Connection.Password = Password;
        _working.Connection.IntegratedSecurity = IntegratedSecurity;
        _working.Connection.Encrypt = Encrypt;
        _working.Connection.TrustServerCertificate = TrustServerCertificate;

        _working.TableNames.ArcadiaName = ArcadiaName.Trim();
        _working.TableNames.TelecasterName = TelecasterName.Trim();
        _working.TableNames.AuthName = AuthName.Trim();
        _working.TableNames.AccountsName = AccountsName.Trim();
        _working.TableNames.CharacterResource = CharacterResource.Trim();
        _working.TableNames.MonsterResource = MonsterResource.Trim();
        _working.TableNames.StringResource = StringResource.Trim();
        _working.TableNames.ItemResource = ItemResource.Trim();
        _working.TableNames.SkillResource = SkillResource.Trim();
        _working.TableNames.StateResource = StateResource.Trim();
        _working.TableNames.NpcResource = NpcResource.Trim();
        _working.TableNames.SummonResource = SummonResource.Trim();

        _working.LimitSelectQueries = LimitSelectQueries;
        _working.UseLocalCache = UseLocalCache;
        _working.EnableEntityIcons = EnableEntityIcons;
        _working.EntityIconsPath = EntityIconsPath.Trim();

        _working.ConnectionString = _connectionStringBuilder.Build(_working.Provider, _working.Connection);
    }

    private void OnProviderChanged()
    {
        this.RaisePropertyChanged(nameof(IsMsSql));
        this.RaisePropertyChanged(nameof(SqlAuthEnabled));

        if (Provider == DatabaseProvider.MSSQL && Port == 3306)
        {
            Port = 1433;
        }
        else if (Provider == DatabaseProvider.MySQL && Port == 1433)
        {
            Port = 3306;
        }
    }

    private static int DefaultPort(DatabaseProvider provider)
        => provider == DatabaseProvider.MSSQL ? 1433 : 3306;

    private bool TryBuildConnectionString(out string connectionString, out string validationMessage)
    {
        connectionString = string.Empty;
        validationMessage = string.Empty;

        WriteToWorking();

        if (string.IsNullOrWhiteSpace(_working.Connection.Server))
        {
            validationMessage = "Server is required.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(_working.Connection.Database))
        {
            validationMessage = "Database is required.";
            return false;
        }

        if (_working.Provider == DatabaseProvider.MSSQL
            && !_working.Connection.IntegratedSecurity
            && string.IsNullOrWhiteSpace(_working.Connection.UserId))
        {
            validationMessage = "User ID is required for SQL authentication.";
            return false;
        }

        if (_working.Provider == DatabaseProvider.MySQL && string.IsNullOrWhiteSpace(_working.Connection.UserId))
        {
            validationMessage = "User ID is required for MySQL.";
            return false;
        }

        if (_working.EnableEntityIcons)
        {
            if (string.IsNullOrWhiteSpace(_working.EntityIconsPath))
            {
                validationMessage = "Icons folder is required when entity icons are enabled.";
                return false;
            }

            if (!Directory.Exists(_working.EntityIconsPath))
            {
                validationMessage = "Selected icons folder does not exist.";
                return false;
            }
        }

        connectionString = _connectionStringBuilder.Build(_working.Provider, _working.Connection);
        return true;
    }

    private async Task TestConnectionAsync()
    {
        if (!TryBuildConnectionString(out var connectionString, out var validationMessage))
        {
            Status = validationMessage;
            await _dlg.ShowWarningAsync("Validation", validationMessage);
            return;
        }

        Status = "Testing connection...";
        try
        {
            await _repository.TestConnectionAsync(_working.Provider, connectionString, CancellationToken.None);
            Status = "Connection successful.";
            await _dlg.ShowInfoAsync("Database Test", "Connection successful.");
        }
        catch (Exception ex)
        {
            Status = "Connection test failed.";
            await _dlg.ShowErrorAsync("Database Test Failed", ex.Message);
        }
    }

    private async Task SaveAsync()
    {
        if (!TryBuildConnectionString(out var connectionString, out var validationMessage))
        {
            Status = validationMessage;
            await _dlg.ShowWarningAsync("Validation", validationMessage);
            return;
        }

        TrySaveToEnv(connectionString);

        var updated = _working.Clone();
        _holder.Set(updated);
        Log.Debug(
            "Settings.Save: re-applying IconCache (enabled={Enabled} path={Path}).",
            updated.EnableEntityIcons,
            updated.EntityIconsPath ?? "<null>");
        IconCache.Configure(updated.EnableEntityIcons, updated.EntityIconsPath);

        try
        {
            await _settingsService.SaveAsync(updated, CancellationToken.None);
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Failed to persist settings.");
        }

        CloseRequested?.Invoke(this, true);
    }

    private void TrySaveToEnv(string connectionString)
    {
        try
        {
            var envPath = Path.Combine(AppContext.BaseDirectory, ".env");
            File.WriteAllLines(
                envPath,
                [
                    $"{DotEnv.DbProviderEnvKey}={_working.Provider}",
                    $"{DotEnv.DbConnectionStringEnvKey}={connectionString}",
                ]);
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "Failed to write .env (best-effort).");
        }
    }

    private async Task ExportCacheAsync()
    {
        if (!TryBuildConnectionString(out var connectionString, out var validationMessage))
        {
            Status = validationMessage;
            await _dlg.ShowWarningAsync("Validation", validationMessage);
            return;
        }

        var tokens = _working.TableNames.ToTokenMap();
        var provider = _working.Provider;

        try
        {
            Status = "Exporting Monsters...";
            await _localCacheService.SaveAsync("monsters", await _repository.GetMonstersAsync(provider, connectionString, tokens));

            Status = "Exporting Items...";
            await _localCacheService.SaveAsync("items", await _repository.GetItemsAsync(provider, connectionString, tokens));

            Status = "Exporting Skills...";
            await _localCacheService.SaveAsync("skills", await _repository.GetSkillsAsync(provider, connectionString, tokens));

            Status = "Exporting Buffs/States...";
            await _localCacheService.SaveAsync("states", await _repository.GetStatesAsync(provider, connectionString, tokens));

            Status = "Exporting NPCs...";
            await _localCacheService.SaveAsync("npcs", await _repository.GetNpcsAsync(provider, connectionString, tokens));

            Status = "Exporting Summons...";
            await _localCacheService.SaveAsync("summons", await _repository.GetSummonsAsync(provider, connectionString, tokens));

            RefreshCacheStatus();
            Status = "Cache export complete.";
            await _dlg.ShowInfoAsync("Export Complete", "All tables exported to local cache successfully.");
        }
        catch (Exception ex)
        {
            Status = "Cache export failed.";
            await _dlg.ShowErrorAsync("Export Failed", ex.Message);
        }
    }

    private async Task BrowseAsync()
    {
        if (FolderPicker is null)
        {
            return;
        }

        var start = Directory.Exists(EntityIconsPath) ? EntityIconsPath : null;
        var selected = await FolderPicker(start);
        if (!string.IsNullOrWhiteSpace(selected))
        {
            EntityIconsPath = selected;
        }
    }

    private void RefreshCacheStatus()
    {
        var dates = CacheEntities
            .Select(e => _localCacheService.GetCacheDate(e.Key))
            .Where(d => d.HasValue)
            .Select(d => d!.Value)
            .ToList();

        CacheStatus = dates.Count == 0
            ? "No cache found."
            : $"Cache available — last export: {dates.Min():yyyy-MM-dd HH:mm} ({dates.Count}/{CacheEntities.Length} tables)";
    }
}
