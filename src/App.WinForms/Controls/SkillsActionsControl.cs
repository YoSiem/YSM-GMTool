namespace App.WinForms.Controls;

public partial class SkillsActionsControl : UserControl
{
    public SkillsActionsControl()
    {
        InitializeComponent();
    }

    public event EventHandler? LearnSkillRequested;

    public event EventHandler? LearnAllJobSkillsRequested;

    public event EventHandler? GetBasicSkillLevelRequested;

    public string TargetPlayer => txtTargetPlayer.Text.Trim();

    private void btnLearnSkill_Click(object sender, EventArgs e)
    {
        LearnSkillRequested?.Invoke(this, EventArgs.Empty);
    }

    private void btnLearnAllJobSkills_Click(object sender, EventArgs e)
    {
        LearnAllJobSkillsRequested?.Invoke(this, EventArgs.Empty);
    }

    private void btnGetBasicSkillLevel_Click(object sender, EventArgs e)
    {
        GetBasicSkillLevelRequested?.Invoke(this, EventArgs.Empty);
    }
}
