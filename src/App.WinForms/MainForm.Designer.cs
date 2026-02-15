namespace App.WinForms;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null;
    private TableLayoutPanel tlpRoot;
    private TabControl tabMain;
    private TabPage tabPlayerchecker;
    private TabPage tabMonster;
    private TabPage tabItems;
    private TabPage tabSkills;
    private TabPage tabBuffs;
    private TabPage tabNpcs;
    private TabPage tabSummons;
    private TabPage tabCommandOverview;
    private Controls.EntityBrowserControl browserPlayerchecker;
    private Controls.EntityBrowserControl browserMonster;
    private Controls.EntityBrowserControl browserItems;
    private Controls.EntityBrowserControl browserSkills;
    private Controls.EntityBrowserControl browserBuffs;
    private Controls.EntityBrowserControl browserNpcs;
    private Controls.EntityBrowserControl browserSummons;
    private TableLayoutPanel tlpSidebar;
    private PictureBox picAvatar;
    private Label lblProviderCaption;
    private Label lblProviderValue;
    private Label lblPlayer;
    private ComboBox cmbPlayers;
    private Label lblNewPlayer;
    private TextBox txtNewPlayer;
    private FlowLayoutPanel flpPlayerButtons;
    private Button btnAddPlayer;
    private Button btnRemovePlayer;
    private CheckBox chkAppendCommands;
    private FontAwesome.Sharp.IconButton btnSettings;
    private TableLayoutPanel tlpCommandOverview;
    private ListBox lstCommands;
    private FlowLayoutPanel flpCommandButtons;
    private Button btnCopySelected;
    private Button btnCopyAll;
    private Button btnClearCommands;

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
        tlpRoot = new TableLayoutPanel();
        tabMain = new TabControl();
        tabPlayerchecker = new TabPage();
        browserPlayerchecker = new Controls.EntityBrowserControl();
        tabMonster = new TabPage();
        browserMonster = new Controls.EntityBrowserControl();
        tabItems = new TabPage();
        browserItems = new Controls.EntityBrowserControl();
        tabSkills = new TabPage();
        browserSkills = new Controls.EntityBrowserControl();
        tabBuffs = new TabPage();
        browserBuffs = new Controls.EntityBrowserControl();
        tabNpcs = new TabPage();
        browserNpcs = new Controls.EntityBrowserControl();
        tabSummons = new TabPage();
        browserSummons = new Controls.EntityBrowserControl();
        tabCommandOverview = new TabPage();
        tlpCommandOverview = new TableLayoutPanel();
        lstCommands = new ListBox();
        flpCommandButtons = new FlowLayoutPanel();
        btnCopySelected = new Button();
        btnCopyAll = new Button();
        btnClearCommands = new Button();
        tlpSidebar = new TableLayoutPanel();
        picAvatar = new PictureBox();
        lblProviderCaption = new Label();
        lblProviderValue = new Label();
        lblPlayer = new Label();
        cmbPlayers = new ComboBox();
        lblNewPlayer = new Label();
        txtNewPlayer = new TextBox();
        flpPlayerButtons = new FlowLayoutPanel();
        btnAddPlayer = new Button();
        btnRemovePlayer = new Button();
        chkAppendCommands = new CheckBox();
        btnSettings = new FontAwesome.Sharp.IconButton();
        tlpRoot.SuspendLayout();
        tabMain.SuspendLayout();
        tabPlayerchecker.SuspendLayout();
        tabMonster.SuspendLayout();
        tabItems.SuspendLayout();
        tabSkills.SuspendLayout();
        tabBuffs.SuspendLayout();
        tabNpcs.SuspendLayout();
        tabSummons.SuspendLayout();
        tabCommandOverview.SuspendLayout();
        tlpCommandOverview.SuspendLayout();
        flpCommandButtons.SuspendLayout();
        tlpSidebar.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)picAvatar).BeginInit();
        flpPlayerButtons.SuspendLayout();
        SuspendLayout();
        // 
        // tlpRoot
        // 
        tlpRoot.ColumnCount = 2;
        tlpRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 310F));
        tlpRoot.Controls.Add(tabMain, 0, 0);
        tlpRoot.Controls.Add(tlpSidebar, 1, 0);
        tlpRoot.Dock = DockStyle.Fill;
        tlpRoot.Location = new Point(0, 0);
        tlpRoot.Name = "tlpRoot";
        tlpRoot.RowCount = 1;
        tlpRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpRoot.Size = new Size(1380, 860);
        tlpRoot.TabIndex = 0;
        // 
        // tabMain
        // 
        tabMain.Controls.Add(tabPlayerchecker);
        tabMain.Controls.Add(tabMonster);
        tabMain.Controls.Add(tabItems);
        tabMain.Controls.Add(tabSkills);
        tabMain.Controls.Add(tabBuffs);
        tabMain.Controls.Add(tabNpcs);
        tabMain.Controls.Add(tabSummons);
        tabMain.Controls.Add(tabCommandOverview);
        tabMain.Dock = DockStyle.Fill;
        tabMain.Location = new Point(3, 3);
        tabMain.Name = "tabMain";
        tabMain.SelectedIndex = 0;
        tabMain.Size = new Size(1064, 854);
        tabMain.TabIndex = 0;
        // 
        // tabPlayerchecker
        // 
        tabPlayerchecker.Controls.Add(browserPlayerchecker);
        tabPlayerchecker.Location = new Point(4, 29);
        tabPlayerchecker.Name = "tabPlayerchecker";
        tabPlayerchecker.Padding = new Padding(3);
        tabPlayerchecker.Size = new Size(1056, 821);
        tabPlayerchecker.TabIndex = 0;
        tabPlayerchecker.Text = "Playerchecker";
        tabPlayerchecker.UseVisualStyleBackColor = true;
        // 
        // browserPlayerchecker
        // 
        browserPlayerchecker.Dock = DockStyle.Fill;
        browserPlayerchecker.Location = new Point(3, 3);
        browserPlayerchecker.Name = "browserPlayerchecker";
        browserPlayerchecker.Size = new Size(1050, 815);
        browserPlayerchecker.TabIndex = 0;
        // 
        // tabMonster
        // 
        tabMonster.Controls.Add(browserMonster);
        tabMonster.Location = new Point(4, 29);
        tabMonster.Name = "tabMonster";
        tabMonster.Padding = new Padding(3);
        tabMonster.Size = new Size(1056, 821);
        tabMonster.TabIndex = 1;
        tabMonster.Text = "Monster";
        tabMonster.UseVisualStyleBackColor = true;
        // 
        // browserMonster
        // 
        browserMonster.Dock = DockStyle.Fill;
        browserMonster.Location = new Point(3, 3);
        browserMonster.Name = "browserMonster";
        browserMonster.Size = new Size(1050, 815);
        browserMonster.TabIndex = 0;
        // 
        // tabItems
        // 
        tabItems.Controls.Add(browserItems);
        tabItems.Location = new Point(4, 29);
        tabItems.Name = "tabItems";
        tabItems.Padding = new Padding(3);
        tabItems.Size = new Size(1056, 821);
        tabItems.TabIndex = 2;
        tabItems.Text = "Items";
        tabItems.UseVisualStyleBackColor = true;
        // 
        // browserItems
        // 
        browserItems.Dock = DockStyle.Fill;
        browserItems.Location = new Point(3, 3);
        browserItems.Name = "browserItems";
        browserItems.Size = new Size(1050, 815);
        browserItems.TabIndex = 0;
        // 
        // tabSkills
        // 
        tabSkills.Controls.Add(browserSkills);
        tabSkills.Location = new Point(4, 29);
        tabSkills.Name = "tabSkills";
        tabSkills.Padding = new Padding(3);
        tabSkills.Size = new Size(1056, 821);
        tabSkills.TabIndex = 3;
        tabSkills.Text = "Skills";
        tabSkills.UseVisualStyleBackColor = true;
        // 
        // browserSkills
        // 
        browserSkills.Dock = DockStyle.Fill;
        browserSkills.Location = new Point(3, 3);
        browserSkills.Name = "browserSkills";
        browserSkills.Size = new Size(1050, 815);
        browserSkills.TabIndex = 0;
        // 
        // tabBuffs
        // 
        tabBuffs.Controls.Add(browserBuffs);
        tabBuffs.Location = new Point(4, 29);
        tabBuffs.Name = "tabBuffs";
        tabBuffs.Padding = new Padding(3);
        tabBuffs.Size = new Size(1056, 821);
        tabBuffs.TabIndex = 4;
        tabBuffs.Text = "Buffs";
        tabBuffs.UseVisualStyleBackColor = true;
        // 
        // browserBuffs
        // 
        browserBuffs.Dock = DockStyle.Fill;
        browserBuffs.Location = new Point(3, 3);
        browserBuffs.Name = "browserBuffs";
        browserBuffs.Size = new Size(1050, 815);
        browserBuffs.TabIndex = 0;
        // 
        // tabNpcs
        // 
        tabNpcs.Controls.Add(browserNpcs);
        tabNpcs.Location = new Point(4, 29);
        tabNpcs.Name = "tabNpcs";
        tabNpcs.Padding = new Padding(3);
        tabNpcs.Size = new Size(1056, 821);
        tabNpcs.TabIndex = 5;
        tabNpcs.Text = "NPCs";
        tabNpcs.UseVisualStyleBackColor = true;
        // 
        // browserNpcs
        // 
        browserNpcs.Dock = DockStyle.Fill;
        browserNpcs.Location = new Point(3, 3);
        browserNpcs.Name = "browserNpcs";
        browserNpcs.Size = new Size(1050, 815);
        browserNpcs.TabIndex = 0;
        // 
        // tabSummons
        // 
        tabSummons.Controls.Add(browserSummons);
        tabSummons.Location = new Point(4, 29);
        tabSummons.Name = "tabSummons";
        tabSummons.Padding = new Padding(3);
        tabSummons.Size = new Size(1056, 821);
        tabSummons.TabIndex = 6;
        tabSummons.Text = "Summons";
        tabSummons.UseVisualStyleBackColor = true;
        // 
        // browserSummons
        // 
        browserSummons.Dock = DockStyle.Fill;
        browserSummons.Location = new Point(3, 3);
        browserSummons.Name = "browserSummons";
        browserSummons.Size = new Size(1050, 815);
        browserSummons.TabIndex = 0;
        // 
        // tabCommandOverview
        // 
        tabCommandOverview.Controls.Add(tlpCommandOverview);
        tabCommandOverview.Location = new Point(4, 29);
        tabCommandOverview.Name = "tabCommandOverview";
        tabCommandOverview.Padding = new Padding(3);
        tabCommandOverview.Size = new Size(1056, 821);
        tabCommandOverview.TabIndex = 7;
        tabCommandOverview.Text = "Command Overview";
        tabCommandOverview.UseVisualStyleBackColor = true;
        // 
        // tlpCommandOverview
        // 
        tlpCommandOverview.ColumnCount = 1;
        tlpCommandOverview.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpCommandOverview.Controls.Add(lstCommands, 0, 0);
        tlpCommandOverview.Controls.Add(flpCommandButtons, 0, 1);
        tlpCommandOverview.Dock = DockStyle.Fill;
        tlpCommandOverview.Location = new Point(3, 3);
        tlpCommandOverview.Name = "tlpCommandOverview";
        tlpCommandOverview.RowCount = 2;
        tlpCommandOverview.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpCommandOverview.RowStyles.Add(new RowStyle());
        tlpCommandOverview.Size = new Size(1050, 815);
        tlpCommandOverview.TabIndex = 0;
        // 
        // lstCommands
        // 
        lstCommands.Dock = DockStyle.Fill;
        lstCommands.FormattingEnabled = true;
        lstCommands.Location = new Point(3, 3);
        lstCommands.Name = "lstCommands";
        lstCommands.Size = new Size(1044, 773);
        lstCommands.TabIndex = 0;
        // 
        // flpCommandButtons
        // 
        flpCommandButtons.AutoSize = true;
        flpCommandButtons.Controls.Add(btnCopySelected);
        flpCommandButtons.Controls.Add(btnCopyAll);
        flpCommandButtons.Controls.Add(btnClearCommands);
        flpCommandButtons.Dock = DockStyle.Right;
        flpCommandButtons.Location = new Point(693, 782);
        flpCommandButtons.Name = "flpCommandButtons";
        flpCommandButtons.Size = new Size(354, 30);
        flpCommandButtons.TabIndex = 1;
        // 
        // btnCopySelected
        // 
        btnCopySelected.Location = new Point(3, 3);
        btnCopySelected.Name = "btnCopySelected";
        btnCopySelected.Size = new Size(111, 24);
        btnCopySelected.TabIndex = 0;
        btnCopySelected.Text = "Copy selected";
        btnCopySelected.UseVisualStyleBackColor = true;
        btnCopySelected.Click += btnCopySelected_Click;
        // 
        // btnCopyAll
        // 
        btnCopyAll.Location = new Point(120, 3);
        btnCopyAll.Name = "btnCopyAll";
        btnCopyAll.Size = new Size(111, 24);
        btnCopyAll.TabIndex = 1;
        btnCopyAll.Text = "Copy all";
        btnCopyAll.UseVisualStyleBackColor = true;
        btnCopyAll.Click += btnCopyAll_Click;
        // 
        // btnClearCommands
        // 
        btnClearCommands.Location = new Point(237, 3);
        btnClearCommands.Name = "btnClearCommands";
        btnClearCommands.Size = new Size(114, 24);
        btnClearCommands.TabIndex = 2;
        btnClearCommands.Text = "Clear";
        btnClearCommands.UseVisualStyleBackColor = true;
        btnClearCommands.Click += btnClearCommands_Click;
        // 
        // tlpSidebar
        // 
        tlpSidebar.ColumnCount = 1;
        tlpSidebar.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpSidebar.Controls.Add(picAvatar, 0, 0);
        tlpSidebar.Controls.Add(lblProviderCaption, 0, 1);
        tlpSidebar.Controls.Add(lblProviderValue, 0, 2);
        tlpSidebar.Controls.Add(lblPlayer, 0, 3);
        tlpSidebar.Controls.Add(cmbPlayers, 0, 4);
        tlpSidebar.Controls.Add(lblNewPlayer, 0, 5);
        tlpSidebar.Controls.Add(txtNewPlayer, 0, 6);
        tlpSidebar.Controls.Add(flpPlayerButtons, 0, 7);
        tlpSidebar.Controls.Add(chkAppendCommands, 0, 8);
        tlpSidebar.Controls.Add(btnSettings, 0, 9);
        tlpSidebar.Dock = DockStyle.Fill;
        tlpSidebar.Location = new Point(1073, 3);
        tlpSidebar.Name = "tlpSidebar";
        tlpSidebar.Padding = new Padding(8);
        tlpSidebar.RowCount = 10;
        tlpSidebar.RowStyles.Add(new RowStyle());
        tlpSidebar.RowStyles.Add(new RowStyle());
        tlpSidebar.RowStyles.Add(new RowStyle());
        tlpSidebar.RowStyles.Add(new RowStyle());
        tlpSidebar.RowStyles.Add(new RowStyle());
        tlpSidebar.RowStyles.Add(new RowStyle());
        tlpSidebar.RowStyles.Add(new RowStyle());
        tlpSidebar.RowStyles.Add(new RowStyle());
        tlpSidebar.RowStyles.Add(new RowStyle());
        tlpSidebar.RowStyles.Add(new RowStyle());
        tlpSidebar.Size = new Size(304, 854);
        tlpSidebar.TabIndex = 1;
        // 
        // picAvatar
        // 
        picAvatar.Dock = DockStyle.Fill;
        picAvatar.Location = new Point(11, 11);
        picAvatar.Name = "picAvatar";
        picAvatar.Size = new Size(282, 140);
        picAvatar.SizeMode = PictureBoxSizeMode.Zoom;
        picAvatar.TabIndex = 0;
        picAvatar.TabStop = false;
        // 
        // lblProviderCaption
        // 
        lblProviderCaption.AutoSize = true;
        lblProviderCaption.Location = new Point(11, 154);
        lblProviderCaption.Name = "lblProviderCaption";
        lblProviderCaption.Padding = new Padding(0, 10, 0, 0);
        lblProviderCaption.Size = new Size(126, 30);
        lblProviderCaption.TabIndex = 1;
        lblProviderCaption.Text = "Database provider";
        // 
        // lblProviderValue
        // 
        lblProviderValue.AutoSize = true;
        lblProviderValue.Location = new Point(11, 184);
        lblProviderValue.Name = "lblProviderValue";
        lblProviderValue.Size = new Size(51, 20);
        lblProviderValue.TabIndex = 2;
        lblProviderValue.Text = "MSSQL";
        // 
        // lblPlayer
        // 
        lblPlayer.AutoSize = true;
        lblPlayer.Location = new Point(11, 204);
        lblPlayer.Name = "lblPlayer";
        lblPlayer.Padding = new Padding(0, 10, 0, 0);
        lblPlayer.Size = new Size(50, 30);
        lblPlayer.TabIndex = 3;
        lblPlayer.Text = "Player";
        // 
        // cmbPlayers
        // 
        cmbPlayers.Dock = DockStyle.Top;
        cmbPlayers.FormattingEnabled = true;
        cmbPlayers.Location = new Point(11, 237);
        cmbPlayers.Name = "cmbPlayers";
        cmbPlayers.Size = new Size(282, 28);
        cmbPlayers.TabIndex = 4;
        cmbPlayers.SelectedIndexChanged += cmbPlayers_SelectedIndexChanged;
        // 
        // lblNewPlayer
        // 
        lblNewPlayer.AutoSize = true;
        lblNewPlayer.Location = new Point(11, 268);
        lblNewPlayer.Name = "lblNewPlayer";
        lblNewPlayer.Padding = new Padding(0, 10, 0, 0);
        lblNewPlayer.Size = new Size(46, 30);
        lblNewPlayer.TabIndex = 5;
        lblNewPlayer.Text = "New:";
        // 
        // txtNewPlayer
        // 
        txtNewPlayer.Dock = DockStyle.Top;
        txtNewPlayer.Location = new Point(11, 301);
        txtNewPlayer.Name = "txtNewPlayer";
        txtNewPlayer.Size = new Size(282, 27);
        txtNewPlayer.TabIndex = 6;
        // 
        // flpPlayerButtons
        // 
        flpPlayerButtons.AutoSize = true;
        flpPlayerButtons.Controls.Add(btnAddPlayer);
        flpPlayerButtons.Controls.Add(btnRemovePlayer);
        flpPlayerButtons.Dock = DockStyle.Top;
        flpPlayerButtons.Location = new Point(11, 334);
        flpPlayerButtons.Name = "flpPlayerButtons";
        flpPlayerButtons.Size = new Size(282, 35);
        flpPlayerButtons.TabIndex = 7;
        // 
        // btnAddPlayer
        // 
        btnAddPlayer.Location = new Point(3, 3);
        btnAddPlayer.Name = "btnAddPlayer";
        btnAddPlayer.Size = new Size(70, 29);
        btnAddPlayer.TabIndex = 0;
        btnAddPlayer.Text = "+";
        btnAddPlayer.UseVisualStyleBackColor = true;
        btnAddPlayer.Click += btnAddPlayer_Click;
        // 
        // btnRemovePlayer
        // 
        btnRemovePlayer.Location = new Point(79, 3);
        btnRemovePlayer.Name = "btnRemovePlayer";
        btnRemovePlayer.Size = new Size(70, 29);
        btnRemovePlayer.TabIndex = 1;
        btnRemovePlayer.Text = "-";
        btnRemovePlayer.UseVisualStyleBackColor = true;
        btnRemovePlayer.Click += btnRemovePlayer_Click;
        // 
        // chkAppendCommands
        // 
        chkAppendCommands.AutoSize = true;
        chkAppendCommands.Checked = true;
        chkAppendCommands.CheckState = CheckState.Checked;
        chkAppendCommands.Location = new Point(11, 375);
        chkAppendCommands.Name = "chkAppendCommands";
        chkAppendCommands.Padding = new Padding(0, 10, 0, 0);
        chkAppendCommands.Size = new Size(222, 34);
        chkAppendCommands.TabIndex = 8;
        chkAppendCommands.Text = "Append / run to commands";
        chkAppendCommands.UseVisualStyleBackColor = true;
        chkAppendCommands.CheckedChanged += chkAppendCommands_CheckedChanged;
        // 
        // btnSettings
        // 
        btnSettings.Dock = DockStyle.Top;
        btnSettings.IconChar = FontAwesome.Sharp.IconChar.Gear;
        btnSettings.IconColor = Color.DimGray;
        btnSettings.IconFont = FontAwesome.Sharp.IconFont.Auto;
        btnSettings.IconSize = 20;
        btnSettings.ImageAlign = ContentAlignment.MiddleLeft;
        btnSettings.Location = new Point(11, 415);
        btnSettings.Name = "btnSettings";
        btnSettings.Size = new Size(282, 32);
        btnSettings.TabIndex = 9;
        btnSettings.Text = "Settings";
        btnSettings.TextAlign = ContentAlignment.MiddleRight;
        btnSettings.UseVisualStyleBackColor = true;
        btnSettings.Click += btnSettings_Click;
        // 
        // MainForm
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1380, 860);
        Controls.Add(tlpRoot);
        MinimumSize = new Size(1200, 700);
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "YSM GM Tool";
        Load += MainForm_Load;
        FormClosing += MainForm_FormClosing;
        tlpRoot.ResumeLayout(false);
        tabMain.ResumeLayout(false);
        tabPlayerchecker.ResumeLayout(false);
        tabMonster.ResumeLayout(false);
        tabItems.ResumeLayout(false);
        tabSkills.ResumeLayout(false);
        tabBuffs.ResumeLayout(false);
        tabNpcs.ResumeLayout(false);
        tabSummons.ResumeLayout(false);
        tabCommandOverview.ResumeLayout(false);
        tlpCommandOverview.ResumeLayout(false);
        tlpCommandOverview.PerformLayout();
        flpCommandButtons.ResumeLayout(false);
        tlpSidebar.ResumeLayout(false);
        tlpSidebar.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)picAvatar).EndInit();
        flpPlayerButtons.ResumeLayout(false);
        ResumeLayout(false);
    }
}
