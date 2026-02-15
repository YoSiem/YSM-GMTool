namespace App.WinForms.Controls;

partial class PlayerCheckerActionsControl
{
    private System.ComponentModel.IContainer components = null;
    private GroupBox gbPlayerChecker;
    private TableLayoutPanel tlp;
    private Label lblPlayerName;
    private TextBox txtPlayerName;
    private Button btnCreateCheckCommand;

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
        gbPlayerChecker = new GroupBox();
        tlp = new TableLayoutPanel();
        lblPlayerName = new Label();
        txtPlayerName = new TextBox();
        btnCreateCheckCommand = new Button();
        gbPlayerChecker.SuspendLayout();
        tlp.SuspendLayout();
        SuspendLayout();
        // 
        // gbPlayerChecker
        // 
        gbPlayerChecker.Controls.Add(tlp);
        gbPlayerChecker.Dock = DockStyle.Fill;
        gbPlayerChecker.Location = new Point(0, 0);
        gbPlayerChecker.Name = "gbPlayerChecker";
        gbPlayerChecker.Size = new Size(430, 160);
        gbPlayerChecker.TabIndex = 0;
        gbPlayerChecker.TabStop = false;
        gbPlayerChecker.Text = "Playerchecker Actions";
        // 
        // tlp
        // 
        tlp.ColumnCount = 2;
        tlp.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlp.Controls.Add(lblPlayerName, 0, 0);
        tlp.Controls.Add(txtPlayerName, 1, 0);
        tlp.Controls.Add(btnCreateCheckCommand, 0, 1);
        tlp.Dock = DockStyle.Fill;
        tlp.Location = new Point(3, 23);
        tlp.Name = "tlp";
        tlp.RowCount = 2;
        tlp.RowStyles.Add(new RowStyle());
        tlp.RowStyles.Add(new RowStyle());
        tlp.Size = new Size(424, 134);
        tlp.TabIndex = 0;
        // 
        // lblPlayerName
        // 
        lblPlayerName.Anchor = AnchorStyles.Left;
        lblPlayerName.AutoSize = true;
        lblPlayerName.Location = new Point(3, 9);
        lblPlayerName.Name = "lblPlayerName";
        lblPlayerName.Size = new Size(91, 20);
        lblPlayerName.TabIndex = 0;
        lblPlayerName.Text = "Player name";
        // 
        // txtPlayerName
        // 
        txtPlayerName.Dock = DockStyle.Fill;
        txtPlayerName.Location = new Point(100, 3);
        txtPlayerName.Name = "txtPlayerName";
        txtPlayerName.PlaceholderText = "optional, fallback to selected row";
        txtPlayerName.Size = new Size(321, 27);
        txtPlayerName.TabIndex = 1;
        // 
        // btnCreateCheckCommand
        // 
        tlp.SetColumnSpan(btnCreateCheckCommand, 2);
        btnCreateCheckCommand.Dock = DockStyle.Fill;
        btnCreateCheckCommand.Location = new Point(3, 36);
        btnCreateCheckCommand.Name = "btnCreateCheckCommand";
        btnCreateCheckCommand.Size = new Size(418, 29);
        btnCreateCheckCommand.TabIndex = 2;
        btnCreateCheckCommand.Text = "Create check command";
        btnCreateCheckCommand.UseVisualStyleBackColor = true;
        btnCreateCheckCommand.Click += btnCreateCheckCommand_Click;
        // 
        // PlayerCheckerActionsControl
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(gbPlayerChecker);
        Name = "PlayerCheckerActionsControl";
        Size = new Size(430, 160);
        gbPlayerChecker.ResumeLayout(false);
        tlp.ResumeLayout(false);
        tlp.PerformLayout();
        ResumeLayout(false);
    }
}
