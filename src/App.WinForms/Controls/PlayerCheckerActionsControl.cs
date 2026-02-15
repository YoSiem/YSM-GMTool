namespace App.WinForms.Controls;

public partial class PlayerCheckerActionsControl : UserControl
{
    public PlayerCheckerActionsControl()
    {
        InitializeComponent();
    }

    public event EventHandler? CreateCheckCommandRequested;

    public string PlayerName => txtPlayerName.Text.Trim();

    private void btnCreateCheckCommand_Click(object sender, EventArgs e)
    {
        CreateCheckCommandRequested?.Invoke(this, EventArgs.Empty);
    }
}
