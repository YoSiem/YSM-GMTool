namespace App.WinForms.Forms;

partial class AboutForm
{
    private System.ComponentModel.IContainer components = null;
    private TableLayoutPanel tlp;
    private PictureBox picIcon;
    private Label lblTitle;
    private Label lblAuthor;
    private Label lblVibe;
    private Label lblPS;
    private Button btnOk;

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
        picIcon = new PictureBox();
        lblTitle = new Label();
        lblAuthor = new Label();
        lblVibe = new Label();
        lblPS = new Label();
        btnOk = new Button();
        tlp.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)picIcon).BeginInit();
        SuspendLayout();
        // 
        // tlp
        // 
        tlp.ColumnCount = 1;
        tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlp.Controls.Add(picIcon, 0, 0);
        tlp.Controls.Add(lblTitle, 0, 1);
        tlp.Controls.Add(lblAuthor, 0, 2);
        tlp.Controls.Add(lblVibe, 0, 3);
        tlp.Controls.Add(lblPS, 0, 4);
        tlp.Controls.Add(btnOk, 0, 5);
        tlp.Dock = DockStyle.Fill;
        tlp.Location = new Point(12, 12);
        tlp.Name = "tlp";
        tlp.RowCount = 6;
        tlp.RowStyles.Add(new RowStyle());
        tlp.RowStyles.Add(new RowStyle());
        tlp.RowStyles.Add(new RowStyle());
        tlp.RowStyles.Add(new RowStyle());
        tlp.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlp.RowStyles.Add(new RowStyle());
        tlp.Size = new Size(460, 420);
        tlp.TabIndex = 0;
        //
        // picIcon
        //
        picIcon.Anchor = AnchorStyles.Top;
        picIcon.Location = new Point(198, 3);
        picIcon.Name = "picIcon";
        picIcon.Size = new Size(64, 64);
        picIcon.SizeMode = PictureBoxSizeMode.Zoom;
        picIcon.TabIndex = 3;
        picIcon.TabStop = false;
        // 
        // lblTitle
        // 
        lblTitle.AutoSize = true;
        lblTitle.Dock = DockStyle.Fill;
        lblTitle.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 238);
        lblTitle.Location = new Point(3, 70);
        lblTitle.Name = "lblTitle";
        lblTitle.Padding = new Padding(0, 4, 0, 4);
        lblTitle.Size = new Size(454, 36);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "GM Tool";
        lblTitle.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblAuthor
        // 
        lblAuthor.AutoSize = true;
        lblAuthor.Dock = DockStyle.Fill;
        lblAuthor.Location = new Point(3, 106);
        lblAuthor.Name = "lblAuthor";
        lblAuthor.Padding = new Padding(0, 4, 0, 0);
        lblAuthor.Size = new Size(454, 24);
        lblAuthor.TabIndex = 1;
        lblAuthor.Text = "Created by YoSiem.";
        lblAuthor.TextAlign = ContentAlignment.MiddleCenter;
        //
        // lblVibe
        //
        lblVibe.AutoSize = true;
        lblVibe.Dock = DockStyle.Fill;
        lblVibe.Location = new Point(3, 130);
        lblVibe.Name = "lblVibe";
        lblVibe.Padding = new Padding(0, 0, 0, 8);
        lblVibe.Size = new Size(454, 28);
        lblVibe.TabIndex = 4;
        lblVibe.Text = "Vibecoded in one day";
        lblVibe.TextAlign = ContentAlignment.MiddleCenter;
        //
        // lblPS
        //
        lblPS.AutoSize = true;
        lblPS.Dock = DockStyle.Fill;
        lblPS.Location = new Point(3, 161);
        lblPS.Name = "lblPS";
        lblPS.Padding = new Padding(0, 8, 0, 8);
        lblPS.Size = new Size(454, 210);
        lblPS.TabIndex = 5;
        lblPS.Text = "PS:\r\n\r\nIf creating the best applications in this community takes less than a single day with a basic AI agent, it is honestly disappointing that no one else is doing it.";
        lblPS.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // btnOk
        // 
        btnOk.Anchor = AnchorStyles.Right;
        btnOk.DialogResult = DialogResult.OK;
        btnOk.Location = new Point(357, 381);
        btnOk.Name = "btnOk";
        btnOk.Size = new Size(100, 29);
        btnOk.TabIndex = 2;
        btnOk.Text = "OK";
        btnOk.UseVisualStyleBackColor = true;
        // 
        // AboutForm
        // 
        AcceptButton = btnOk;
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(484, 444);
        Controls.Add(tlp);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "AboutForm";
        Padding = new Padding(12);
        StartPosition = FormStartPosition.CenterParent;
        Text = "About";
        tlp.ResumeLayout(false);
        tlp.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)picIcon).EndInit();
        ResumeLayout(false);
    }
}
