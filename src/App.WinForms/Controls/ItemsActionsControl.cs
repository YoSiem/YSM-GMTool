namespace App.WinForms.Controls;

public partial class ItemsActionsControl : UserControl
{
    public ItemsActionsControl()
    {
        InitializeComponent();
        cmbTargetMode.SelectedIndex = 0;
        ToggleTargetInput();
    }

    public event EventHandler? CreateGiveCommandRequested;

    public bool GiveToOtherPlayer => (cmbTargetMode.SelectedItem as string)?.Equals("Other player", StringComparison.OrdinalIgnoreCase) == true;

    public string TargetPlayer => txtTargetPlayer.Text.Trim();

    public int Amount => (int)nudAmount.Value;

    public int Level => (int)nudLevel.Value;

    public int Enhance => (int)nudEnhance.Value;

    public int ItmCode => (int)nudItmCode.Value;

    private void btnCreateGiveCommand_Click(object sender, EventArgs e)
    {
        CreateGiveCommandRequested?.Invoke(this, EventArgs.Empty);
    }

    private void cmbTargetMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        ToggleTargetInput();
    }

    private void ToggleTargetInput()
    {
        txtTargetPlayer.Enabled = GiveToOtherPlayer;
    }
}
