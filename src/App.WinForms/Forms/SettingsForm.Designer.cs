namespace App.WinForms.Forms;

partial class SettingsForm
{
    private System.ComponentModel.IContainer components = null;
    private TableLayoutPanel tlp;
    private Label lblProvider;
    private ComboBox cmbProvider;
    private Label lblConnectionString;
    private TextBox txtConnectionString;
    private FlowLayoutPanel flpButtons;
    private Button btnTestConnection;
    private Button btnSave;
    private Button btnCancel;
    private Label lblStatus;

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
        tlp = new TableLayoutPanel();
        lblProvider = new Label();
        cmbProvider = new ComboBox();
        lblConnectionString = new Label();
        txtConnectionString = new TextBox();
        flpButtons = new FlowLayoutPanel();
        btnTestConnection = new Button();
        btnSave = new Button();
        btnCancel = new Button();
        lblStatus = new Label();
        tlp.SuspendLayout();
        flpButtons.SuspendLayout();
        SuspendLayout();
        // 
        // tlp
        // 
        tlp.ColumnCount = 2;
        tlp.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlp.Controls.Add(lblProvider, 0, 0);
        tlp.Controls.Add(cmbProvider, 1, 0);
        tlp.Controls.Add(lblConnectionString, 0, 1);
        tlp.Controls.Add(txtConnectionString, 1, 1);
        tlp.Controls.Add(flpButtons, 1, 2);
        tlp.Controls.Add(lblStatus, 1, 3);
        tlp.Dock = DockStyle.Fill;
        tlp.Location = new Point(12, 12);
        tlp.Name = "tlp";
        tlp.RowCount = 4;
        tlp.RowStyles.Add(new RowStyle());
        tlp.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlp.RowStyles.Add(new RowStyle());
        tlp.RowStyles.Add(new RowStyle());
        tlp.Size = new Size(760, 337);
        tlp.TabIndex = 0;
        // 
        // lblProvider
        // 
        lblProvider.Anchor = AnchorStyles.Left;
        lblProvider.AutoSize = true;
        lblProvider.Location = new Point(3, 9);
        lblProvider.Name = "lblProvider";
        lblProvider.Size = new Size(124, 20);
        lblProvider.TabIndex = 0;
        lblProvider.Text = "Database provider";
        // 
        // cmbProvider
        // 
        cmbProvider.Dock = DockStyle.Fill;
        cmbProvider.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbProvider.FormattingEnabled = true;
        cmbProvider.Location = new Point(133, 3);
        cmbProvider.Name = "cmbProvider";
        cmbProvider.Size = new Size(624, 28);
        cmbProvider.TabIndex = 1;
        // 
        // lblConnectionString
        // 
        lblConnectionString.Anchor = AnchorStyles.Left;
        lblConnectionString.AutoSize = true;
        lblConnectionString.Location = new Point(3, 60);
        lblConnectionString.Name = "lblConnectionString";
        lblConnectionString.Size = new Size(124, 20);
        lblConnectionString.TabIndex = 2;
        lblConnectionString.Text = "Connection string";
        // 
        // txtConnectionString
        // 
        txtConnectionString.Dock = DockStyle.Fill;
        txtConnectionString.Location = new Point(133, 37);
        txtConnectionString.Multiline = true;
        txtConnectionString.Name = "txtConnectionString";
        txtConnectionString.ScrollBars = ScrollBars.Vertical;
        txtConnectionString.Size = new Size(624, 232);
        txtConnectionString.TabIndex = 3;
        // 
        // flpButtons
        // 
        flpButtons.AutoSize = true;
        flpButtons.Controls.Add(btnTestConnection);
        flpButtons.Controls.Add(btnSave);
        flpButtons.Controls.Add(btnCancel);
        flpButtons.Dock = DockStyle.Right;
        flpButtons.FlowDirection = FlowDirection.RightToLeft;
        flpButtons.Location = new Point(414, 275);
        flpButtons.Name = "flpButtons";
        flpButtons.Size = new Size(343, 35);
        flpButtons.TabIndex = 4;
        // 
        // btnTestConnection
        // 
        btnTestConnection.Location = new Point(240, 3);
        btnTestConnection.Name = "btnTestConnection";
        btnTestConnection.Size = new Size(100, 29);
        btnTestConnection.TabIndex = 0;
        btnTestConnection.Text = "Test";
        btnTestConnection.UseVisualStyleBackColor = true;
        btnTestConnection.Click += btnTestConnection_Click;
        // 
        // btnSave
        // 
        btnSave.Location = new Point(134, 3);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(100, 29);
        btnSave.TabIndex = 1;
        btnSave.Text = "Save";
        btnSave.UseVisualStyleBackColor = true;
        btnSave.Click += btnSave_Click;
        // 
        // btnCancel
        // 
        btnCancel.Location = new Point(28, 3);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(100, 29);
        btnCancel.TabIndex = 2;
        btnCancel.Text = "Cancel";
        btnCancel.UseVisualStyleBackColor = true;
        btnCancel.Click += btnCancel_Click;
        // 
        // lblStatus
        // 
        lblStatus.AutoSize = true;
        lblStatus.Dock = DockStyle.Fill;
        lblStatus.Location = new Point(133, 313);
        lblStatus.Name = "lblStatus";
        lblStatus.Padding = new Padding(0, 8, 0, 0);
        lblStatus.Size = new Size(624, 24);
        lblStatus.TabIndex = 5;
        lblStatus.Text = "";
        // 
        // SettingsForm
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(784, 361);
        Controls.Add(tlp);
        MinimumSize = new Size(700, 350);
        Name = "SettingsForm";
        Padding = new Padding(12);
        StartPosition = FormStartPosition.CenterParent;
        Text = "Settings";
        tlp.ResumeLayout(false);
        tlp.PerformLayout();
        flpButtons.ResumeLayout(false);
        ResumeLayout(false);
    }
}
