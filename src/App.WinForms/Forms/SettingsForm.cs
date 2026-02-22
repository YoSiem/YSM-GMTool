using App.Core.Enums;
using App.Core.Interfaces;
using App.Core.Models;

namespace App.WinForms.Forms;

public partial class SettingsForm : Form
{
    private static readonly (string Key, string Label)[] CacheEntities =
    [
        ("monsters",  "Monsters"),
        ("items",     "Items"),
        ("skills",    "Skills"),
        ("states",    "Buffs/States"),
        ("npcs",      "NPCs"),
        ("summons",   "Summons"),
    ];

    private readonly IGameDataRepository _repository;
    private readonly IConnectionStringBuilderService _connectionStringBuilder;
    private readonly ILocalCacheService _localCacheService;
    private readonly AppSettings _workingSettings;
    private bool _isLoading;

    public SettingsForm(
        IGameDataRepository repository,
        IConnectionStringBuilderService connectionStringBuilder,
        ILocalCacheService localCacheService,
        AppSettings currentSettings)
    {
        _repository = repository;
        _connectionStringBuilder = connectionStringBuilder;
        _localCacheService = localCacheService;
        _workingSettings = currentSettings.Clone();

        InitializeComponent();
        ApplyDialogIcon();
        LoadSettingsIntoControls();
        RefreshCacheStatus();
    }

    public AppSettings UpdatedSettings => _workingSettings.Clone();

    private void LoadSettingsIntoControls()
    {
        _isLoading = true;

        cmbProvider.DataSource = Enum.GetValues<DatabaseProvider>();
        cmbProvider.SelectedItem = _workingSettings.Provider;

        if (string.IsNullOrWhiteSpace(_workingSettings.Connection.Server)
            && !string.IsNullOrWhiteSpace(_workingSettings.ConnectionString)
            && _connectionStringBuilder.TryParse(_workingSettings.Provider, _workingSettings.ConnectionString, out var parsed))
        {
            _workingSettings.Connection = parsed;
        }

        txtServer.Text = _workingSettings.Connection.Server;
        var port = _workingSettings.Connection.Port > 0
            ? _workingSettings.Connection.Port
            : (_workingSettings.Provider == DatabaseProvider.MSSQL ? 1433 : 3306);
        nudPort.Value = Math.Clamp(port, (int)nudPort.Minimum, (int)nudPort.Maximum);
        txtDatabase.Text = _workingSettings.Connection.Database;
        txtUserId.Text = _workingSettings.Connection.UserId;
        txtPassword.Text = _workingSettings.Connection.Password;
        chkIntegratedSecurity.Checked = _workingSettings.Connection.IntegratedSecurity;
        chkEncrypt.Checked = _workingSettings.Connection.Encrypt;
        chkTrustServerCertificate.Checked = _workingSettings.Connection.TrustServerCertificate;

        txtArcadiaName.Text = _workingSettings.TableNames.ArcadiaName;
        txtTelecasterName.Text = _workingSettings.TableNames.TelecasterName;
        txtAuthName.Text = _workingSettings.TableNames.AuthName;
        txtAccountsName.Text = _workingSettings.TableNames.AccountsName;
        txtCharacterResource.Text = _workingSettings.TableNames.CharacterResource;
        txtMonsterResource.Text = _workingSettings.TableNames.MonsterResource;
        txtStringResource.Text = _workingSettings.TableNames.StringResource;
        txtItemResource.Text = _workingSettings.TableNames.ItemResource;
        txtSkillResource.Text = _workingSettings.TableNames.SkillResource;
        txtStateResource.Text = _workingSettings.TableNames.StateResource;
        txtNpcResource.Text = _workingSettings.TableNames.NpcResource;
        txtSummonResource.Text = _workingSettings.TableNames.SummonResource;

        _isLoading = false;

        UpdateAuthUi();
        chkLimitSelectQueries.Checked = _workingSettings.LimitSelectQueries;
        chkUseLocalCache.Checked = _workingSettings.UseLocalCache;
    }

    private void RefreshCacheStatus()
    {
        var dates = CacheEntities
            .Select(e => _localCacheService.GetCacheDate(e.Key))
            .Where(d => d.HasValue)
            .Select(d => d!.Value)
            .ToList();

        if (dates.Count == 0)
        {
            lblCacheStatus.Text = "No cache found.";
        }
        else
        {
            var oldest = dates.Min();
            lblCacheStatus.Text = $"Cache available — last export: {oldest:yyyy-MM-dd HH:mm} ({dates.Count}/{CacheEntities.Length} tables)";
        }
    }

    private void ReadControlsIntoWorkingSettings()
    {
        _workingSettings.Provider = cmbProvider.SelectedItem is DatabaseProvider provider
            ? provider
            : DatabaseProvider.MSSQL;

        _workingSettings.Connection.Server = txtServer.Text.Trim();
        _workingSettings.Connection.Port = (int)nudPort.Value;
        _workingSettings.Connection.Database = txtDatabase.Text.Trim();
        _workingSettings.Connection.UserId = txtUserId.Text.Trim();
        _workingSettings.Connection.Password = txtPassword.Text;
        _workingSettings.Connection.IntegratedSecurity = chkIntegratedSecurity.Checked;
        _workingSettings.Connection.Encrypt = chkEncrypt.Checked;
        _workingSettings.Connection.TrustServerCertificate = chkTrustServerCertificate.Checked;

        _workingSettings.TableNames.ArcadiaName = txtArcadiaName.Text.Trim();
        _workingSettings.TableNames.TelecasterName = txtTelecasterName.Text.Trim();
        _workingSettings.TableNames.AuthName = txtAuthName.Text.Trim();
        _workingSettings.TableNames.AccountsName = txtAccountsName.Text.Trim();
        _workingSettings.TableNames.CharacterResource = txtCharacterResource.Text.Trim();
        _workingSettings.TableNames.MonsterResource = txtMonsterResource.Text.Trim();
        _workingSettings.TableNames.StringResource = txtStringResource.Text.Trim();
        _workingSettings.TableNames.ItemResource = txtItemResource.Text.Trim();
        _workingSettings.TableNames.SkillResource = txtSkillResource.Text.Trim();
        _workingSettings.TableNames.StateResource = txtStateResource.Text.Trim();
        _workingSettings.TableNames.NpcResource = txtNpcResource.Text.Trim();
        _workingSettings.TableNames.SummonResource = txtSummonResource.Text.Trim();

        _workingSettings.LimitSelectQueries = chkLimitSelectQueries.Checked;
        _workingSettings.UseLocalCache = chkUseLocalCache.Checked;

        _workingSettings.ConnectionString = _connectionStringBuilder.Build(_workingSettings.Provider, _workingSettings.Connection);
    }

    private void ApplyDialogIcon()
    {
        try
        {
            using var exeIcon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            if (exeIcon is not null)
            {
                Icon = (Icon)exeIcon.Clone();
                ShowIcon = true;
            }
        }
        catch
        {
            // Keep default if extraction fails.
        }
    }

    private bool TryBuildConnectionString(out string connectionString, out string validationMessage)
    {
        connectionString = string.Empty;
        validationMessage = string.Empty;

        ReadControlsIntoWorkingSettings();

        if (string.IsNullOrWhiteSpace(_workingSettings.Connection.Server))
        {
            validationMessage = "Server is required.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(_workingSettings.Connection.Database))
        {
            validationMessage = "Database is required.";
            return false;
        }

        if (_workingSettings.Provider == DatabaseProvider.MSSQL && !_workingSettings.Connection.IntegratedSecurity)
        {
            if (string.IsNullOrWhiteSpace(_workingSettings.Connection.UserId))
            {
                validationMessage = "User ID is required for SQL authentication.";
                return false;
            }
        }

        if (_workingSettings.Provider == DatabaseProvider.MySQL && string.IsNullOrWhiteSpace(_workingSettings.Connection.UserId))
        {
            validationMessage = "User ID is required for MySQL.";
            return false;
        }

        connectionString = _connectionStringBuilder.Build(_workingSettings.Provider, _workingSettings.Connection);
        return true;
    }

    private async void btnTestConnection_Click(object sender, EventArgs e)
    {
        if (!TryBuildConnectionString(out var connectionString, out var validationMessage))
        {
            lblStatus.Text = validationMessage;
            MessageBox.Show(this, validationMessage, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        btnTestConnection.Enabled = false;
        lblStatus.Text = "Testing connection...";

        try
        {
            await _repository.TestConnectionAsync(_workingSettings.Provider, connectionString);
            lblStatus.Text = "Connection successful.";
            MessageBox.Show(this, "Connection successful.", "Database Test", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            lblStatus.Text = "Connection test failed.";
            MessageBox.Show(this, ex.Message, "Database Test Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            btnTestConnection.Enabled = true;
        }
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
        if (!TryBuildConnectionString(out var connectionString, out var validationMessage))
        {
            lblStatus.Text = validationMessage;
            MessageBox.Show(this, validationMessage, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        TrySaveToEnv(connectionString);

        DialogResult = DialogResult.OK;
        Close();
    }

    private void TrySaveToEnv(string connectionString)
    {
        try
        {
            var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var envPath = Path.Combine(appDirectory, ".env");

            File.WriteAllLines(envPath, new[]
            {
                $"YSM_DB_PROVIDER={_workingSettings.Provider}",
                $"YSM_DB_CONNECTION_STRING={connectionString}"
            });
        }
        catch
        {
            // .env save is best-effort; don't block saving settings.
        }
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private async void btnExportCache_Click(object sender, EventArgs e)
    {
        if (!TryBuildConnectionString(out var connectionString, out var validationMessage))
        {
            lblStatus.Text = validationMessage;
            MessageBox.Show(this, validationMessage, "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        btnExportCache.Enabled = false;
        var queryTokens = _workingSettings.TableNames.ToTokenMap();

        try
        {
            lblStatus.Text = "Exporting Monsters...";
            var monsters = await _repository.GetMonstersAsync(_workingSettings.Provider, connectionString, queryTokens);
            await _localCacheService.SaveAsync("monsters", monsters);

            lblStatus.Text = "Exporting Items...";
            var items = await _repository.GetItemsAsync(_workingSettings.Provider, connectionString, queryTokens);
            await _localCacheService.SaveAsync("items", items);

            lblStatus.Text = "Exporting Skills...";
            var skills = await _repository.GetSkillsAsync(_workingSettings.Provider, connectionString, queryTokens);
            await _localCacheService.SaveAsync("skills", skills);

            lblStatus.Text = "Exporting Buffs/States...";
            var states = await _repository.GetStatesAsync(_workingSettings.Provider, connectionString, queryTokens);
            await _localCacheService.SaveAsync("states", states);

            lblStatus.Text = "Exporting NPCs...";
            var npcs = await _repository.GetNpcsAsync(_workingSettings.Provider, connectionString, queryTokens);
            await _localCacheService.SaveAsync("npcs", npcs);

            lblStatus.Text = "Exporting Summons...";
            var summons = await _repository.GetSummonsAsync(_workingSettings.Provider, connectionString, queryTokens);
            await _localCacheService.SaveAsync("summons", summons);

            RefreshCacheStatus();
            lblStatus.Text = "Cache export complete.";
            MessageBox.Show(this, "All tables exported to local cache successfully.", "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            lblStatus.Text = "Cache export failed.";
            MessageBox.Show(this, ex.Message, "Export Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            btnExportCache.Enabled = true;
        }
    }

    private void cmbProvider_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (_isLoading)
        {
            return;
        }

        if (cmbProvider.SelectedItem is not DatabaseProvider provider)
        {
            return;
        }

        if (provider == DatabaseProvider.MSSQL && nudPort.Value == 3306)
        {
            nudPort.Value = 1433;
        }
        else if (provider == DatabaseProvider.MySQL && nudPort.Value == 1433)
        {
            nudPort.Value = 3306;
        }

        UpdateAuthUi();
    }

    private void ConnectionField_Changed(object? sender, EventArgs e)
    {
        if (_isLoading)
        {
            return;
        }

        UpdateAuthUi();
    }

    private void UpdateAuthUi()
    {
        var isMsSql = cmbProvider.SelectedItem is DatabaseProvider.MSSQL;
        chkIntegratedSecurity.Enabled = isMsSql;
        chkEncrypt.Enabled = isMsSql;
        chkTrustServerCertificate.Enabled = isMsSql;

        var useSqlAuth = !isMsSql || !chkIntegratedSecurity.Checked;
        txtUserId.Enabled = useSqlAuth;
        txtPassword.Enabled = useSqlAuth;
    }
}
