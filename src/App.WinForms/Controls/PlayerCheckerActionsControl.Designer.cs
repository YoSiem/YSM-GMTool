namespace App.WinForms.Controls;

partial class PlayerCheckerActionsControl
{
    private System.ComponentModel.IContainer components = null;
    private GroupBox gbPlayerChecker;
    private TableLayoutPanel tlp;
    private TableLayoutPanel tlpActionButtons;
    private Button btnLoadInventory;
    private Button btnLoadWh;
    private Button btnOpenInfos;

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
        tlpActionButtons = new TableLayoutPanel();
        btnLoadInventory = new Button();
        btnLoadWh = new Button();
        btnOpenInfos = new Button();
        gbPlayerChecker.SuspendLayout();
        tlp.SuspendLayout();
        tlpActionButtons.SuspendLayout();
        SuspendLayout();
        //
        // gbPlayerChecker
        //
        gbPlayerChecker.Controls.Add(tlp);
        gbPlayerChecker.Dock = DockStyle.Fill;
        gbPlayerChecker.Location = new Point(0, 0);
        gbPlayerChecker.Name = "gbPlayerChecker";
        gbPlayerChecker.Size = new Size(360, 80);
        gbPlayerChecker.TabIndex = 0;
        gbPlayerChecker.TabStop = false;
        gbPlayerChecker.Text = "Playerchecker Actions";
        //
        // tlp
        //
        tlp.ColumnCount = 1;
        tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlp.Controls.Add(tlpActionButtons, 0, 0);
        tlp.Dock = DockStyle.Fill;
        tlp.Location = new Point(3, 23);
        tlp.Name = "tlp";
        tlp.RowCount = 1;
        tlp.RowStyles.Add(new RowStyle());
        tlp.Size = new Size(354, 54);
        tlp.TabIndex = 0;
        //
        // tlpActionButtons
        //
        tlpActionButtons.ColumnCount = 3;
        tlpActionButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3F));
        tlpActionButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3F));
        tlpActionButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.4F));
        tlpActionButtons.Controls.Add(btnLoadInventory, 0, 0);
        tlpActionButtons.Controls.Add(btnLoadWh, 1, 0);
        tlpActionButtons.Controls.Add(btnOpenInfos, 2, 0);
        tlpActionButtons.Dock = DockStyle.Fill;
        tlpActionButtons.Location = new Point(3, 3);
        tlpActionButtons.Name = "tlpActionButtons";
        tlpActionButtons.RowCount = 1;
        tlpActionButtons.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        tlpActionButtons.Size = new Size(348, 35);
        tlpActionButtons.TabIndex = 0;
        //
        // btnLoadInventory
        //
        btnLoadInventory.Dock = DockStyle.Fill;
        btnLoadInventory.Location = new Point(3, 3);
        btnLoadInventory.Name = "btnLoadInventory";
        btnLoadInventory.Size = new Size(109, 29);
        btnLoadInventory.TabIndex = 0;
        btnLoadInventory.Text = "Load inventory";
        btnLoadInventory.UseVisualStyleBackColor = true;
        btnLoadInventory.Click += btnLoadInventory_Click;
        //
        // btnLoadWh
        //
        btnLoadWh.Dock = DockStyle.Fill;
        btnLoadWh.Location = new Point(118, 3);
        btnLoadWh.Name = "btnLoadWh";
        btnLoadWh.Size = new Size(109, 29);
        btnLoadWh.TabIndex = 1;
        btnLoadWh.Text = "Load WH";
        btnLoadWh.UseVisualStyleBackColor = true;
        btnLoadWh.Click += btnLoadWh_Click;
        //
        // btnOpenInfos
        //
        btnOpenInfos.Dock = DockStyle.Fill;
        btnOpenInfos.Location = new Point(233, 3);
        btnOpenInfos.Name = "btnOpenInfos";
        btnOpenInfos.Size = new Size(112, 29);
        btnOpenInfos.TabIndex = 2;
        btnOpenInfos.Text = "Open infos";
        btnOpenInfos.UseVisualStyleBackColor = true;
        btnOpenInfos.Click += btnOpenInfos_Click;
        //
        // PlayerCheckerActionsControl
        //
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(gbPlayerChecker);
        Name = "PlayerCheckerActionsControl";
        Size = new Size(360, 80);
        gbPlayerChecker.ResumeLayout(false);
        tlp.ResumeLayout(false);
        tlp.PerformLayout();
        tlpActionButtons.ResumeLayout(false);
        ResumeLayout(false);
    }
}
