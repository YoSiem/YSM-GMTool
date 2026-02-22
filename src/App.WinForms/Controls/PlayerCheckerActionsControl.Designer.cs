namespace App.WinForms.Controls;

partial class PlayerCheckerActionsControl
{
    private System.ComponentModel.IContainer components = null;
    private GroupBox gbPlayerChecker;
    private TableLayoutPanel tlp;
    private Button btnLoadInventory;
    private Button btnLoadWh;
    private Button btnOpenInfos;
    private Button btnLoadAllCharacters;
    private Button btnLoadOnlineCharacters;

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
        btnLoadAllCharacters = new Button();
        btnLoadOnlineCharacters = new Button();
        btnLoadInventory = new Button();
        btnLoadWh = new Button();
        btnOpenInfos = new Button();
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
        gbPlayerChecker.Size = new Size(360, 100);
        gbPlayerChecker.TabIndex = 0;
        gbPlayerChecker.TabStop = false;
        gbPlayerChecker.Text = "Playerchecker Actions";
        //
        // tlp — 6 equal columns, 2 fixed-height rows
        //
        tlp.ColumnCount = 6;
        tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.67F));
        tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.67F));
        tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.67F));
        tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.67F));
        tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.67F));
        tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.65F));
        // Row 0: Load All (cols 0-2) | Load Online (cols 3-5)
        tlp.Controls.Add(btnLoadAllCharacters, 0, 0);
        tlp.SetColumnSpan(btnLoadAllCharacters, 3);
        tlp.Controls.Add(btnLoadOnlineCharacters, 3, 0);
        tlp.SetColumnSpan(btnLoadOnlineCharacters, 3);
        // Row 1: Load Inventory (cols 0-1) | Load WH (cols 2-3) | Open Infos (cols 4-5)
        tlp.Controls.Add(btnLoadInventory, 0, 1);
        tlp.SetColumnSpan(btnLoadInventory, 2);
        tlp.Controls.Add(btnLoadWh, 2, 1);
        tlp.SetColumnSpan(btnLoadWh, 2);
        tlp.Controls.Add(btnOpenInfos, 4, 1);
        tlp.SetColumnSpan(btnOpenInfos, 2);
        tlp.Dock = DockStyle.Fill;
        tlp.Location = new Point(3, 23);
        tlp.Name = "tlp";
        tlp.RowCount = 2;
        tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
        tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
        tlp.Size = new Size(354, 70);
        tlp.TabIndex = 0;
        //
        // btnLoadAllCharacters
        //
        btnLoadAllCharacters.Dock = DockStyle.Fill;
        btnLoadAllCharacters.Location = new Point(3, 3);
        btnLoadAllCharacters.Name = "btnLoadAllCharacters";
        btnLoadAllCharacters.Size = new Size(171, 29);
        btnLoadAllCharacters.TabIndex = 0;
        btnLoadAllCharacters.Text = "Load All Characters";
        btnLoadAllCharacters.UseVisualStyleBackColor = true;
        btnLoadAllCharacters.Click += btnLoadAllCharacters_Click;
        //
        // btnLoadOnlineCharacters
        //
        btnLoadOnlineCharacters.Dock = DockStyle.Fill;
        btnLoadOnlineCharacters.Location = new Point(177, 3);
        btnLoadOnlineCharacters.Name = "btnLoadOnlineCharacters";
        btnLoadOnlineCharacters.Size = new Size(171, 29);
        btnLoadOnlineCharacters.TabIndex = 1;
        btnLoadOnlineCharacters.Text = "Load Online Characters";
        btnLoadOnlineCharacters.UseVisualStyleBackColor = true;
        btnLoadOnlineCharacters.Click += btnLoadOnlineCharacters_Click;
        //
        // btnLoadInventory
        //
        btnLoadInventory.Dock = DockStyle.Fill;
        btnLoadInventory.Location = new Point(3, 38);
        btnLoadInventory.Name = "btnLoadInventory";
        btnLoadInventory.Size = new Size(112, 29);
        btnLoadInventory.TabIndex = 2;
        btnLoadInventory.Text = "Load Inventory";
        btnLoadInventory.UseVisualStyleBackColor = true;
        btnLoadInventory.Click += btnLoadInventory_Click;
        //
        // btnLoadWh
        //
        btnLoadWh.Dock = DockStyle.Fill;
        btnLoadWh.Location = new Point(120, 38);
        btnLoadWh.Name = "btnLoadWh";
        btnLoadWh.Size = new Size(112, 29);
        btnLoadWh.TabIndex = 3;
        btnLoadWh.Text = "Load WH";
        btnLoadWh.UseVisualStyleBackColor = true;
        btnLoadWh.Click += btnLoadWh_Click;
        //
        // btnOpenInfos
        //
        btnOpenInfos.Dock = DockStyle.Fill;
        btnOpenInfos.Location = new Point(237, 38);
        btnOpenInfos.Name = "btnOpenInfos";
        btnOpenInfos.Size = new Size(114, 29);
        btnOpenInfos.TabIndex = 4;
        btnOpenInfos.Text = "Open Infos";
        btnOpenInfos.UseVisualStyleBackColor = true;
        btnOpenInfos.Click += btnOpenInfos_Click;
        //
        // PlayerCheckerActionsControl
        //
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(gbPlayerChecker);
        Name = "PlayerCheckerActionsControl";
        Size = new Size(360, 100);
        gbPlayerChecker.ResumeLayout(false);
        tlp.ResumeLayout(false);
        ResumeLayout(false);
    }
}
