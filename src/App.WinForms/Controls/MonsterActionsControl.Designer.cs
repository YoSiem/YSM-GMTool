namespace App.WinForms.Controls;

partial class MonsterActionsControl
{
    private System.ComponentModel.IContainer components = null;
    private GroupBox gbMonsterActions;
    private TableLayoutPanel tlp;
    private Label lblAmount;
    private NumericUpDown nudAmount;
    private Label lblSpawnMode;
    private ComboBox cmbSpawnMode;
    private Label lblX;
    private NumericUpDown nudX;
    private Label lblY;
    private NumericUpDown nudY;
    private Label lblLayer;
    private NumericUpDown nudLayer;
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
        lblAmount = new Label();
        nudAmount = new NumericUpDown();
        lblSpawnMode = new Label();
        cmbSpawnMode = new ComboBox();
        lblX = new Label();
        nudX = new NumericUpDown();
        lblY = new Label();
        nudY = new NumericUpDown();
        lblLayer = new Label();
        nudLayer = new NumericUpDown();
        btnCreateSpawnCommand = new Button();
        gbMonsterActions.SuspendLayout();
        tlp.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudAmount).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudX).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudY).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudLayer).BeginInit();
        SuspendLayout();
        // 
        // gbMonsterActions
        // 
        gbMonsterActions.Controls.Add(tlp);
        gbMonsterActions.Dock = DockStyle.Fill;
        gbMonsterActions.Location = new Point(0, 0);
        gbMonsterActions.Name = "gbMonsterActions";
        gbMonsterActions.Size = new Size(430, 250);
        gbMonsterActions.TabIndex = 0;
        gbMonsterActions.TabStop = false;
        gbMonsterActions.Text = "Monster Commands";
        // 
        // tlp
        // 
        tlp.ColumnCount = 2;
        tlp.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlp.Controls.Add(lblAmount, 0, 0);
        tlp.Controls.Add(nudAmount, 1, 0);
        tlp.Controls.Add(lblSpawnMode, 0, 1);
        tlp.Controls.Add(cmbSpawnMode, 1, 1);
        tlp.Controls.Add(lblX, 0, 2);
        tlp.Controls.Add(nudX, 1, 2);
        tlp.Controls.Add(lblY, 0, 3);
        tlp.Controls.Add(nudY, 1, 3);
        tlp.Controls.Add(lblLayer, 0, 4);
        tlp.Controls.Add(nudLayer, 1, 4);
        tlp.Controls.Add(btnCreateSpawnCommand, 0, 5);
        tlp.Dock = DockStyle.Fill;
        tlp.Location = new Point(3, 23);
        tlp.Name = "tlp";
        tlp.RowCount = 6;
        tlp.RowStyles.Add(new RowStyle());
        tlp.RowStyles.Add(new RowStyle());
        tlp.RowStyles.Add(new RowStyle());
        tlp.RowStyles.Add(new RowStyle());
        tlp.RowStyles.Add(new RowStyle());
        tlp.RowStyles.Add(new RowStyle());
        tlp.Size = new Size(424, 224);
        tlp.TabIndex = 0;
        // 
        // lblAmount
        // 
        lblAmount.Anchor = AnchorStyles.Left;
        lblAmount.AutoSize = true;
        lblAmount.Location = new Point(3, 9);
        lblAmount.Name = "lblAmount";
        lblAmount.Size = new Size(59, 20);
        lblAmount.TabIndex = 0;
        lblAmount.Text = "Amount";
        // 
        // nudAmount
        // 
        nudAmount.Dock = DockStyle.Fill;
        nudAmount.Location = new Point(94, 3);
        nudAmount.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
        nudAmount.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        nudAmount.Name = "nudAmount";
        nudAmount.Size = new Size(327, 27);
        nudAmount.TabIndex = 1;
        nudAmount.Value = new decimal(new int[] { 1, 0, 0, 0 });
        // 
        // lblSpawnMode
        // 
        lblSpawnMode.Anchor = AnchorStyles.Left;
        lblSpawnMode.AutoSize = true;
        lblSpawnMode.Location = new Point(3, 43);
        lblSpawnMode.Name = "lblSpawnMode";
        lblSpawnMode.Size = new Size(85, 20);
        lblSpawnMode.TabIndex = 2;
        lblSpawnMode.Text = "Spawn type";
        // 
        // cmbSpawnMode
        // 
        cmbSpawnMode.Dock = DockStyle.Fill;
        cmbSpawnMode.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbSpawnMode.FormattingEnabled = true;
        cmbSpawnMode.Items.AddRange(new object[] { "At your place", "Coordinates" });
        cmbSpawnMode.Location = new Point(94, 37);
        cmbSpawnMode.Name = "cmbSpawnMode";
        cmbSpawnMode.Size = new Size(327, 28);
        cmbSpawnMode.TabIndex = 3;
        cmbSpawnMode.SelectedIndexChanged += cmbSpawnMode_SelectedIndexChanged;
        // 
        // lblX
        // 
        lblX.Anchor = AnchorStyles.Left;
        lblX.AutoSize = true;
        lblX.Location = new Point(3, 77);
        lblX.Name = "lblX";
        lblX.Size = new Size(17, 20);
        lblX.TabIndex = 4;
        lblX.Text = "X";
        // 
        // nudX
        // 
        nudX.Dock = DockStyle.Fill;
        nudX.Location = new Point(94, 71);
        nudX.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
        nudX.Minimum = new decimal(new int[] { 100000, 0, 0, int.MinValue });
        nudX.Name = "nudX";
        nudX.Size = new Size(327, 27);
        nudX.TabIndex = 5;
        // 
        // lblY
        // 
        lblY.Anchor = AnchorStyles.Left;
        lblY.AutoSize = true;
        lblY.Location = new Point(3, 111);
        lblY.Name = "lblY";
        lblY.Size = new Size(16, 20);
        lblY.TabIndex = 6;
        lblY.Text = "Y";
        // 
        // nudY
        // 
        nudY.Dock = DockStyle.Fill;
        nudY.Location = new Point(94, 105);
        nudY.Maximum = new decimal(new int[] { 100000, 0, 0, 0 });
        nudY.Minimum = new decimal(new int[] { 100000, 0, 0, int.MinValue });
        nudY.Name = "nudY";
        nudY.Size = new Size(327, 27);
        nudY.TabIndex = 7;
        // 
        // lblLayer
        // 
        lblLayer.Anchor = AnchorStyles.Left;
        lblLayer.AutoSize = true;
        lblLayer.Location = new Point(3, 145);
        lblLayer.Name = "lblLayer";
        lblLayer.Size = new Size(44, 20);
        lblLayer.TabIndex = 8;
        lblLayer.Text = "Layer";
        // 
        // nudLayer
        // 
        nudLayer.Dock = DockStyle.Fill;
        nudLayer.Location = new Point(94, 139);
        nudLayer.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
        nudLayer.Minimum = new decimal(new int[] { 20, 0, 0, int.MinValue });
        nudLayer.Name = "nudLayer";
        nudLayer.Size = new Size(327, 27);
        nudLayer.TabIndex = 9;
        // 
        // btnCreateSpawnCommand
        // 
        tlp.SetColumnSpan(btnCreateSpawnCommand, 2);
        btnCreateSpawnCommand.Dock = DockStyle.Fill;
        btnCreateSpawnCommand.Location = new Point(3, 173);
        btnCreateSpawnCommand.Name = "btnCreateSpawnCommand";
        btnCreateSpawnCommand.Size = new Size(418, 29);
        btnCreateSpawnCommand.TabIndex = 10;
        btnCreateSpawnCommand.Text = "Create spawn command";
        btnCreateSpawnCommand.UseVisualStyleBackColor = true;
        btnCreateSpawnCommand.Click += btnCreateSpawnCommand_Click;
        // 
        // MonsterActionsControl
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(gbMonsterActions);
        Name = "MonsterActionsControl";
        Size = new Size(430, 250);
        gbMonsterActions.ResumeLayout(false);
        tlp.ResumeLayout(false);
        tlp.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)nudAmount).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudX).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudY).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudLayer).EndInit();
        ResumeLayout(false);
    }
}
