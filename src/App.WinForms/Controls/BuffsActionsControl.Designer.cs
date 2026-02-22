namespace App.WinForms.Controls;

partial class BuffsActionsControl
{
    private System.ComponentModel.IContainer components = null;
    private GroupBox gbBuffActions;
    private TableLayoutPanel tlpRoot;
    private GroupBox gbSelected;
    private TableLayoutPanel tlpSelected;
    private Label lblStateId;
    private NumericUpDown nudStateId;
    private Label lblBuffName;
    private TextBox txtBuffName;
    private Label lblBuffLevel;
    private NumericUpDown nudBuffLevel;
    private Label lblDuration;
    private NumericUpDown nudDurationMinutes;
    private Label lblDurationUnit;
    private TableLayoutPanel tlpBottom;
    private GroupBox gbGlobalBuffs;
    private FlowLayoutPanel flpGlobalButtons;
    private Button btnAddTimedWorldState;
    private Button btnAddEventState;
    private Button btnRemoveEventState;
    private GroupBox gbPlayerCreature;
    private TableLayoutPanel tlpPlayerCreature;
    private RadioButton rbPlayer;
    private RadioButton rbSummon;
    private Button btnAddBuff;
    private Button btnRemoveBuff;

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
        gbBuffActions = new GroupBox();
        tlpRoot = new TableLayoutPanel();
        gbSelected = new GroupBox();
        tlpSelected = new TableLayoutPanel();
        lblStateId = new Label();
        nudStateId = new NumericUpDown();
        lblBuffName = new Label();
        txtBuffName = new TextBox();
        lblBuffLevel = new Label();
        nudBuffLevel = new NumericUpDown();
        lblDuration = new Label();
        nudDurationMinutes = new NumericUpDown();
        lblDurationUnit = new Label();
        tlpBottom = new TableLayoutPanel();
        gbGlobalBuffs = new GroupBox();
        flpGlobalButtons = new FlowLayoutPanel();
        btnAddTimedWorldState = new Button();
        btnAddEventState = new Button();
        btnRemoveEventState = new Button();
        gbPlayerCreature = new GroupBox();
        tlpPlayerCreature = new TableLayoutPanel();
        rbPlayer = new RadioButton();
        rbSummon = new RadioButton();
        btnAddBuff = new Button();
        btnRemoveBuff = new Button();
        gbBuffActions.SuspendLayout();
        tlpRoot.SuspendLayout();
        gbSelected.SuspendLayout();
        tlpSelected.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudStateId).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudBuffLevel).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudDurationMinutes).BeginInit();
        tlpBottom.SuspendLayout();
        gbGlobalBuffs.SuspendLayout();
        flpGlobalButtons.SuspendLayout();
        gbPlayerCreature.SuspendLayout();
        tlpPlayerCreature.SuspendLayout();
        SuspendLayout();
        // 
        // gbBuffActions
        // 
        gbBuffActions.Controls.Add(tlpRoot);
        gbBuffActions.Dock = DockStyle.Fill;
        gbBuffActions.Location = new Point(0, 0);
        gbBuffActions.Name = "gbBuffActions";
        gbBuffActions.Size = new Size(430, 350);
        gbBuffActions.TabIndex = 0;
        gbBuffActions.TabStop = false;
        gbBuffActions.Text = "Buffs";
        // 
        // tlpRoot
        // 
        tlpRoot.ColumnCount = 1;
        tlpRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpRoot.Controls.Add(gbSelected, 0, 0);
        tlpRoot.Controls.Add(tlpBottom, 0, 1);
        tlpRoot.Dock = DockStyle.Fill;
        tlpRoot.Location = new Point(3, 23);
        tlpRoot.Name = "tlpRoot";
        tlpRoot.RowCount = 3;
        tlpRoot.RowStyles.Add(new RowStyle());
        tlpRoot.RowStyles.Add(new RowStyle());
        tlpRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpRoot.Size = new Size(424, 324);
        tlpRoot.TabIndex = 0;
        // 
        // gbSelected
        // 
        gbSelected.Controls.Add(tlpSelected);
        gbSelected.Dock = DockStyle.Top;
        gbSelected.Location = new Point(3, 3);
        gbSelected.Name = "gbSelected";
        gbSelected.Size = new Size(418, 120);
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
        tlpSelected.Controls.Add(lblStateId, 0, 0);
        tlpSelected.Controls.Add(nudStateId, 1, 0);
        tlpSelected.Controls.Add(lblBuffName, 0, 1);
        tlpSelected.Controls.Add(txtBuffName, 1, 1);
        tlpSelected.Controls.Add(lblBuffLevel, 2, 0);
        tlpSelected.Controls.Add(nudBuffLevel, 3, 0);
        tlpSelected.Controls.Add(lblDuration, 2, 1);
        tlpSelected.Controls.Add(nudDurationMinutes, 3, 1);
        tlpSelected.Controls.Add(lblDurationUnit, 3, 2);
        tlpSelected.Dock = DockStyle.Fill;
        tlpSelected.Location = new Point(3, 23);
        tlpSelected.Name = "tlpSelected";
        tlpSelected.RowCount = 3;
        tlpSelected.RowStyles.Add(new RowStyle());
        tlpSelected.RowStyles.Add(new RowStyle());
        tlpSelected.RowStyles.Add(new RowStyle());
        tlpSelected.Size = new Size(412, 94);
        tlpSelected.TabIndex = 0;
        // 
        // lblStateId
        // 
        lblStateId.Anchor = AnchorStyles.Left;
        lblStateId.AutoSize = true;
        lblStateId.Location = new Point(3, 8);
        lblStateId.Name = "lblStateId";
        lblStateId.Size = new Size(51, 20);
        lblStateId.TabIndex = 0;
        lblStateId.Text = "Buff-ID:";
        // 
        // nudStateId
        // 
        nudStateId.Dock = DockStyle.Fill;
        nudStateId.Location = new Point(60, 3);
        nudStateId.Maximum = new decimal(new int[] { 1000000000, 0, 0, 0 });
        nudStateId.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        nudStateId.Name = "nudStateId";
        nudStateId.Size = new Size(145, 27);
        nudStateId.TabIndex = 1;
        nudStateId.Value = new decimal(new int[] { 1, 0, 0, 0 });
        // 
        // lblBuffName
        // 
        lblBuffName.Anchor = AnchorStyles.Left;
        lblBuffName.AutoSize = true;
        lblBuffName.Location = new Point(3, 42);
        lblBuffName.Name = "lblBuffName";
        lblBuffName.Size = new Size(71, 20);
        lblBuffName.TabIndex = 2;
        lblBuffName.Text = "Buffname:";
        // 
        // txtBuffName
        // 
        txtBuffName.Dock = DockStyle.Fill;
        txtBuffName.Location = new Point(80, 37);
        txtBuffName.Name = "txtBuffName";
        txtBuffName.ReadOnly = true;
        txtBuffName.Size = new Size(125, 27);
        txtBuffName.TabIndex = 3;
        // 
        // lblBuffLevel
        // 
        lblBuffLevel.Anchor = AnchorStyles.Left;
        lblBuffLevel.AutoSize = true;
        lblBuffLevel.Location = new Point(211, 8);
        lblBuffLevel.Name = "lblBuffLevel";
        lblBuffLevel.Size = new Size(70, 20);
        lblBuffLevel.TabIndex = 4;
        lblBuffLevel.Text = "Bufflevel:";
        // 
        // nudBuffLevel
        // 
        nudBuffLevel.Dock = DockStyle.Fill;
        nudBuffLevel.Location = new Point(287, 3);
        nudBuffLevel.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
        nudBuffLevel.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        nudBuffLevel.Name = "nudBuffLevel";
        nudBuffLevel.Size = new Size(122, 27);
        nudBuffLevel.TabIndex = 5;
        nudBuffLevel.Value = new decimal(new int[] { 1, 0, 0, 0 });
        // 
        // lblDuration
        // 
        lblDuration.Anchor = AnchorStyles.Left;
        lblDuration.AutoSize = true;
        lblDuration.Location = new Point(211, 42);
        lblDuration.Name = "lblDuration";
        lblDuration.Size = new Size(67, 20);
        lblDuration.TabIndex = 6;
        lblDuration.Text = "Duration:";
        // 
        // nudDurationMinutes
        // 
        nudDurationMinutes.Dock = DockStyle.Fill;
        nudDurationMinutes.Location = new Point(287, 37);
        nudDurationMinutes.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
        nudDurationMinutes.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        nudDurationMinutes.Name = "nudDurationMinutes";
        nudDurationMinutes.Size = new Size(122, 27);
        nudDurationMinutes.TabIndex = 7;
        nudDurationMinutes.Value = new decimal(new int[] { 1, 0, 0, 0 });
        // 
        // lblDurationUnit
        // 
        lblDurationUnit.Anchor = AnchorStyles.Left;
        lblDurationUnit.AutoSize = true;
        lblDurationUnit.Location = new Point(287, 69);
        lblDurationUnit.Name = "lblDurationUnit";
        lblDurationUnit.Size = new Size(30, 20);
        lblDurationUnit.TabIndex = 8;
        lblDurationUnit.Text = "Min";
        // 
        // tlpBottom
        // 
        tlpBottom.ColumnCount = 2;
        tlpBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpBottom.Controls.Add(gbGlobalBuffs, 0, 0);
        tlpBottom.Controls.Add(gbPlayerCreature, 1, 0);
        tlpBottom.Dock = DockStyle.Top;
        tlpBottom.Location = new Point(3, 129);
        tlpBottom.Name = "tlpBottom";
        tlpBottom.RowCount = 1;
        tlpBottom.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpBottom.Size = new Size(418, 166);
        tlpBottom.TabIndex = 1;
        // 
        // gbGlobalBuffs
        // 
        gbGlobalBuffs.Controls.Add(flpGlobalButtons);
        gbGlobalBuffs.Dock = DockStyle.Fill;
        gbGlobalBuffs.Location = new Point(3, 3);
        gbGlobalBuffs.Name = "gbGlobalBuffs";
        gbGlobalBuffs.Size = new Size(203, 160);
        gbGlobalBuffs.TabIndex = 0;
        gbGlobalBuffs.TabStop = false;
        gbGlobalBuffs.Text = "Global Buffs";
        // 
        // flpGlobalButtons
        // 
        flpGlobalButtons.Controls.Add(btnAddTimedWorldState);
        flpGlobalButtons.Controls.Add(btnAddEventState);
        flpGlobalButtons.Controls.Add(btnRemoveEventState);
        flpGlobalButtons.Dock = DockStyle.Fill;
        flpGlobalButtons.FlowDirection = FlowDirection.TopDown;
        flpGlobalButtons.Location = new Point(3, 23);
        flpGlobalButtons.Name = "flpGlobalButtons";
        flpGlobalButtons.Size = new Size(197, 134);
        flpGlobalButtons.TabIndex = 0;
        flpGlobalButtons.WrapContents = false;
        // 
        // btnAddTimedWorldState
        // 
        btnAddTimedWorldState.Location = new Point(3, 3);
        btnAddTimedWorldState.Name = "btnAddTimedWorldState";
        btnAddTimedWorldState.Size = new Size(190, 36);
        btnAddTimedWorldState.TabIndex = 0;
        btnAddTimedWorldState.Text = "Add Timed World State";
        btnAddTimedWorldState.UseVisualStyleBackColor = true;
        btnAddTimedWorldState.Click += btnAddTimedWorldState_Click;
        // 
        // btnAddEventState
        // 
        btnAddEventState.Location = new Point(3, 39);
        btnAddEventState.Name = "btnAddEventState";
        btnAddEventState.Size = new Size(190, 36);
        btnAddEventState.TabIndex = 1;
        btnAddEventState.Text = "Add Event State";
        btnAddEventState.UseVisualStyleBackColor = true;
        btnAddEventState.Click += btnAddEventState_Click;
        // 
        // btnRemoveEventState
        // 
        btnRemoveEventState.Location = new Point(3, 75);
        btnRemoveEventState.Name = "btnRemoveEventState";
        btnRemoveEventState.Size = new Size(190, 36);
        btnRemoveEventState.TabIndex = 2;
        btnRemoveEventState.Text = "Remove Event State";
        btnRemoveEventState.UseVisualStyleBackColor = true;
        btnRemoveEventState.Click += btnRemoveEventState_Click;
        // 
        // gbPlayerCreature
        // 
        gbPlayerCreature.Controls.Add(tlpPlayerCreature);
        gbPlayerCreature.Dock = DockStyle.Fill;
        gbPlayerCreature.Location = new Point(212, 3);
        gbPlayerCreature.Name = "gbPlayerCreature";
        gbPlayerCreature.Size = new Size(203, 160);
        gbPlayerCreature.TabIndex = 1;
        gbPlayerCreature.TabStop = false;
        gbPlayerCreature.Text = "Player/Creature Buffs";
        // 
        // tlpPlayerCreature
        // 
        tlpPlayerCreature.ColumnCount = 1;
        tlpPlayerCreature.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpPlayerCreature.Controls.Add(rbPlayer, 0, 0);
        tlpPlayerCreature.Controls.Add(rbSummon, 0, 1);
        tlpPlayerCreature.Controls.Add(btnAddBuff, 0, 2);
        tlpPlayerCreature.Controls.Add(btnRemoveBuff, 0, 3);
        tlpPlayerCreature.Dock = DockStyle.Fill;
        tlpPlayerCreature.Location = new Point(3, 23);
        tlpPlayerCreature.Name = "tlpPlayerCreature";
        tlpPlayerCreature.RowCount = 5;
        tlpPlayerCreature.RowStyles.Add(new RowStyle());
        tlpPlayerCreature.RowStyles.Add(new RowStyle());
        tlpPlayerCreature.RowStyles.Add(new RowStyle());
        tlpPlayerCreature.RowStyles.Add(new RowStyle());
        tlpPlayerCreature.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpPlayerCreature.Size = new Size(197, 134);
        tlpPlayerCreature.TabIndex = 0;
        // 
        // rbPlayer
        // 
        rbPlayer.AutoSize = true;
        rbPlayer.Location = new Point(3, 3);
        rbPlayer.Name = "rbPlayer";
        rbPlayer.Size = new Size(67, 24);
        rbPlayer.TabIndex = 0;
        rbPlayer.TabStop = true;
        rbPlayer.Text = "Player";
        rbPlayer.UseVisualStyleBackColor = true;
        // 
        // rbSummon
        // 
        rbSummon.AutoSize = true;
        rbSummon.Location = new Point(3, 33);
        rbSummon.Name = "rbSummon";
        rbSummon.Size = new Size(82, 24);
        rbSummon.TabIndex = 1;
        rbSummon.TabStop = true;
        rbSummon.Text = "Summon";
        rbSummon.UseVisualStyleBackColor = true;
        // 
        // btnAddBuff
        // 
        btnAddBuff.Dock = DockStyle.Fill;
        btnAddBuff.Location = new Point(3, 63);
        btnAddBuff.Name = "btnAddBuff";
        btnAddBuff.Size = new Size(191, 36);
        btnAddBuff.TabIndex = 2;
        btnAddBuff.Text = "Add Buff";
        btnAddBuff.UseVisualStyleBackColor = true;
        btnAddBuff.Click += btnAddBuff_Click;
        // 
        // btnRemoveBuff
        // 
        btnRemoveBuff.Dock = DockStyle.Fill;
        btnRemoveBuff.Location = new Point(3, 99);
        btnRemoveBuff.Name = "btnRemoveBuff";
        btnRemoveBuff.Size = new Size(191, 36);
        btnRemoveBuff.TabIndex = 3;
        btnRemoveBuff.Text = "Remove Buff";
        btnRemoveBuff.UseVisualStyleBackColor = true;
        btnRemoveBuff.Click += btnRemoveBuff_Click;
        // 
        // BuffsActionsControl
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(gbBuffActions);
        Name = "BuffsActionsControl";
        Size = new Size(430, 350);
        AutoSize = true;
        AutoSizeMode = AutoSizeMode.GrowAndShrink;
        gbBuffActions.ResumeLayout(false);
        tlpRoot.ResumeLayout(false);
        gbSelected.ResumeLayout(false);
        tlpSelected.ResumeLayout(false);
        tlpSelected.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)nudStateId).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudBuffLevel).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudDurationMinutes).EndInit();
        tlpBottom.ResumeLayout(false);
        gbGlobalBuffs.ResumeLayout(false);
        flpGlobalButtons.ResumeLayout(false);
        gbPlayerCreature.ResumeLayout(false);
        tlpPlayerCreature.ResumeLayout(false);
        tlpPlayerCreature.PerformLayout();
        ResumeLayout(false);
    }
}
