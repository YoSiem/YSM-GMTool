namespace App.WinForms.Controls;

partial class SummonsActionsControl
{
    private System.ComponentModel.IContainer components = null;
    private GroupBox gbSummonActions;
    private TableLayoutPanel tlpRoot;
    private GroupBox gbAddSummon;
    private GroupBox gbStageSummon;
    private TableLayoutPanel tlpAdd;
    private TableLayoutPanel tlpStage;
    private Label lblSummonId;
    private NumericUpDown nudSummonId;
    private CheckBox chkUseAddStage;
    private Label lblAddStage;
    private NumericUpDown nudAddStage;
    private Button btnAddSummon;
    private Label lblSlot;
    private ComboBox cmbSlot;
    private Label lblStage;
    private NumericUpDown nudStage;
    private Button btnStageSummon;

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
        gbSummonActions = new GroupBox();
        tlpRoot = new TableLayoutPanel();
        gbAddSummon = new GroupBox();
        tlpAdd = new TableLayoutPanel();
        lblSummonId = new Label();
        nudSummonId = new NumericUpDown();
        chkUseAddStage = new CheckBox();
        lblAddStage = new Label();
        nudAddStage = new NumericUpDown();
        btnAddSummon = new Button();
        gbStageSummon = new GroupBox();
        tlpStage = new TableLayoutPanel();
        lblSlot = new Label();
        cmbSlot = new ComboBox();
        lblStage = new Label();
        nudStage = new NumericUpDown();
        btnStageSummon = new Button();
        gbSummonActions.SuspendLayout();
        tlpRoot.SuspendLayout();
        gbAddSummon.SuspendLayout();
        tlpAdd.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudSummonId).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudAddStage).BeginInit();
        gbStageSummon.SuspendLayout();
        tlpStage.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudStage).BeginInit();
        SuspendLayout();
        // 
        // gbSummonActions
        // 
        gbSummonActions.Controls.Add(tlpRoot);
        gbSummonActions.Dock = DockStyle.Fill;
        gbSummonActions.Location = new Point(0, 0);
        gbSummonActions.Name = "gbSummonActions";
        gbSummonActions.Size = new Size(430, 320);
        gbSummonActions.TabIndex = 0;
        gbSummonActions.TabStop = false;
        gbSummonActions.Text = "Summon Commands";
        // 
        // tlpRoot
        // 
        tlpRoot.ColumnCount = 1;
        tlpRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpRoot.Controls.Add(gbAddSummon, 0, 0);
        tlpRoot.Controls.Add(gbStageSummon, 0, 1);
        tlpRoot.Dock = DockStyle.Fill;
        tlpRoot.Location = new Point(3, 23);
        tlpRoot.Name = "tlpRoot";
        tlpRoot.RowCount = 2;
        tlpRoot.RowStyles.Add(new RowStyle());
        tlpRoot.RowStyles.Add(new RowStyle());
        tlpRoot.Size = new Size(424, 294);
        tlpRoot.TabIndex = 0;
        // 
        // gbAddSummon
        // 
        gbAddSummon.Controls.Add(tlpAdd);
        gbAddSummon.Dock = DockStyle.Fill;
        gbAddSummon.Location = new Point(3, 3);
        gbAddSummon.Name = "gbAddSummon";
        gbAddSummon.Size = new Size(418, 114);
        gbAddSummon.TabIndex = 0;
        gbAddSummon.TabStop = false;
        gbAddSummon.Text = "Add Summon";
        // 
        // tlpAdd
        // 
        tlpAdd.ColumnCount = 4;
        tlpAdd.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
        tlpAdd.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));
        tlpAdd.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
        tlpAdd.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55F));
        tlpAdd.Controls.Add(lblSummonId, 0, 0);
        tlpAdd.Controls.Add(nudSummonId, 1, 0);
        tlpAdd.Controls.Add(chkUseAddStage, 2, 0);
        tlpAdd.Controls.Add(lblAddStage, 0, 1);
        tlpAdd.Controls.Add(nudAddStage, 1, 1);
        tlpAdd.Controls.Add(btnAddSummon, 2, 1);
        tlpAdd.Dock = DockStyle.Fill;
        tlpAdd.Location = new Point(3, 23);
        tlpAdd.Name = "tlpAdd";
        tlpAdd.RowCount = 2;
        tlpAdd.RowStyles.Add(new RowStyle());
        tlpAdd.RowStyles.Add(new RowStyle());
        tlpAdd.Size = new Size(412, 88);
        tlpAdd.TabIndex = 0;
        // 
        // lblSummonId
        // 
        lblSummonId.Anchor = AnchorStyles.Left;
        lblSummonId.AutoSize = true;
        lblSummonId.Location = new Point(3, 6);
        lblSummonId.Name = "lblSummonId";
        lblSummonId.Size = new Size(75, 20);
        lblSummonId.TabIndex = 0;
        lblSummonId.Text = "SummonID";
        // 
        // nudSummonId
        // 
        nudSummonId.Dock = DockStyle.Fill;
        nudSummonId.Location = new Point(83, 3);
        nudSummonId.Maximum = new decimal(new int[] { 2000000000, 0, 0, 0 });
        nudSummonId.Name = "nudSummonId";
        nudSummonId.Size = new Size(93, 27);
        nudSummonId.TabIndex = 1;
        // 
        // chkUseAddStage
        // 
        chkUseAddStage.Anchor = AnchorStyles.Left;
        tlpAdd.SetColumnSpan(chkUseAddStage, 2);
        chkUseAddStage.AutoSize = true;
        chkUseAddStage.Location = new Point(182, 4);
        chkUseAddStage.Name = "chkUseAddStage";
        chkUseAddStage.Size = new Size(99, 24);
        chkUseAddStage.TabIndex = 2;
        chkUseAddStage.Text = "Use stage";
        chkUseAddStage.UseVisualStyleBackColor = true;
        chkUseAddStage.CheckedChanged += chkUseAddStage_CheckedChanged;
        // 
        // lblAddStage
        // 
        lblAddStage.Anchor = AnchorStyles.Left;
        lblAddStage.AutoSize = true;
        lblAddStage.Location = new Point(3, 40);
        lblAddStage.Name = "lblAddStage";
        lblAddStage.Size = new Size(44, 20);
        lblAddStage.TabIndex = 3;
        lblAddStage.Text = "Stage";
        // 
        // nudAddStage
        // 
        nudAddStage.Dock = DockStyle.Fill;
        nudAddStage.Location = new Point(83, 37);
        nudAddStage.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
        nudAddStage.Name = "nudAddStage";
        nudAddStage.Size = new Size(93, 27);
        nudAddStage.TabIndex = 4;
        // 
        // btnAddSummon
        // 
        tlpAdd.SetColumnSpan(btnAddSummon, 2);
        btnAddSummon.Dock = DockStyle.Fill;
        btnAddSummon.Location = new Point(182, 37);
        btnAddSummon.Name = "btnAddSummon";
        btnAddSummon.Size = new Size(227, 36);
        btnAddSummon.TabIndex = 5;
        btnAddSummon.Text = "Add Summon";
        btnAddSummon.UseVisualStyleBackColor = true;
        btnAddSummon.Click += btnAddSummon_Click;
        // 
        // gbStageSummon
        // 
        gbStageSummon.Controls.Add(tlpStage);
        gbStageSummon.Dock = DockStyle.Fill;
        gbStageSummon.Location = new Point(3, 123);
        gbStageSummon.Name = "gbStageSummon";
        gbStageSummon.Size = new Size(418, 168);
        gbStageSummon.TabIndex = 1;
        gbStageSummon.TabStop = false;
        gbStageSummon.Text = "Stage Summon";
        // 
        // tlpStage
        // 
        tlpStage.ColumnCount = 2;
        tlpStage.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 90F));
        tlpStage.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpStage.Controls.Add(lblSlot, 0, 0);
        tlpStage.Controls.Add(cmbSlot, 1, 0);
        tlpStage.Controls.Add(lblStage, 0, 1);
        tlpStage.Controls.Add(nudStage, 1, 1);
        tlpStage.Controls.Add(btnStageSummon, 0, 2);
        tlpStage.Dock = DockStyle.Fill;
        tlpStage.Location = new Point(3, 23);
        tlpStage.Name = "tlpStage";
        tlpStage.RowCount = 3;
        tlpStage.RowStyles.Add(new RowStyle());
        tlpStage.RowStyles.Add(new RowStyle());
        tlpStage.RowStyles.Add(new RowStyle());
        tlpStage.Size = new Size(412, 142);
        tlpStage.TabIndex = 0;
        // 
        // lblSlot
        // 
        lblSlot.Anchor = AnchorStyles.Left;
        lblSlot.AutoSize = true;
        lblSlot.Location = new Point(3, 7);
        lblSlot.Name = "lblSlot";
        lblSlot.Size = new Size(34, 20);
        lblSlot.TabIndex = 0;
        lblSlot.Text = "Slot";
        // 
        // cmbSlot
        // 
        cmbSlot.Dock = DockStyle.Fill;
        cmbSlot.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbSlot.FormattingEnabled = true;
        cmbSlot.Items.AddRange(new object[] { 0, 1, 2, 3, 4, 5 });
        cmbSlot.Location = new Point(93, 3);
        cmbSlot.Name = "cmbSlot";
        cmbSlot.Size = new Size(316, 28);
        cmbSlot.TabIndex = 1;
        // 
        // lblStage
        // 
        lblStage.Anchor = AnchorStyles.Left;
        lblStage.AutoSize = true;
        lblStage.Location = new Point(3, 40);
        lblStage.Name = "lblStage";
        lblStage.Size = new Size(44, 20);
        lblStage.TabIndex = 2;
        lblStage.Text = "Stage";
        // 
        // nudStage
        // 
        nudStage.Dock = DockStyle.Fill;
        nudStage.Location = new Point(93, 37);
        nudStage.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
        nudStage.Name = "nudStage";
        nudStage.Size = new Size(316, 27);
        nudStage.TabIndex = 3;
        // 
        // btnStageSummon
        // 
        tlpStage.SetColumnSpan(btnStageSummon, 2);
        btnStageSummon.Dock = DockStyle.Fill;
        btnStageSummon.Location = new Point(3, 70);
        btnStageSummon.Name = "btnStageSummon";
        btnStageSummon.Size = new Size(406, 36);
        btnStageSummon.TabIndex = 4;
        btnStageSummon.Text = "Stage Summon";
        btnStageSummon.UseVisualStyleBackColor = true;
        btnStageSummon.Click += btnStageSummon_Click;
        // 
        // SummonsActionsControl
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(gbSummonActions);
        MinimumSize = new Size(430, 0);
        Name = "SummonsActionsControl";
        Size = new Size(430, 320);
        AutoSize = true;
        AutoSizeMode = AutoSizeMode.GrowAndShrink;
        gbSummonActions.ResumeLayout(false);
        tlpRoot.ResumeLayout(false);
        gbAddSummon.ResumeLayout(false);
        tlpAdd.ResumeLayout(false);
        tlpAdd.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)nudSummonId).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudAddStage).EndInit();
        gbStageSummon.ResumeLayout(false);
        tlpStage.ResumeLayout(false);
        tlpStage.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)nudStage).EndInit();
        ResumeLayout(false);
    }
}
