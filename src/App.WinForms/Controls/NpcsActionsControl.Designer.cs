namespace App.WinForms.Controls;

partial class NpcsActionsControl
{
    private System.ComponentModel.IContainer components = null;
    private GroupBox gbNpcActions;
    private TableLayoutPanel tlp;
    private Label lblLayer;
    private NumericUpDown nudLayer;
    private Button btnCreateMoveCommand;

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
        gbNpcActions = new GroupBox();
        tlp = new TableLayoutPanel();
        lblLayer = new Label();
        nudLayer = new NumericUpDown();
        btnCreateMoveCommand = new Button();
        gbNpcActions.SuspendLayout();
        tlp.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudLayer).BeginInit();
        SuspendLayout();
        // 
        // gbNpcActions
        // 
        gbNpcActions.Controls.Add(tlp);
        gbNpcActions.Dock = DockStyle.Fill;
        gbNpcActions.Location = new Point(0, 0);
        gbNpcActions.Name = "gbNpcActions";
        gbNpcActions.Size = new Size(430, 160);
        gbNpcActions.TabIndex = 0;
        gbNpcActions.TabStop = false;
        gbNpcActions.Text = "NPC Commands";
        // 
        // tlp
        // 
        tlp.ColumnCount = 2;
        tlp.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlp.Controls.Add(lblLayer, 0, 0);
        tlp.Controls.Add(nudLayer, 1, 0);
        tlp.Controls.Add(btnCreateMoveCommand, 0, 1);
        tlp.Dock = DockStyle.Fill;
        tlp.Location = new Point(3, 23);
        tlp.Name = "tlp";
        tlp.RowCount = 2;
        tlp.RowStyles.Add(new RowStyle());
        tlp.RowStyles.Add(new RowStyle());
        tlp.Size = new Size(424, 134);
        tlp.TabIndex = 0;
        // 
        // lblLayer
        // 
        lblLayer.Anchor = AnchorStyles.Left;
        lblLayer.AutoSize = true;
        lblLayer.Location = new Point(3, 9);
        lblLayer.Name = "lblLayer";
        lblLayer.Size = new Size(44, 20);
        lblLayer.TabIndex = 0;
        lblLayer.Text = "Layer";
        // 
        // nudLayer
        // 
        nudLayer.Dock = DockStyle.Fill;
        nudLayer.Location = new Point(53, 3);
        nudLayer.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
        nudLayer.Minimum = new decimal(new int[] { 20, 0, 0, int.MinValue });
        nudLayer.Name = "nudLayer";
        nudLayer.Size = new Size(368, 27);
        nudLayer.TabIndex = 1;
        // 
        // btnCreateMoveCommand
        // 
        tlp.SetColumnSpan(btnCreateMoveCommand, 2);
        btnCreateMoveCommand.Dock = DockStyle.Fill;
        btnCreateMoveCommand.Location = new Point(3, 36);
        btnCreateMoveCommand.Name = "btnCreateMoveCommand";
        btnCreateMoveCommand.Size = new Size(418, 29);
        btnCreateMoveCommand.TabIndex = 2;
        btnCreateMoveCommand.Text = "Create move-to-npc command";
        btnCreateMoveCommand.UseVisualStyleBackColor = true;
        btnCreateMoveCommand.Click += btnCreateMoveCommand_Click;
        // 
        // NpcsActionsControl
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(gbNpcActions);
        Name = "NpcsActionsControl";
        Size = new Size(430, 160);
        gbNpcActions.ResumeLayout(false);
        tlp.ResumeLayout(false);
        tlp.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)nudLayer).EndInit();
        ResumeLayout(false);
    }
}
