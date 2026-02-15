namespace App.WinForms.Controls;

partial class SkillsActionsControl
{
    private System.ComponentModel.IContainer components = null;
    private GroupBox gbSkillsActions;
    private TableLayoutPanel tlp;
    private Label lblTargetPlayer;
    private TextBox txtTargetPlayer;
    private Button btnLearnSkill;
    private Button btnLearnAllJobSkills;
    private Button btnGetBasicSkillLevel;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        gbSkillsActions = new GroupBox();
        tlp = new TableLayoutPanel();
        lblTargetPlayer = new Label();
        txtTargetPlayer = new TextBox();
        btnLearnSkill = new Button();
        btnLearnAllJobSkills = new Button();
        btnGetBasicSkillLevel = new Button();
        gbSkillsActions.SuspendLayout();
        tlp.SuspendLayout();
        SuspendLayout();
        // 
        // gbSkillsActions
        // 
        gbSkillsActions.Controls.Add(tlp);
        gbSkillsActions.Dock = DockStyle.Fill;
        gbSkillsActions.Location = new Point(0, 0);
        gbSkillsActions.Name = "gbSkillsActions";
        gbSkillsActions.Size = new Size(430, 210);
        gbSkillsActions.TabIndex = 0;
        gbSkillsActions.TabStop = false;
        gbSkillsActions.Text = "Skill Commands";
        // 
        // tlp
        // 
        tlp.ColumnCount = 2;
        tlp.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlp.Controls.Add(lblTargetPlayer, 0, 0);
        tlp.Controls.Add(txtTargetPlayer, 1, 0);
        tlp.Controls.Add(btnLearnSkill, 0, 1);
        tlp.Controls.Add(btnLearnAllJobSkills, 0, 2);
        tlp.Controls.Add(btnGetBasicSkillLevel, 0, 3);
        tlp.Dock = DockStyle.Fill;
        tlp.Location = new Point(3, 23);
        tlp.Name = "tlp";
        tlp.RowCount = 4;
        tlp.RowStyles.Add(new RowStyle());
        tlp.RowStyles.Add(new RowStyle());
        tlp.RowStyles.Add(new RowStyle());
        tlp.RowStyles.Add(new RowStyle());
        tlp.Size = new Size(424, 184);
        tlp.TabIndex = 0;
        // 
        // lblTargetPlayer
        // 
        lblTargetPlayer.Anchor = AnchorStyles.Left;
        lblTargetPlayer.AutoSize = true;
        lblTargetPlayer.Location = new Point(3, 9);
        lblTargetPlayer.Name = "lblTargetPlayer";
        lblTargetPlayer.Size = new Size(87, 20);
        lblTargetPlayer.TabIndex = 0;
        lblTargetPlayer.Text = "Target player";
        // 
        // txtTargetPlayer
        // 
        txtTargetPlayer.Dock = DockStyle.Fill;
        txtTargetPlayer.Location = new Point(96, 3);
        txtTargetPlayer.Name = "txtTargetPlayer";
        txtTargetPlayer.PlaceholderText = "optional";
        txtTargetPlayer.Size = new Size(325, 27);
        txtTargetPlayer.TabIndex = 1;
        // 
        // btnLearnSkill
        // 
        tlp.SetColumnSpan(btnLearnSkill, 2);
        btnLearnSkill.Dock = DockStyle.Fill;
        btnLearnSkill.Location = new Point(3, 36);
        btnLearnSkill.Name = "btnLearnSkill";
        btnLearnSkill.Size = new Size(418, 29);
        btnLearnSkill.TabIndex = 2;
        btnLearnSkill.Text = "Learn selected skill";
        btnLearnSkill.UseVisualStyleBackColor = true;
        btnLearnSkill.Click += btnLearnSkill_Click;
        // 
        // btnLearnAllJobSkills
        // 
        tlp.SetColumnSpan(btnLearnAllJobSkills, 2);
        btnLearnAllJobSkills.Dock = DockStyle.Fill;
        btnLearnAllJobSkills.Location = new Point(3, 71);
        btnLearnAllJobSkills.Name = "btnLearnAllJobSkills";
        btnLearnAllJobSkills.Size = new Size(418, 29);
        btnLearnAllJobSkills.TabIndex = 3;
        btnLearnAllJobSkills.Text = "Learn all job skills";
        btnLearnAllJobSkills.UseVisualStyleBackColor = true;
        btnLearnAllJobSkills.Click += btnLearnAllJobSkills_Click;
        // 
        // btnGetBasicSkillLevel
        // 
        tlp.SetColumnSpan(btnGetBasicSkillLevel, 2);
        btnGetBasicSkillLevel.Dock = DockStyle.Fill;
        btnGetBasicSkillLevel.Location = new Point(3, 106);
        btnGetBasicSkillLevel.Name = "btnGetBasicSkillLevel";
        btnGetBasicSkillLevel.Size = new Size(418, 29);
        btnGetBasicSkillLevel.TabIndex = 4;
        btnGetBasicSkillLevel.Text = "Get basic skill level";
        btnGetBasicSkillLevel.UseVisualStyleBackColor = true;
        btnGetBasicSkillLevel.Click += btnGetBasicSkillLevel_Click;
        // 
        // SkillsActionsControl
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(gbSkillsActions);
        Name = "SkillsActionsControl";
        Size = new Size(430, 210);
        gbSkillsActions.ResumeLayout(false);
        tlp.ResumeLayout(false);
        tlp.PerformLayout();
        ResumeLayout(false);
    }
}
