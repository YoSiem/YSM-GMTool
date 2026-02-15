namespace App.WinForms.Controls;

partial class SummonsActionsControl
{
    private System.ComponentModel.IContainer components = null;
    private GroupBox gbSummonActions;
    private TableLayoutPanel tlp;
    private Label lblAmount;
    private NumericUpDown nudAmount;
    private Button btnCreateSummonCommand;

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
        tlp = new TableLayoutPanel();
        lblAmount = new Label();
        nudAmount = new NumericUpDown();
        btnCreateSummonCommand = new Button();
        gbSummonActions.SuspendLayout();
        tlp.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudAmount).BeginInit();
        SuspendLayout();
        // 
        // gbSummonActions
        // 
        gbSummonActions.Controls.Add(tlp);
        gbSummonActions.Dock = DockStyle.Fill;
        gbSummonActions.Location = new Point(0, 0);
        gbSummonActions.Name = "gbSummonActions";
        gbSummonActions.Size = new Size(430, 160);
        gbSummonActions.TabIndex = 0;
        gbSummonActions.TabStop = false;
        gbSummonActions.Text = "Summon Commands";
        // 
        // tlp
        // 
        tlp.ColumnCount = 2;
        tlp.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlp.Controls.Add(lblAmount, 0, 0);
        tlp.Controls.Add(nudAmount, 1, 0);
        tlp.Controls.Add(btnCreateSummonCommand, 0, 1);
        tlp.Dock = DockStyle.Fill;
        tlp.Location = new Point(3, 23);
        tlp.Name = "tlp";
        tlp.RowCount = 2;
        tlp.RowStyles.Add(new RowStyle());
        tlp.RowStyles.Add(new RowStyle());
        tlp.Size = new Size(424, 134);
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
        nudAmount.Location = new Point(68, 3);
        nudAmount.Maximum = new decimal(new int[] { 10000, 0, 0, 0 });
        nudAmount.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        nudAmount.Name = "nudAmount";
        nudAmount.Size = new Size(353, 27);
        nudAmount.TabIndex = 1;
        nudAmount.Value = new decimal(new int[] { 1, 0, 0, 0 });
        // 
        // btnCreateSummonCommand
        // 
        tlp.SetColumnSpan(btnCreateSummonCommand, 2);
        btnCreateSummonCommand.Dock = DockStyle.Fill;
        btnCreateSummonCommand.Location = new Point(3, 36);
        btnCreateSummonCommand.Name = "btnCreateSummonCommand";
        btnCreateSummonCommand.Size = new Size(418, 29);
        btnCreateSummonCommand.TabIndex = 2;
        btnCreateSummonCommand.Text = "Create summon command";
        btnCreateSummonCommand.UseVisualStyleBackColor = true;
        btnCreateSummonCommand.Click += btnCreateSummonCommand_Click;
        // 
        // SummonsActionsControl
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(gbSummonActions);
        Name = "SummonsActionsControl";
        Size = new Size(430, 160);
        gbSummonActions.ResumeLayout(false);
        tlp.ResumeLayout(false);
        tlp.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)nudAmount).EndInit();
        ResumeLayout(false);
    }
}
