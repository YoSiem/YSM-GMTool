namespace App.WinForms.Controls;

public partial class MonsterActionsControl : UserControl
{
    public MonsterActionsControl()
    {
        InitializeComponent();
        cmbSpawnMode.SelectedIndex = 0;
        ToggleCoordinateInputs();
    }

    public event EventHandler? CreateCommandRequested;

    public int Amount => (int)nudAmount.Value;

    public string SpawnMode => (cmbSpawnMode.SelectedItem as string) ?? "At your place";

    public int X => (int)nudX.Value;

    public int Y => (int)nudY.Value;

    public int Layer => (int)nudLayer.Value;

    private void btnCreateSpawnCommand_Click(object sender, EventArgs e)
    {
        CreateCommandRequested?.Invoke(this, EventArgs.Empty);
    }

    private void cmbSpawnMode_SelectedIndexChanged(object sender, EventArgs e)
    {
        ToggleCoordinateInputs();
    }

    private void ToggleCoordinateInputs()
    {
        var coordinatesMode = SpawnMode.Equals("Coordinates", StringComparison.OrdinalIgnoreCase);
        nudX.Enabled = coordinatesMode;
        nudY.Enabled = coordinatesMode;
        nudLayer.Enabled = coordinatesMode;
    }
}
