namespace App.WinForms.Controls;

public partial class BuffsActionsControl : UserControl
{
    public BuffsActionsControl()
    {
        InitializeComponent();
    }

    public event EventHandler? AddBuffRequested;

    public event EventHandler? RemoveBuffRequested;

    public event EventHandler? SetTimedWorldStateRequested;

    public int DurationSeconds => (int)nudDurationSeconds.Value;

    public string WorldStateValue => txtWorldStateValue.Text.Trim();

    private void btnAddBuff_Click(object sender, EventArgs e)
    {
        AddBuffRequested?.Invoke(this, EventArgs.Empty);
    }

    private void btnRemoveBuff_Click(object sender, EventArgs e)
    {
        RemoveBuffRequested?.Invoke(this, EventArgs.Empty);
    }

    private void btnSetTimedWorldState_Click(object sender, EventArgs e)
    {
        SetTimedWorldStateRequested?.Invoke(this, EventArgs.Empty);
    }
}
