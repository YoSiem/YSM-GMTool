namespace App.WinForms.Controls;

public partial class NpcsActionsControl : UserControl
{
    public NpcsActionsControl()
    {
        InitializeComponent();
    }

    public event EventHandler? CreateNpcMoveCommandRequested;

    public int Layer => (int)nudLayer.Value;

    private void btnCreateMoveCommand_Click(object sender, EventArgs e)
    {
        CreateNpcMoveCommandRequested?.Invoke(this, EventArgs.Empty);
    }
}
