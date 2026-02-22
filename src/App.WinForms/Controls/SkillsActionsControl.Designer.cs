namespace App.WinForms.Controls;

partial class SkillsActionsControl
{
    private System.ComponentModel.IContainer components = null;
    private GroupBox gbSkillsActions;
    private TableLayoutPanel tlpRoot;
    private GroupBox gbSelected;
    private TableLayoutPanel tlpSelected;
    private Label lblSkillId;
    private NumericUpDown nudSkillId;
    private Label lblSkillLevel;
    private NumericUpDown nudSkillLevel;
    private GroupBox gbPlayerSkills;
    private FlowLayoutPanel flpPlayerSkills;
    private Button btnLearnSkill;
    private Button btnSetSkill;
    private Button btnRemoveSkill;
    private Button btnLearnAllJobSkills;
    private GroupBox gbCreatureSkills;
    private TableLayoutPanel tlpCreature;
    private Label lblCreatureSlotIndex;
    private NumericUpDown nudCreatureSlotIndex;
    private Button btnLearnCreatureSkill;
    private Button btnLearnCreatureAllSkill;

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
        tlpRoot = new TableLayoutPanel();
        gbSelected = new GroupBox();
        tlpSelected = new TableLayoutPanel();
        lblSkillId = new Label();
        nudSkillId = new NumericUpDown();
        lblSkillLevel = new Label();
        nudSkillLevel = new NumericUpDown();
        gbPlayerSkills = new GroupBox();
        flpPlayerSkills = new FlowLayoutPanel();
        btnLearnSkill = new Button();
        btnSetSkill = new Button();
        btnRemoveSkill = new Button();
        btnLearnAllJobSkills = new Button();
        gbCreatureSkills = new GroupBox();
        tlpCreature = new TableLayoutPanel();
        lblCreatureSlotIndex = new Label();
        nudCreatureSlotIndex = new NumericUpDown();
        btnLearnCreatureSkill = new Button();
        btnLearnCreatureAllSkill = new Button();
        gbSkillsActions.SuspendLayout();
        tlpRoot.SuspendLayout();
        gbSelected.SuspendLayout();
        tlpSelected.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudSkillId).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudSkillLevel).BeginInit();
        gbPlayerSkills.SuspendLayout();
        flpPlayerSkills.SuspendLayout();
        gbCreatureSkills.SuspendLayout();
        tlpCreature.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudCreatureSlotIndex).BeginInit();
        SuspendLayout();
        // 
        // gbSkillsActions
        // 
        gbSkillsActions.Controls.Add(tlpRoot);
        gbSkillsActions.Dock = DockStyle.Fill;
        gbSkillsActions.Location = new Point(0, 0);
        gbSkillsActions.Name = "gbSkillsActions";
        gbSkillsActions.Size = new Size(430, 426);
        gbSkillsActions.TabIndex = 0;
        gbSkillsActions.TabStop = false;
        gbSkillsActions.Text = "Skills";
        // 
        // tlpRoot
        // 
        tlpRoot.ColumnCount = 1;
        tlpRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpRoot.Controls.Add(gbSelected, 0, 0);
        tlpRoot.Controls.Add(gbPlayerSkills, 0, 1);
        tlpRoot.Controls.Add(gbCreatureSkills, 0, 2);
        tlpRoot.Dock = DockStyle.Fill;
        tlpRoot.Location = new Point(3, 23);
        tlpRoot.Name = "tlpRoot";
        tlpRoot.RowCount = 4;
        tlpRoot.RowStyles.Add(new RowStyle());
        tlpRoot.RowStyles.Add(new RowStyle());
        tlpRoot.RowStyles.Add(new RowStyle());
        tlpRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpRoot.Size = new Size(424, 400);
        tlpRoot.TabIndex = 0;
        // 
        // gbSelected
        // 
        gbSelected.Controls.Add(tlpSelected);
        gbSelected.Dock = DockStyle.Top;
        gbSelected.Location = new Point(3, 3);
        gbSelected.Name = "gbSelected";
        gbSelected.Size = new Size(418, 83);
        gbSelected.TabIndex = 0;
        gbSelected.TabStop = false;
        gbSelected.Text = "Selected";
        // 
        // tlpSelected
        // 
        tlpSelected.ColumnCount = 4;
        tlpSelected.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        tlpSelected.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpSelected.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        tlpSelected.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpSelected.Controls.Add(lblSkillId, 0, 0);
        tlpSelected.Controls.Add(nudSkillId, 1, 0);
        tlpSelected.Controls.Add(lblSkillLevel, 2, 0);
        tlpSelected.Controls.Add(nudSkillLevel, 3, 0);
        tlpSelected.Dock = DockStyle.Fill;
        tlpSelected.Location = new Point(3, 23);
        tlpSelected.Name = "tlpSelected";
        tlpSelected.RowCount = 1;
        tlpSelected.RowStyles.Add(new RowStyle());
        tlpSelected.Size = new Size(412, 57);
        tlpSelected.TabIndex = 0;
        // 
        // lblSkillId
        // 
        lblSkillId.Anchor = AnchorStyles.Left;
        lblSkillId.AutoSize = true;
        lblSkillId.Location = new Point(3, 8);
        lblSkillId.Name = "lblSkillId";
        lblSkillId.Size = new Size(26, 20);
        lblSkillId.TabIndex = 0;
        lblSkillId.Text = "ID:";
        // 
        // nudSkillId
        // 
        nudSkillId.Dock = DockStyle.Fill;
        nudSkillId.Location = new Point(35, 3);
        nudSkillId.Maximum = new decimal(new int[] { 1000000000, 0, 0, 0 });
        nudSkillId.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        nudSkillId.Name = "nudSkillId";
        nudSkillId.Size = new Size(136, 27);
        nudSkillId.TabIndex = 1;
        nudSkillId.Value = new decimal(new int[] { 1, 0, 0, 0 });
        // 
        // lblSkillLevel
        // 
        lblSkillLevel.Anchor = AnchorStyles.Left;
        lblSkillLevel.AutoSize = true;
        lblSkillLevel.Location = new Point(177, 8);
        lblSkillLevel.Name = "lblSkillLevel";
        lblSkillLevel.Size = new Size(48, 20);
        lblSkillLevel.TabIndex = 2;
        lblSkillLevel.Text = "Level:";
        // 
        // nudSkillLevel
        // 
        nudSkillLevel.Dock = DockStyle.Fill;
        nudSkillLevel.Location = new Point(231, 3);
        nudSkillLevel.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
        nudSkillLevel.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        nudSkillLevel.Name = "nudSkillLevel";
        nudSkillLevel.Size = new Size(178, 27);
        nudSkillLevel.TabIndex = 3;
        nudSkillLevel.Value = new decimal(new int[] { 1, 0, 0, 0 });
        // 
        // gbPlayerSkills
        // 
        gbPlayerSkills.Controls.Add(flpPlayerSkills);
        gbPlayerSkills.Dock = DockStyle.Top;
        gbPlayerSkills.Location = new Point(3, 92);
        gbPlayerSkills.Name = "gbPlayerSkills";
        gbPlayerSkills.Size = new Size(418, 135);
        gbPlayerSkills.TabIndex = 1;
        gbPlayerSkills.TabStop = false;
        gbPlayerSkills.Text = "Player Skills";
        // 
        // flpPlayerSkills
        // 
        flpPlayerSkills.Controls.Add(btnLearnSkill);
        flpPlayerSkills.Controls.Add(btnSetSkill);
        flpPlayerSkills.Controls.Add(btnRemoveSkill);
        flpPlayerSkills.Controls.Add(btnLearnAllJobSkills);
        flpPlayerSkills.Dock = DockStyle.Fill;
        flpPlayerSkills.Location = new Point(3, 23);
        flpPlayerSkills.Name = "flpPlayerSkills";
        flpPlayerSkills.Size = new Size(412, 109);
        flpPlayerSkills.TabIndex = 0;
        // 
        // btnLearnSkill
        // 
        btnLearnSkill.Location = new Point(3, 3);
        btnLearnSkill.Name = "btnLearnSkill";
        btnLearnSkill.Size = new Size(196, 36);
        btnLearnSkill.TabIndex = 0;
        btnLearnSkill.Text = "Learn skill";
        btnLearnSkill.UseVisualStyleBackColor = true;
        btnLearnSkill.Click += btnLearnSkill_Click;
        // 
        // btnSetSkill
        // 
        btnSetSkill.Location = new Point(205, 3);
        btnSetSkill.Name = "btnSetSkill";
        btnSetSkill.Size = new Size(196, 36);
        btnSetSkill.TabIndex = 1;
        btnSetSkill.Text = "Set skill level";
        btnSetSkill.UseVisualStyleBackColor = true;
        btnSetSkill.Click += btnSetSkill_Click;
        // 
        // btnRemoveSkill
        // 
        btnRemoveSkill.Location = new Point(3, 43);
        btnRemoveSkill.Name = "btnRemoveSkill";
        btnRemoveSkill.Size = new Size(196, 36);
        btnRemoveSkill.TabIndex = 2;
        btnRemoveSkill.Text = "Remove skill";
        btnRemoveSkill.UseVisualStyleBackColor = true;
        btnRemoveSkill.Click += btnRemoveSkill_Click;
        // 
        // btnLearnAllJobSkills
        // 
        btnLearnAllJobSkills.Location = new Point(205, 43);
        btnLearnAllJobSkills.Name = "btnLearnAllJobSkills";
        btnLearnAllJobSkills.Size = new Size(196, 36);
        btnLearnAllJobSkills.TabIndex = 3;
        btnLearnAllJobSkills.Text = "Learn all skill";
        btnLearnAllJobSkills.UseVisualStyleBackColor = true;
        btnLearnAllJobSkills.Click += btnLearnAllJobSkills_Click;
        //
        // gbCreatureSkills
        //
        gbCreatureSkills.Controls.Add(tlpCreature);
        gbCreatureSkills.Dock = DockStyle.Top;
        gbCreatureSkills.Location = new Point(3, 233);
        gbCreatureSkills.Name = "gbCreatureSkills";
        gbCreatureSkills.Size = new Size(418, 145);
        gbCreatureSkills.TabIndex = 2;
        gbCreatureSkills.TabStop = false;
        gbCreatureSkills.Text = "Creature Skills";
        //
        // tlpCreature
        //
        tlpCreature.ColumnCount = 2;
        tlpCreature.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        tlpCreature.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpCreature.Controls.Add(lblCreatureSlotIndex, 0, 0);
        tlpCreature.Controls.Add(nudCreatureSlotIndex, 1, 0);
        tlpCreature.Controls.Add(btnLearnCreatureSkill, 0, 1);
        tlpCreature.Controls.Add(btnLearnCreatureAllSkill, 0, 2);
        tlpCreature.Dock = DockStyle.Fill;
        tlpCreature.Location = new Point(3, 23);
        tlpCreature.Name = "tlpCreature";
        tlpCreature.RowCount = 3;
        tlpCreature.RowStyles.Add(new RowStyle());
        tlpCreature.RowStyles.Add(new RowStyle());
        tlpCreature.RowStyles.Add(new RowStyle());
        tlpCreature.Size = new Size(412, 119);
        tlpCreature.TabIndex = 0;
        // 
        // lblCreatureSlotIndex
        // 
        lblCreatureSlotIndex.Anchor = AnchorStyles.Left;
        lblCreatureSlotIndex.AutoSize = true;
        lblCreatureSlotIndex.Location = new Point(3, 8);
        lblCreatureSlotIndex.Name = "lblCreatureSlotIndex";
        lblCreatureSlotIndex.Size = new Size(113, 20);
        lblCreatureSlotIndex.TabIndex = 0;
        lblCreatureSlotIndex.Text = "Creature slot (0=all):";
        // 
        // nudCreatureSlotIndex
        // 
        nudCreatureSlotIndex.Dock = DockStyle.Fill;
        nudCreatureSlotIndex.Location = new Point(122, 3);
        nudCreatureSlotIndex.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
        nudCreatureSlotIndex.Name = "nudCreatureSlotIndex";
        nudCreatureSlotIndex.Size = new Size(287, 27);
        nudCreatureSlotIndex.TabIndex = 1;
        // 
        // btnLearnCreatureSkill
        // 
        tlpCreature.SetColumnSpan(btnLearnCreatureSkill, 2);
        btnLearnCreatureSkill.Dock = DockStyle.Fill;
        btnLearnCreatureSkill.Location = new Point(3, 36);
        btnLearnCreatureSkill.Name = "btnLearnCreatureSkill";
        btnLearnCreatureSkill.Size = new Size(406, 36);
        btnLearnCreatureSkill.TabIndex = 2;
        btnLearnCreatureSkill.Text = "Learn creature skill";
        btnLearnCreatureSkill.UseVisualStyleBackColor = true;
        btnLearnCreatureSkill.Click += btnLearnCreatureSkill_Click;
        // 
        // btnLearnCreatureAllSkill
        // 
        tlpCreature.SetColumnSpan(btnLearnCreatureAllSkill, 2);
        btnLearnCreatureAllSkill.Dock = DockStyle.Fill;
        btnLearnCreatureAllSkill.Location = new Point(3, 76);
        btnLearnCreatureAllSkill.Name = "btnLearnCreatureAllSkill";
        btnLearnCreatureAllSkill.Size = new Size(406, 36);
        btnLearnCreatureAllSkill.TabIndex = 3;
        btnLearnCreatureAllSkill.Text = "Learn creature all skill";
        btnLearnCreatureAllSkill.UseVisualStyleBackColor = true;
        btnLearnCreatureAllSkill.Click += btnLearnCreatureAllSkill_Click;
        // 
        // SkillsActionsControl
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(gbSkillsActions);
        Name = "SkillsActionsControl";
        Size = new Size(430, 426);
        AutoSize = true;
        AutoSizeMode = AutoSizeMode.GrowAndShrink;
        gbSkillsActions.ResumeLayout(false);
        tlpRoot.ResumeLayout(false);
        gbSelected.ResumeLayout(false);
        tlpSelected.ResumeLayout(false);
        tlpSelected.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)nudSkillId).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudSkillLevel).EndInit();
        gbPlayerSkills.ResumeLayout(false);
        flpPlayerSkills.ResumeLayout(false);
        gbCreatureSkills.ResumeLayout(false);
        tlpCreature.ResumeLayout(false);
        tlpCreature.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)nudCreatureSlotIndex).EndInit();
        ResumeLayout(false);
    }
}
