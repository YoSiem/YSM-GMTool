namespace App.WinForms.Controls;

partial class MonsterActionsControl
{
    private System.ComponentModel.IContainer components = null;
    private GroupBox gbMonsterActions;
    private TableLayoutPanel tlp;
    private Label lblSpawnMode;
    private ComboBox cmbSpawnMode;
    private Label lblMonsterId;
    private NumericUpDown nudMonsterId;
    private Label lblAmount;
    private NumericUpDown nudAmount;
    private Label lblX;
    private NumericUpDown nudX;
    private Label lblY;
    private NumericUpDown nudY;
    private Label lblLayer;
    private NumericUpDown nudLayer;
    private CheckBox chkUseLifetime;
    private Label lblMinutes;
    private NumericUpDown nudMinutesLifetime;
    private Button btnCreateSpawnCommand;

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
        gbMonsterActions = new GroupBox();
        tlp = new TableLayoutPanel();
        lblSpawnMode = new Label();
        cmbSpawnMode = new ComboBox();
        lblMonsterId = new Label();
        nudMonsterId = new NumericUpDown();
        lblAmount = new Label();
        nudAmount = new NumericUpDown();
        lblX = new Label();
        nudX = new NumericUpDown();
        lblY = new Label();
        nudY = new NumericUpDown();
        lblLayer = new Label();
        nudLayer = new NumericUpDown();
        chkUseLifetime = new CheckBox();
        lblMinutes = new Label();
        nudMinutesLifetime = new NumericUpDown();
        btnCreateSpawnCommand = new Button();
        gbMonsterActions.SuspendLayout();
        tlp.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudMonsterId).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudAmount).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudX).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudY).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudLayer).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudMinutesLifetime).BeginInit();
        SuspendLayout();
        // 
        // gbMonsterActions
        // 
        gbMonsterActions.Controls.Add(tlp);
        gbMonsterActions.Dock = DockStyle.Fill;
        gbMonsterActions.Location = new Point(0, 0);
        gbMonsterActions.Name = "gbMonsterActions";
        gbMonsterActions.Size = new Size(430, 370);
        gbMonsterActions.TabIndex = 0;
        gbMonsterActions.TabStop = false;
        gbMonsterActions.Text = "Monster";
        // 
        // tlp
        // 
        tlp.ColumnCount = 2;
        tlp.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlp.Controls.Add(lblSpawnMode, 0, 0);
        tlp.Controls.Add(cmbSpawnMode, 1, 0);
        tlp.Controls.Add(lblMonsterId, 0, 1);
        tlp.Controls.Add(nudMonsterId, 1, 1);
        tlp.Controls.Add(lblAmount, 0, 2);
        tlp.Controls.Add(nudAmount, 1, 2);
        tlp.Controls.Add(lblX, 0, 3);
        tlp.Controls.Add(nudX, 1, 3);
        tlp.Controls.Add(lblY, 0, 4);
        tlp.Controls.Add(nudY, 1, 4);
        tlp.Controls.Add(lblLayer, 0, 5);
        tlp.Controls.Add(nudLayer, 1, 5);
        tlp.Controls.Add(chkUseLifetime, 0, 6);
        tlp.Controls.Add(lblMinutes, 0, 7);
        tlp.Controls.Add(nudMinutesLifetime, 1, 7);
        tlp.Controls.Add(btnCreateSpawnCommand, 0, 8);
        tlp.Dock = DockStyle.Fill;
        tlp.Location = new Point(3, 23);
        tlp.Name = "tlp";
        tlp.RowCount = 9;
        tlp.RowStyles.Add(new RowStyle());
        tlp.RowStyles.Add(new RowStyle());
        tlp.RowStyles.Add(new RowStyle());
        tlp.RowStyles.Add(new RowStyle());
        tlp.RowStyles.Add(new RowStyle());
        tlp.RowStyles.Add(new RowStyle());
        tlp.RowStyles.Add(new RowStyle());
        tlp.RowStyles.Add(new RowStyle());
        tlp.RowStyles.Add(new RowStyle());
        tlp.Size = new Size(424, 344);
        tlp.TabIndex = 0;
        // 
        // lblSpawnMode
        // 
        lblSpawnMode.Anchor = AnchorStyles.Left;
        lblSpawnMode.AutoSize = true;
        lblSpawnMode.Location = new Point(3, 8);
        lblSpawnMode.Name = "lblSpawnMode";
        lblSpawnMode.Size = new Size(88, 20);
        lblSpawnMode.TabIndex = 0;
        lblSpawnMode.Text = "Spawntype:";
        // 
        // cmbSpawnMode
        // 
        cmbSpawnMode.Dock = DockStyle.Fill;
        cmbSpawnMode.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbSpawnMode.FormattingEnabled = true;
        cmbSpawnMode.Items.AddRange(new object[] { "At your place", "At selected player place", "At specific coordinates" });
        cmbSpawnMode.Location = new Point(97, 3);
        cmbSpawnMode.Name = "cmbSpawnMode";
        cmbSpawnMode.Size = new Size(324, 28);
        cmbSpawnMode.TabIndex = 1;
        cmbSpawnMode.SelectedIndexChanged += cmbSpawnMode_SelectedIndexChanged;
        // 
        // lblMonsterId
        // 
        lblMonsterId.Anchor = AnchorStyles.Left;
        lblMonsterId.AutoSize = true;
        lblMonsterId.Location = new Point(3, 43);
        lblMonsterId.Name = "lblMonsterId";
        lblMonsterId.Size = new Size(26, 20);
        lblMonsterId.TabIndex = 2;
        lblMonsterId.Text = "ID:";
        // 
        // nudMonsterId
        // 
        nudMonsterId.Dock = DockStyle.Fill;
        nudMonsterId.Location = new Point(97, 37);
        nudMonsterId.Maximum = new decimal(new int[] { 1000000000, 0, 0, 0 });
        nudMonsterId.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        nudMonsterId.Name = "nudMonsterId";
        nudMonsterId.Size = new Size(324, 27);
        nudMonsterId.TabIndex = 3;
        nudMonsterId.Value = new decimal(new int[] { 1, 0, 0, 0 });
        // 
        // lblAmount
        // 
        lblAmount.Anchor = AnchorStyles.Left;
        lblAmount.AutoSize = true;
        lblAmount.Location = new Point(3, 77);
        lblAmount.Name = "lblAmount";
        lblAmount.Size = new Size(67, 20);
        lblAmount.TabIndex = 4;
        lblAmount.Text = "Amount:";
        // 
        // nudAmount
        // 
        nudAmount.Dock = DockStyle.Fill;
        nudAmount.Location = new Point(97, 71);
        nudAmount.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
        nudAmount.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        nudAmount.Name = "nudAmount";
        nudAmount.Size = new Size(324, 27);
        nudAmount.TabIndex = 5;
        nudAmount.Value = new decimal(new int[] { 1, 0, 0, 0 });
        // 
        // lblX
        // 
        lblX.Anchor = AnchorStyles.Left;
        lblX.AutoSize = true;
        lblX.Location = new Point(3, 111);
        lblX.Name = "lblX";
        lblX.Size = new Size(20, 20);
        lblX.TabIndex = 6;
        lblX.Text = "X:";
        // 
        // nudX
        // 
        nudX.Dock = DockStyle.Fill;
        nudX.Location = new Point(97, 105);
        nudX.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
        nudX.Minimum = new decimal(new int[] { 100000, 0, 0, int.MinValue });
        nudX.Name = "nudX";
        nudX.Size = new Size(324, 27);
        nudX.TabIndex = 7;
        // 
        // lblY
        // 
        lblY.Anchor = AnchorStyles.Left;
        lblY.AutoSize = true;
        lblY.Location = new Point(3, 145);
        lblY.Name = "lblY";
        lblY.Size = new Size(19, 20);
        lblY.TabIndex = 8;
        lblY.Text = "Y:";
        // 
        // nudY
        // 
        nudY.Dock = DockStyle.Fill;
        nudY.Location = new Point(97, 139);
        nudY.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
        nudY.Minimum = new decimal(new int[] { 100000, 0, 0, int.MinValue });
        nudY.Name = "nudY";
        nudY.Size = new Size(324, 27);
        nudY.TabIndex = 9;
        // 
        // lblLayer
        // 
        lblLayer.Anchor = AnchorStyles.Left;
        lblLayer.AutoSize = true;
        lblLayer.Location = new Point(3, 179);
        lblLayer.Name = "lblLayer";
        lblLayer.Size = new Size(47, 20);
        lblLayer.TabIndex = 10;
        lblLayer.Text = "Layer:";
        // 
        // nudLayer
        // 
        nudLayer.Dock = DockStyle.Fill;
        nudLayer.Location = new Point(97, 173);
        nudLayer.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
        nudLayer.Minimum = new decimal(new int[] { 1000, 0, 0, int.MinValue });
        nudLayer.Name = "nudLayer";
        nudLayer.Size = new Size(324, 27);
        nudLayer.TabIndex = 11;
        nudLayer.Value = new decimal(new int[] { 1, 0, 0, 0 });
        // 
        // chkUseLifetime
        // 
        chkUseLifetime.Anchor = AnchorStyles.Left;
        chkUseLifetime.AutoSize = true;
        chkUseLifetime.Location = new Point(3, 206);
        chkUseLifetime.Name = "chkUseLifetime";
        chkUseLifetime.Size = new Size(85, 24);
        chkUseLifetime.TabIndex = 12;
        chkUseLifetime.Text = "Lifetime:";
        chkUseLifetime.UseVisualStyleBackColor = true;
        chkUseLifetime.CheckedChanged += chkUseLifetime_CheckedChanged;
        // 
        // lblMinutes
        // 
        lblMinutes.Anchor = AnchorStyles.Left;
        lblMinutes.AutoSize = true;
        lblMinutes.Location = new Point(3, 241);
        lblMinutes.Name = "lblMinutes";
        lblMinutes.Size = new Size(65, 20);
        lblMinutes.TabIndex = 13;
        lblMinutes.Text = "Minutes:";
        // 
        // nudMinutesLifetime
        // 
        nudMinutesLifetime.Dock = DockStyle.Fill;
        nudMinutesLifetime.Location = new Point(97, 236);
        nudMinutesLifetime.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
        nudMinutesLifetime.Minimum = new decimal(new int[] { 1, 0, 0, int.MinValue });
        nudMinutesLifetime.Name = "nudMinutesLifetime";
        nudMinutesLifetime.Size = new Size(324, 27);
        nudMinutesLifetime.TabIndex = 14;
        nudMinutesLifetime.Value = new decimal(new int[] { 1, 0, 0, int.MinValue });
        // 
        // btnCreateSpawnCommand
        // 
        tlp.SetColumnSpan(btnCreateSpawnCommand, 2);
        btnCreateSpawnCommand.Dock = DockStyle.Top;
        btnCreateSpawnCommand.Location = new Point(3, 269);
        btnCreateSpawnCommand.Name = "btnCreateSpawnCommand";
        btnCreateSpawnCommand.Size = new Size(418, 36);
        btnCreateSpawnCommand.TabIndex = 15;
        btnCreateSpawnCommand.Text = "Create Command";
        btnCreateSpawnCommand.UseVisualStyleBackColor = true;
        btnCreateSpawnCommand.Click += btnCreateSpawnCommand_Click;
        // 
        // MonsterActionsControl
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(gbMonsterActions);
        Name = "MonsterActionsControl";
        Size = new Size(430, 370);
        AutoSize = true;
        AutoSizeMode = AutoSizeMode.GrowAndShrink;
        gbMonsterActions.ResumeLayout(false);
        tlp.ResumeLayout(false);
        tlp.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)nudMonsterId).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudAmount).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudX).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudY).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudLayer).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudMinutesLifetime).EndInit();
        ResumeLayout(false);
    }
}
