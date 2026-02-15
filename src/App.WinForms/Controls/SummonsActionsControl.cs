namespace App.WinForms.Controls;

public partial class SummonsActionsControl : UserControl
{
    public SummonsActionsControl()
    {
        InitializeComponent();
    }

    public event EventHandler? CreateSummonCommandRequested;

    public int Amount => (int)nudAmount.Value;

    private void btnCreateSummonCommand_Click(object sender, EventArgs e)
    {
        CreateSummonCommandRequested?.Invoke(this, EventArgs.Empty);
    }
}
