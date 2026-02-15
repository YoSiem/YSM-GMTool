namespace App.WinForms.Controls;

partial class ItemsActionsControl
{
    private System.ComponentModel.IContainer components = null;
    private GroupBox gbItemActions;
    private TableLayoutPanel tlp;
    private Label lblTargetMode;
    private ComboBox cmbTargetMode;
    private Label lblTargetPlayer;
    private TextBox txtTargetPlayer;
    private Label lblAmount;
    private NumericUpDown nudAmount;
    private Label lblLevel;
    private NumericUpDown nudLevel;
    private Label lblEnhance;
    private NumericUpDown nudEnhance;
    private Label lblItmCode;
    private NumericUpDown nudItmCode;
    private Button btnCreateGiveCommand;

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
        gbItemActions = new GroupBox();
        tlp = new TableLayoutPanel();
        lblTargetMode = new Label();
        cmbTargetMode = new ComboBox();
        lblTargetPlayer = new Label();
        txtTargetPlayer = new TextBox();
        lblAmount = new Label();
        nudAmount = new NumericUpDown();
        lblLevel = new Label();
        nudLevel = new NumericUpDown();
        lblEnhance = new Label();
        nudEnhance = new NumericUpDown();
        lblItmCode = new Label();
        nudItmCode = new NumericUpDown();
        btnCreateGiveCommand = new Button();
        gbItemActions.SuspendLayout();
        tlp.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudAmount).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudLevel).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudEnhance).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudItmCode).BeginInit();
        SuspendLayout();
        // 
        // gbItemActions
        // 
        gbItemActions.Controls.Add(tlp);
        gbItemActions.Dock = DockStyle.Fill;
        gbItemActions.Location = new Point(0, 0);
        gbItemActions.Name = "gbItemActions";
        gbItemActions.Size = new Size(430, 300);
        gbItemActions.TabIndex = 0;
        gbItemActions.TabStop = false;
        gbItemActions.Text = "Item Commands";
        // 
        // tlp
        // 
        tlp.ColumnCount = 2;
        tlp.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlp.Controls.Add(lblTargetMode, 0, 0);
        tlp.Controls.Add(cmbTargetMode, 1, 0);
        tlp.Controls.Add(lblTargetPlayer, 0, 1);
        tlp.Controls.Add(txtTargetPlayer, 1, 1);
        tlp.Controls.Add(lblAmount, 0, 2);
        tlp.Controls.Add(nudAmount, 1, 2);
        tlp.Controls.Add(lblLevel, 0, 3);
        tlp.Controls.Add(nudLevel, 1, 3);
        tlp.Controls.Add(lblEnhance, 0, 4);
        tlp.Controls.Add(nudEnhance, 1, 4);
        tlp.Controls.Add(lblItmCode, 0, 5);
        tlp.Controls.Add(nudItmCode, 1, 5);
        tlp.Controls.Add(btnCreateGiveCommand, 0, 6);
        tlp.Dock = DockStyle.Fill;
        tlp.Location = new Point(3, 23);
        tlp.Name = "tlp";
        tlp.RowCount = 7;
        tlp.RowStyles.Add(new RowStyle());
        tlp.RowStyles.Add(new RowStyle());
        tlp.RowStyles.Add(new RowStyle());
        tlp.RowStyles.Add(new RowStyle());
        tlp.RowStyles.Add(new RowStyle());
        tlp.RowStyles.Add(new RowStyle());
        tlp.RowStyles.Add(new RowStyle());
        tlp.Size = new Size(424, 274);
        tlp.TabIndex = 0;
        // 
        // lblTargetMode
        // 
        lblTargetMode.Anchor = AnchorStyles.Left;
        lblTargetMode.AutoSize = true;
        lblTargetMode.Location = new Point(3, 9);
        lblTargetMode.Name = "lblTargetMode";
        lblTargetMode.Size = new Size(88, 20);
        lblTargetMode.TabIndex = 0;
        lblTargetMode.Text = "Target mode";
        // 
        // cmbTargetMode
        // 
        cmbTargetMode.Dock = DockStyle.Fill;
        cmbTargetMode.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbTargetMode.FormattingEnabled = true;
        cmbTargetMode.Items.AddRange(new object[] { "Self", "Other player" });
        cmbTargetMode.Location = new Point(97, 3);
        cmbTargetMode.Name = "cmbTargetMode";
        cmbTargetMode.Size = new Size(324, 28);
        cmbTargetMode.TabIndex = 1;
        cmbTargetMode.SelectedIndexChanged += cmbTargetMode_SelectedIndexChanged;
        // 
        // lblTargetPlayer
        // 
        lblTargetPlayer.Anchor = AnchorStyles.Left;
        lblTargetPlayer.AutoSize = true;
        lblTargetPlayer.Location = new Point(3, 43);
        lblTargetPlayer.Name = "lblTargetPlayer";
        lblTargetPlayer.Size = new Size(87, 20);
        lblTargetPlayer.TabIndex = 2;
        lblTargetPlayer.Text = "Target player";
        // 
        // txtTargetPlayer
        // 
        txtTargetPlayer.Dock = DockStyle.Fill;
        txtTargetPlayer.Location = new Point(97, 37);
        txtTargetPlayer.Name = "txtTargetPlayer";
        txtTargetPlayer.PlaceholderText = "required for Other player";
        txtTargetPlayer.Size = new Size(324, 27);
        txtTargetPlayer.TabIndex = 3;
        // 
        // lblAmount
        // 
        lblAmount.Anchor = AnchorStyles.Left;
        lblAmount.AutoSize = true;
        lblAmount.Location = new Point(3, 77);
        lblAmount.Name = "lblAmount";
        lblAmount.Size = new Size(59, 20);
        lblAmount.TabIndex = 4;
        lblAmount.Text = "Amount";
        // 
        // nudAmount
        // 
        nudAmount.Dock = DockStyle.Fill;
        nudAmount.Location = new Point(97, 71);
        nudAmount.Maximum = new decimal(new int[] { 99999, 0, 0, 0 });
        nudAmount.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        nudAmount.Name = "nudAmount";
        nudAmount.Size = new Size(324, 27);
        nudAmount.TabIndex = 5;
        nudAmount.Value = new decimal(new int[] { 1, 0, 0, 0 });
        // 
        // lblLevel
        // 
        lblLevel.Anchor = AnchorStyles.Left;
        lblLevel.AutoSize = true;
        lblLevel.Location = new Point(3, 111);
        lblLevel.Name = "lblLevel";
        lblLevel.Size = new Size(43, 20);
        lblLevel.TabIndex = 6;
        lblLevel.Text = "Level";
        // 
        // nudLevel
        // 
        nudLevel.Dock = DockStyle.Fill;
        nudLevel.Location = new Point(97, 105);
        nudLevel.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
        nudLevel.Name = "nudLevel";
        nudLevel.Size = new Size(324, 27);
        nudLevel.TabIndex = 7;
        // 
        // lblEnhance
        // 
        lblEnhance.Anchor = AnchorStyles.Left;
        lblEnhance.AutoSize = true;
        lblEnhance.Location = new Point(3, 145);
        lblEnhance.Name = "lblEnhance";
        lblEnhance.Size = new Size(64, 20);
        lblEnhance.TabIndex = 8;
        lblEnhance.Text = "Enhance";
        // 
        // nudEnhance
        // 
        nudEnhance.Dock = DockStyle.Fill;
        nudEnhance.Location = new Point(97, 139);
        nudEnhance.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
        nudEnhance.Name = "nudEnhance";
        nudEnhance.Size = new Size(324, 27);
        nudEnhance.TabIndex = 9;
        // 
        // lblItmCode
        // 
        lblItmCode.Anchor = AnchorStyles.Left;
        lblItmCode.AutoSize = true;
        lblItmCode.Location = new Point(3, 179);
        lblItmCode.Name = "lblItmCode";
        lblItmCode.Size = new Size(65, 20);
        lblItmCode.TabIndex = 10;
        lblItmCode.Text = "ITM code";
        // 
        // nudItmCode
        // 
        nudItmCode.Dock = DockStyle.Fill;
        nudItmCode.Location = new Point(97, 173);
        nudItmCode.Maximum = new decimal(new int[] { int.MaxValue, 0, 0, 0 });
        nudItmCode.Name = "nudItmCode";
        nudItmCode.Size = new Size(324, 27);
        nudItmCode.TabIndex = 11;
        // 
        // btnCreateGiveCommand
        // 
        tlp.SetColumnSpan(btnCreateGiveCommand, 2);
        btnCreateGiveCommand.Dock = DockStyle.Fill;
        btnCreateGiveCommand.Location = new Point(3, 207);
        btnCreateGiveCommand.Name = "btnCreateGiveCommand";
        btnCreateGiveCommand.Size = new Size(418, 29);
        btnCreateGiveCommand.TabIndex = 12;
        btnCreateGiveCommand.Text = "Create item command";
        btnCreateGiveCommand.UseVisualStyleBackColor = true;
        btnCreateGiveCommand.Click += btnCreateGiveCommand_Click;
        // 
        // ItemsActionsControl
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(gbItemActions);
        Name = "ItemsActionsControl";
        Size = new Size(430, 300);
        gbItemActions.ResumeLayout(false);
        tlp.ResumeLayout(false);
        tlp.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)nudAmount).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudLevel).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudEnhance).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudItmCode).EndInit();
        ResumeLayout(false);
    }
}
