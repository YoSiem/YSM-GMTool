namespace App.WinForms.Controls;

partial class PlayerCheckerActionsControl
{
    private System.ComponentModel.IContainer components = null;
    private GroupBox gbPlayerChecker;
    private TableLayoutPanel tlp;
    private TableLayoutPanel tlpActionButtons;
    private TableLayoutPanel tlpLoadButtons;
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
        tlpLoadButtons = new TableLayoutPanel();
        btnLoadAllCharacters = new Button();
        btnLoadOnlineCharacters = new Button();
        tlpActionButtons = new TableLayoutPanel();
        btnLoadInventory = new Button();
        btnLoadWh = new Button();
        btnOpenInfos = new Button();
        gbPlayerChecker.SuspendLayout();
        tlp.SuspendLayout();
        tlpLoadButtons.SuspendLayout();
        tlpActionButtons.SuspendLayout();
        SuspendLayout();
        //
        // gbPlayerChecker
        //
        gbPlayerChecker.Controls.Add(tlp);
        gbPlayerChecker.Dock = DockStyle.Fill;
        gbPlayerChecker.Location = new Point(0, 0);
        gbPlayerChecker.Name = "gbPlayerChecker";
        gbPlayerChecker.Size = new Size(360, 120);
        gbPlayerChecker.TabIndex = 0;
        gbPlayerChecker.TabStop = false;
        gbPlayerChecker.Text = "Playerchecker Actions";
        //
        // tlp
        //
        tlp.ColumnCount = 1;
        tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlp.Controls.Add(tlpLoadButtons, 0, 0);
        tlp.Controls.Add(tlpActionButtons, 0, 1);
        tlp.Dock = DockStyle.Fill;
        tlp.Location = new Point(3, 23);
        tlp.Name = "tlp";
        tlp.RowCount = 2;
        tlp.RowStyles.Add(new RowStyle());
        tlp.RowStyles.Add(new RowStyle());
        tlp.Size = new Size(354, 94);
        tlp.TabIndex = 0;
        //
        // tlpLoadButtons
        //
        tlpLoadButtons.ColumnCount = 2;
        tlpLoadButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpLoadButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tlpLoadButtons.Controls.Add(btnLoadAllCharacters, 0, 0);
        tlpLoadButtons.Controls.Add(btnLoadOnlineCharacters, 1, 0);
        tlpLoadButtons.Dock = DockStyle.Fill;
        tlpLoadButtons.Location = new Point(3, 3);
        tlpLoadButtons.Name = "tlpLoadButtons";
        tlpLoadButtons.RowCount = 1;
        tlpLoadButtons.RowStyles.Add(new RowStyle());
        tlpLoadButtons.Size = new Size(348, 36);
        tlpLoadButtons.TabIndex = 1;
        //
        // btnLoadAllCharacters
        //
        btnLoadAllCharacters.Dock = DockStyle.Top;
        btnLoadAllCharacters.Location = new Point(3, 3);
        btnLoadAllCharacters.Name = "btnLoadAllCharacters";
        btnLoadAllCharacters.Size = new Size(168, 36);
        btnLoadAllCharacters.TabIndex = 0;
        btnLoadAllCharacters.Text = "Load All Characters";
        btnLoadAllCharacters.UseVisualStyleBackColor = true;
        btnLoadAllCharacters.Click += btnLoadAllCharacters_Click;
        //
        // btnLoadOnlineCharacters
        //
        btnLoadOnlineCharacters.Dock = DockStyle.Top;
        btnLoadOnlineCharacters.Location = new Point(177, 3);
        btnLoadOnlineCharacters.Name = "btnLoadOnlineCharacters";
        btnLoadOnlineCharacters.Size = new Size(168, 36);
        btnLoadOnlineCharacters.TabIndex = 1;
        btnLoadOnlineCharacters.Text = "Load Online Characters";
        btnLoadOnlineCharacters.UseVisualStyleBackColor = true;
        btnLoadOnlineCharacters.Click += btnLoadOnlineCharacters_Click;
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
        tlpActionButtons.Location = new Point(3, 44);
        tlpActionButtons.Name = "tlpActionButtons";
        tlpActionButtons.RowCount = 1;
        tlpActionButtons.RowStyles.Add(new RowStyle());
        tlpActionButtons.Size = new Size(348, 36);
        tlpActionButtons.TabIndex = 0;
        //
        // btnLoadInventory
        //
        btnLoadInventory.Dock = DockStyle.Top;
        btnLoadInventory.Location = new Point(3, 3);
        btnLoadInventory.Name = "btnLoadInventory";
        btnLoadInventory.Size = new Size(109, 36);
        btnLoadInventory.TabIndex = 0;
        btnLoadInventory.Text = "Load inventory";
        btnLoadInventory.UseVisualStyleBackColor = true;
        btnLoadInventory.Click += btnLoadInventory_Click;
        //
        // btnLoadWh
        //
        btnLoadWh.Dock = DockStyle.Top;
        btnLoadWh.Location = new Point(118, 3);
        btnLoadWh.Name = "btnLoadWh";
        btnLoadWh.Size = new Size(109, 36);
        btnLoadWh.TabIndex = 1;
        btnLoadWh.Text = "Load WH";
        btnLoadWh.UseVisualStyleBackColor = true;
        btnLoadWh.Click += btnLoadWh_Click;
        //
        // btnOpenInfos
        //
        btnOpenInfos.Dock = DockStyle.Top;
        btnOpenInfos.Location = new Point(233, 3);
        btnOpenInfos.Name = "btnOpenInfos";
        btnOpenInfos.Size = new Size(112, 36);
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
        Size = new Size(360, 120);
        AutoSize = true;
        AutoSizeMode = AutoSizeMode.GrowAndShrink;
        gbPlayerChecker.ResumeLayout(false);
        tlp.ResumeLayout(false);
        tlp.PerformLayout();
        tlpLoadButtons.ResumeLayout(false);
        tlpActionButtons.ResumeLayout(false);
        ResumeLayout(false);
    }
}
