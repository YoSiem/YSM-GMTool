using App.Core.Enums;
using App.Core.Interfaces;

namespace App.WinForms.Forms;

public partial class SettingsForm : Form
{
    private readonly IGameDataRepository _repository;

    public SettingsForm(IGameDataRepository repository, DatabaseProvider provider, string connectionString)
    {
        _repository = repository;
        InitializeComponent();

        cmbProvider.DataSource = Enum.GetValues<DatabaseProvider>();
        cmbProvider.SelectedItem = provider;
        txtConnectionString.Text = connectionString;
    }

    public DatabaseProvider SelectedProvider => cmbProvider.SelectedItem is DatabaseProvider provider
        ? provider
        : DatabaseProvider.MSSQL;

    public string ConnectionString => txtConnectionString.Text.Trim();

    private async void btnTestConnection_Click(object sender, EventArgs e)
    {
        btnTestConnection.Enabled = false;
        lblStatus.Text = "Testing connection...";

        try
        {
            await _repository.TestConnectionAsync(SelectedProvider, ConnectionString);
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
        if (string.IsNullOrWhiteSpace(ConnectionString))
        {
            MessageBox.Show(this, "Connection string is required.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        DialogResult = DialogResult.OK;
        Close();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
