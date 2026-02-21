namespace App.WinForms.Forms;

partial class SettingsForm
{
    private System.ComponentModel.IContainer components = null;
    private TableLayoutPanel tlpRoot;
    private TabControl tabSettings;
    private TabPage tabConnection;
    private TabPage tabTables;
    private TableLayoutPanel tlpConnection;
    private Label lblProvider;
    private ComboBox cmbProvider;
    private Label lblServer;
    private TextBox txtServer;
    private Label lblPort;
    private NumericUpDown nudPort;
    private Label lblDatabase;
    private TextBox txtDatabase;
    private Label lblUserId;
    private TextBox txtUserId;
    private Label lblPassword;
    private TextBox txtPassword;
    private CheckBox chkIntegratedSecurity;
    private CheckBox chkEncrypt;
    private CheckBox chkTrustServerCertificate;
    private TableLayoutPanel tlpTables;
    private Label lblArcadiaName;
    private TextBox txtArcadiaName;
    private Label lblTelecasterName;
    private TextBox txtTelecasterName;
    private Label lblAuthName;
    private TextBox txtAuthName;
    private Label lblAccountsName;
    private TextBox txtAccountsName;
    private Label lblCharacterResource;
    private TextBox txtCharacterResource;
    private Label lblMonsterResource;
    private TextBox txtMonsterResource;
    private Label lblStringResource;
    private TextBox txtStringResource;
    private Label lblItemResource;
    private TextBox txtItemResource;
    private Label lblSkillResource;
    private TextBox txtSkillResource;
    private Label lblStateResource;
    private TextBox txtStateResource;
    private Label lblNpcResource;
    private TextBox txtNpcResource;
    private Label lblSummonResource;
    private TextBox txtSummonResource;
    private Label lblStatus;
    private FlowLayoutPanel flpButtons;
    private Button btnTestConnection;
    private Button btnSave;
    private Button btnCancel;
    private TabPage tabGeneral;
    private TableLayoutPanel tlpGeneral;
    private CheckBox chkLimitSelectQueries;

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
        tabSettings = new TabControl();
        tabConnection = new TabPage();
        tlpConnection = new TableLayoutPanel();
        lblProvider = new Label();
        cmbProvider = new ComboBox();
        lblServer = new Label();
        txtServer = new TextBox();
        lblPort = new Label();
        nudPort = new NumericUpDown();
        lblDatabase = new Label();
        txtDatabase = new TextBox();
        lblUserId = new Label();
        txtUserId = new TextBox();
        lblPassword = new Label();
        txtPassword = new TextBox();
        chkIntegratedSecurity = new CheckBox();
        chkEncrypt = new CheckBox();
        chkTrustServerCertificate = new CheckBox();
        tabGeneral = new TabPage();
        tlpGeneral = new TableLayoutPanel();
        chkLimitSelectQueries = new CheckBox();
        tabTables = new TabPage();
        tlpTables = new TableLayoutPanel();
        lblArcadiaName = new Label();
        txtArcadiaName = new TextBox();
        lblTelecasterName = new Label();
        txtTelecasterName = new TextBox();
        lblAuthName = new Label();
        txtAuthName = new TextBox();
        lblAccountsName = new Label();
        txtAccountsName = new TextBox();
        lblCharacterResource = new Label();
        txtCharacterResource = new TextBox();
        lblMonsterResource = new Label();
        txtMonsterResource = new TextBox();
        lblStringResource = new Label();
        txtStringResource = new TextBox();
        lblItemResource = new Label();
        txtItemResource = new TextBox();
        lblSkillResource = new Label();
        txtSkillResource = new TextBox();
        lblStateResource = new Label();
        txtStateResource = new TextBox();
        lblNpcResource = new Label();
        txtNpcResource = new TextBox();
        lblSummonResource = new Label();
        txtSummonResource = new TextBox();
        lblStatus = new Label();
        flpButtons = new FlowLayoutPanel();
        btnTestConnection = new Button();
        btnSave = new Button();
        btnCancel = new Button();
        tlpRoot.SuspendLayout();
        tabSettings.SuspendLayout();
        tabConnection.SuspendLayout();
        tlpConnection.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudPort).BeginInit();
        tabTables.SuspendLayout();
        tabGeneral.SuspendLayout();
        tlpGeneral.SuspendLayout();
        tlpTables.SuspendLayout();
        flpButtons.SuspendLayout();
        SuspendLayout();
        // 
        // tlpRoot
        // 
        tlpRoot.ColumnCount = 1;
        tlpRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpRoot.Controls.Add(tabSettings, 0, 0);
        tlpRoot.Controls.Add(lblStatus, 0, 1);
        tlpRoot.Controls.Add(flpButtons, 0, 2);
        tlpRoot.Dock = DockStyle.Fill;
        tlpRoot.Location = new Point(12, 12);
        tlpRoot.Name = "tlpRoot";
        tlpRoot.RowCount = 3;
        tlpRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpRoot.RowStyles.Add(new RowStyle());
        tlpRoot.RowStyles.Add(new RowStyle());
        tlpRoot.Size = new Size(900, 637);
        tlpRoot.TabIndex = 0;
        // 
        // tabSettings
        // 
        tabSettings.Controls.Add(tabConnection);
        tabSettings.Controls.Add(tabTables);
        tabSettings.Controls.Add(tabGeneral);
        tabSettings.Dock = DockStyle.Fill;
        tabSettings.Location = new Point(3, 3);
        tabSettings.Name = "tabSettings";
        tabSettings.SelectedIndex = 0;
        tabSettings.Size = new Size(894, 565);
        tabSettings.TabIndex = 0;
        // 
        // tabConnection
        // 
        tabConnection.Controls.Add(tlpConnection);
        tabConnection.Location = new Point(4, 29);
        tabConnection.Name = "tabConnection";
        tabConnection.Padding = new Padding(8);
        tabConnection.Size = new Size(886, 532);
        tabConnection.TabIndex = 0;
        tabConnection.Text = "Connection";
        tabConnection.UseVisualStyleBackColor = true;
        // 
        // tlpConnection
        // 
        tlpConnection.ColumnCount = 2;
        tlpConnection.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220F));
        tlpConnection.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpConnection.Controls.Add(lblProvider, 0, 0);
        tlpConnection.Controls.Add(cmbProvider, 1, 0);
        tlpConnection.Controls.Add(lblServer, 0, 1);
        tlpConnection.Controls.Add(txtServer, 1, 1);
        tlpConnection.Controls.Add(lblPort, 0, 2);
        tlpConnection.Controls.Add(nudPort, 1, 2);
        tlpConnection.Controls.Add(lblDatabase, 0, 3);
        tlpConnection.Controls.Add(txtDatabase, 1, 3);
        tlpConnection.Controls.Add(lblUserId, 0, 4);
        tlpConnection.Controls.Add(txtUserId, 1, 4);
        tlpConnection.Controls.Add(lblPassword, 0, 5);
        tlpConnection.Controls.Add(txtPassword, 1, 5);
        tlpConnection.Controls.Add(chkIntegratedSecurity, 1, 6);
        tlpConnection.Controls.Add(chkEncrypt, 1, 7);
        tlpConnection.Controls.Add(chkTrustServerCertificate, 1, 8);
        tlpConnection.Controls.Add(lblArcadiaName, 0, 9);
        tlpConnection.Controls.Add(txtArcadiaName, 1, 9);
        tlpConnection.Controls.Add(lblTelecasterName, 0, 10);
        tlpConnection.Controls.Add(txtTelecasterName, 1, 10);
        tlpConnection.Controls.Add(lblAuthName, 0, 11);
        tlpConnection.Controls.Add(txtAuthName, 1, 11);
        tlpConnection.Dock = DockStyle.Fill;
        tlpConnection.Location = new Point(8, 8);
        tlpConnection.Name = "tlpConnection";
        tlpConnection.RowCount = 13;
        tlpConnection.RowStyles.Add(new RowStyle());
        tlpConnection.RowStyles.Add(new RowStyle());
        tlpConnection.RowStyles.Add(new RowStyle());
        tlpConnection.RowStyles.Add(new RowStyle());
        tlpConnection.RowStyles.Add(new RowStyle());
        tlpConnection.RowStyles.Add(new RowStyle());
        tlpConnection.RowStyles.Add(new RowStyle());
        tlpConnection.RowStyles.Add(new RowStyle());
        tlpConnection.RowStyles.Add(new RowStyle());
        tlpConnection.RowStyles.Add(new RowStyle());
        tlpConnection.RowStyles.Add(new RowStyle());
        tlpConnection.RowStyles.Add(new RowStyle());
        tlpConnection.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpConnection.Size = new Size(870, 516);
        tlpConnection.TabIndex = 0;
        // 
        // lblProvider
        // 
        lblProvider.Anchor = AnchorStyles.Left;
        lblProvider.AutoSize = true;
        lblProvider.Location = new Point(3, 8);
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
        cmbProvider.Location = new Point(223, 3);
        cmbProvider.Name = "cmbProvider";
        cmbProvider.Size = new Size(644, 28);
        cmbProvider.TabIndex = 1;
        cmbProvider.SelectedIndexChanged += cmbProvider_SelectedIndexChanged;
        // 
        // lblServer
        // 
        lblServer.Anchor = AnchorStyles.Left;
        lblServer.AutoSize = true;
        lblServer.Location = new Point(3, 42);
        lblServer.Name = "lblServer";
        lblServer.Size = new Size(54, 20);
        lblServer.TabIndex = 2;
        lblServer.Text = "Server";
        // 
        // txtServer
        // 
        txtServer.Dock = DockStyle.Fill;
        txtServer.Location = new Point(223, 37);
        txtServer.Name = "txtServer";
        txtServer.Size = new Size(644, 27);
        txtServer.TabIndex = 3;
        txtServer.TextChanged += ConnectionField_Changed;
        // 
        // lblPort
        // 
        lblPort.Anchor = AnchorStyles.Left;
        lblPort.AutoSize = true;
        lblPort.Location = new Point(3, 76);
        lblPort.Name = "lblPort";
        lblPort.Size = new Size(38, 20);
        lblPort.TabIndex = 4;
        lblPort.Text = "Port";
        // 
        // nudPort
        // 
        nudPort.Dock = DockStyle.Left;
        nudPort.Location = new Point(223, 71);
        nudPort.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
        nudPort.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        nudPort.Name = "nudPort";
        nudPort.Size = new Size(150, 27);
        nudPort.TabIndex = 5;
        nudPort.Value = new decimal(new int[] { 1433, 0, 0, 0 });
        nudPort.ValueChanged += ConnectionField_Changed;
        // 
        // lblDatabase
        // 
        lblDatabase.Anchor = AnchorStyles.Left;
        lblDatabase.AutoSize = true;
        lblDatabase.Location = new Point(3, 110);
        lblDatabase.Name = "lblDatabase";
        lblDatabase.Size = new Size(69, 20);
        lblDatabase.TabIndex = 6;
        lblDatabase.Text = "Database";
        // 
        // txtDatabase
        // 
        txtDatabase.Dock = DockStyle.Fill;
        txtDatabase.Location = new Point(223, 105);
        txtDatabase.Name = "txtDatabase";
        txtDatabase.Size = new Size(644, 27);
        txtDatabase.TabIndex = 7;
        txtDatabase.TextChanged += ConnectionField_Changed;
        // 
        // lblUserId
        // 
        lblUserId.Anchor = AnchorStyles.Left;
        lblUserId.AutoSize = true;
        lblUserId.Location = new Point(3, 144);
        lblUserId.Name = "lblUserId";
        lblUserId.Size = new Size(56, 20);
        lblUserId.TabIndex = 8;
        lblUserId.Text = "User ID";
        // 
        // txtUserId
        // 
        txtUserId.Dock = DockStyle.Fill;
        txtUserId.Location = new Point(223, 139);
        txtUserId.Name = "txtUserId";
        txtUserId.Size = new Size(644, 27);
        txtUserId.TabIndex = 9;
        txtUserId.TextChanged += ConnectionField_Changed;
        // 
        // lblPassword
        // 
        lblPassword.Anchor = AnchorStyles.Left;
        lblPassword.AutoSize = true;
        lblPassword.Location = new Point(3, 178);
        lblPassword.Name = "lblPassword";
        lblPassword.Size = new Size(70, 20);
        lblPassword.TabIndex = 10;
        lblPassword.Text = "Password";
        // 
        // txtPassword
        // 
        txtPassword.Dock = DockStyle.Fill;
        txtPassword.Location = new Point(223, 173);
        txtPassword.Name = "txtPassword";
        txtPassword.Size = new Size(644, 27);
        txtPassword.TabIndex = 11;
        txtPassword.UseSystemPasswordChar = true;
        txtPassword.TextChanged += ConnectionField_Changed;
        // 
        // chkIntegratedSecurity
        // 
        chkIntegratedSecurity.AutoSize = true;
        chkIntegratedSecurity.Location = new Point(223, 207);
        chkIntegratedSecurity.Name = "chkIntegratedSecurity";
        chkIntegratedSecurity.Size = new Size(149, 24);
        chkIntegratedSecurity.TabIndex = 12;
        chkIntegratedSecurity.Text = "Integrated security";
        chkIntegratedSecurity.UseVisualStyleBackColor = true;
        chkIntegratedSecurity.CheckedChanged += ConnectionField_Changed;
        // 
        // chkEncrypt
        // 
        chkEncrypt.AutoSize = true;
        chkEncrypt.Location = new Point(223, 237);
        chkEncrypt.Name = "chkEncrypt";
        chkEncrypt.Size = new Size(78, 24);
        chkEncrypt.TabIndex = 13;
        chkEncrypt.Text = "Encrypt";
        chkEncrypt.UseVisualStyleBackColor = true;
        chkEncrypt.CheckedChanged += ConnectionField_Changed;
        // 
        // chkTrustServerCertificate
        // 
        chkTrustServerCertificate.AutoSize = true;
        chkTrustServerCertificate.Location = new Point(223, 267);
        chkTrustServerCertificate.Name = "chkTrustServerCertificate";
        chkTrustServerCertificate.Size = new Size(172, 24);
        chkTrustServerCertificate.TabIndex = 14;
        chkTrustServerCertificate.Text = "Trust server certificate";
        chkTrustServerCertificate.UseVisualStyleBackColor = true;
        chkTrustServerCertificate.CheckedChanged += ConnectionField_Changed;
        //
        // tabTables
        // 
        tabTables.Controls.Add(tlpTables);
        tabTables.Location = new Point(4, 29);
        tabTables.Name = "tabTables";
        tabTables.Padding = new Padding(8);
        tabTables.Size = new Size(886, 532);
        tabTables.TabIndex = 1;
        tabTables.Text = "Table Names";
        tabTables.UseVisualStyleBackColor = true;
        // 
        // tabGeneral
        // 
        tabGeneral.Controls.Add(tlpGeneral);
        tabGeneral.Location = new Point(4, 29);
        tabGeneral.Name = "tabGeneral";
        tabGeneral.Padding = new Padding(8);
        tabGeneral.Size = new Size(886, 532);
        tabGeneral.TabIndex = 2;
        tabGeneral.Text = "General Options";
        tabGeneral.UseVisualStyleBackColor = true;
        // 
        // tlpGeneral
        // 
        tlpGeneral.ColumnCount = 1;
        tlpGeneral.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpGeneral.Controls.Add(chkLimitSelectQueries, 0, 0);
        tlpGeneral.Dock = DockStyle.Fill;
        tlpGeneral.Location = new Point(8, 8);
        tlpGeneral.Name = "tlpGeneral";
        tlpGeneral.RowCount = 2;
        tlpGeneral.RowStyles.Add(new RowStyle());
        tlpGeneral.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpGeneral.Size = new Size(870, 516);
        tlpGeneral.TabIndex = 0;
        // 
        // chkLimitSelectQueries
        // 
        chkLimitSelectQueries.AutoSize = true;
        chkLimitSelectQueries.Location = new Point(3, 8);
        chkLimitSelectQueries.Name = "chkLimitSelectQueries";
        chkLimitSelectQueries.Size = new Size(250, 24);
        chkLimitSelectQueries.TabIndex = 0;
        chkLimitSelectQueries.Text = "Limit Select queries to 1000 items";
        chkLimitSelectQueries.UseVisualStyleBackColor = true;
        chkLimitSelectQueries.CheckedChanged += ConnectionField_Changed;
        // 
        // tlpTables
        // 
        tlpTables.ColumnCount = 2;
        tlpTables.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 220F));
        tlpTables.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpTables.Controls.Add(lblAccountsName, 0, 0);
        tlpTables.Controls.Add(txtAccountsName, 1, 0);
        tlpTables.Controls.Add(lblCharacterResource, 0, 1);
        tlpTables.Controls.Add(txtCharacterResource, 1, 1);
        tlpTables.Controls.Add(lblMonsterResource, 0, 2);
        tlpTables.Controls.Add(txtMonsterResource, 1, 2);
        tlpTables.Controls.Add(lblStringResource, 0, 3);
        tlpTables.Controls.Add(txtStringResource, 1, 3);
        tlpTables.Controls.Add(lblItemResource, 0, 4);
        tlpTables.Controls.Add(txtItemResource, 1, 4);
        tlpTables.Controls.Add(lblSkillResource, 0, 5);
        tlpTables.Controls.Add(txtSkillResource, 1, 5);
        tlpTables.Controls.Add(lblStateResource, 0, 6);
        tlpTables.Controls.Add(txtStateResource, 1, 6);
        tlpTables.Controls.Add(lblNpcResource, 0, 7);
        tlpTables.Controls.Add(txtNpcResource, 1, 7);
        tlpTables.Controls.Add(lblSummonResource, 0, 8);
        tlpTables.Controls.Add(txtSummonResource, 1, 8);
        tlpTables.Dock = DockStyle.Fill;
        tlpTables.Location = new Point(8, 8);
        tlpTables.Name = "tlpTables";
        tlpTables.RowCount = 10;
        tlpTables.RowStyles.Add(new RowStyle());
        tlpTables.RowStyles.Add(new RowStyle());
        tlpTables.RowStyles.Add(new RowStyle());
        tlpTables.RowStyles.Add(new RowStyle());
        tlpTables.RowStyles.Add(new RowStyle());
        tlpTables.RowStyles.Add(new RowStyle());
        tlpTables.RowStyles.Add(new RowStyle());
        tlpTables.RowStyles.Add(new RowStyle());
        tlpTables.RowStyles.Add(new RowStyle());
        tlpTables.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpTables.Size = new Size(870, 516);
        tlpTables.TabIndex = 0;
        // 
        // lblArcadiaName
        // 
        lblArcadiaName.Anchor = AnchorStyles.Left;
        lblArcadiaName.AutoSize = true;
        lblArcadiaName.Location = new Point(3, 8);
        lblArcadiaName.Name = "lblArcadiaName";
        lblArcadiaName.Size = new Size(96, 20);
        lblArcadiaName.TabIndex = 15;
        lblArcadiaName.Text = "Arcadia Name";
        //
        // txtArcadiaName
        //
        txtArcadiaName.Dock = DockStyle.Fill;
        txtArcadiaName.Location = new Point(223, 3);
        txtArcadiaName.Name = "txtArcadiaName";
        txtArcadiaName.Size = new Size(644, 27);
        txtArcadiaName.TabIndex = 16;
        // 
        // lblTelecasterName
        // 
        lblTelecasterName.Anchor = AnchorStyles.Left;
        lblTelecasterName.AutoSize = true;
        lblTelecasterName.Location = new Point(3, 42);
        lblTelecasterName.Name = "lblTelecasterName";
        lblTelecasterName.Size = new Size(113, 20);
        lblTelecasterName.TabIndex = 17;
        lblTelecasterName.Text = "Telecaster Name";
        //
        // txtTelecasterName
        //
        txtTelecasterName.Dock = DockStyle.Fill;
        txtTelecasterName.Location = new Point(223, 37);
        txtTelecasterName.Name = "txtTelecasterName";
        txtTelecasterName.Size = new Size(644, 27);
        txtTelecasterName.TabIndex = 18;
        // 
        // lblAuthName
        // 
        lblAuthName.Anchor = AnchorStyles.Left;
        lblAuthName.AutoSize = true;
        lblAuthName.Location = new Point(3, 76);
        lblAuthName.Name = "lblAuthName";
        lblAuthName.Size = new Size(83, 20);
        lblAuthName.TabIndex = 19;
        lblAuthName.Text = "Auth Name";
        //
        // txtAuthName
        //
        txtAuthName.Dock = DockStyle.Fill;
        txtAuthName.Location = new Point(223, 71);
        txtAuthName.Name = "txtAuthName";
        txtAuthName.Size = new Size(644, 27);
        txtAuthName.TabIndex = 20;
        //
        // lblAccountsName
        //
        lblAccountsName.Anchor = AnchorStyles.Left;
        lblAccountsName.AutoSize = true;
        lblAccountsName.Location = new Point(3, 8);
        lblAccountsName.Name = "lblAccountsName";
        lblAccountsName.Size = new Size(97, 20);
        lblAccountsName.TabIndex = 21;
        lblAccountsName.Text = "Accounts Name";
        //
        // txtAccountsName
        //
        txtAccountsName.Dock = DockStyle.Fill;
        txtAccountsName.Location = new Point(223, 3);
        txtAccountsName.Name = "txtAccountsName";
        txtAccountsName.Size = new Size(644, 27);
        txtAccountsName.TabIndex = 22;
        //
        // lblCharacterResource
        // 
        lblCharacterResource.Anchor = AnchorStyles.Left;
        lblCharacterResource.AutoSize = true;
        lblCharacterResource.Location = new Point(3, 42);
        lblCharacterResource.Name = "lblCharacterResource";
        lblCharacterResource.Size = new Size(127, 20);
        lblCharacterResource.TabIndex = 6;
        lblCharacterResource.Text = "CharacterResource";
        // 
        // txtCharacterResource
        // 
        txtCharacterResource.Dock = DockStyle.Fill;
        txtCharacterResource.Location = new Point(223, 37);
        txtCharacterResource.Name = "txtCharacterResource";
        txtCharacterResource.Size = new Size(644, 27);
        txtCharacterResource.TabIndex = 7;
        // 
        // lblMonsterResource
        // 
        lblMonsterResource.Anchor = AnchorStyles.Left;
        lblMonsterResource.AutoSize = true;
        lblMonsterResource.Location = new Point(3, 76);
        lblMonsterResource.Name = "lblMonsterResource";
        lblMonsterResource.Size = new Size(121, 20);
        lblMonsterResource.TabIndex = 8;
        lblMonsterResource.Text = "MonsterResource";
        // 
        // txtMonsterResource
        // 
        txtMonsterResource.Dock = DockStyle.Fill;
        txtMonsterResource.Location = new Point(223, 71);
        txtMonsterResource.Name = "txtMonsterResource";
        txtMonsterResource.Size = new Size(644, 27);
        txtMonsterResource.TabIndex = 9;
        // 
        // lblStringResource
        // 
        lblStringResource.Anchor = AnchorStyles.Left;
        lblStringResource.AutoSize = true;
        lblStringResource.Location = new Point(3, 110);
        lblStringResource.Name = "lblStringResource";
        lblStringResource.Size = new Size(108, 20);
        lblStringResource.TabIndex = 10;
        lblStringResource.Text = "StringResource";
        // 
        // txtStringResource
        // 
        txtStringResource.Dock = DockStyle.Fill;
        txtStringResource.Location = new Point(223, 105);
        txtStringResource.Name = "txtStringResource";
        txtStringResource.Size = new Size(644, 27);
        txtStringResource.TabIndex = 11;
        //
        // lblItemResource
        // 
        lblItemResource.Anchor = AnchorStyles.Left;
        lblItemResource.AutoSize = true;
        lblItemResource.Location = new Point(3, 144);
        lblItemResource.Name = "lblItemResource";
        lblItemResource.Size = new Size(92, 20);
        lblItemResource.TabIndex = 14;
        lblItemResource.Text = "ItemResource";
        // 
        // txtItemResource
        // 
        txtItemResource.Dock = DockStyle.Fill;
        txtItemResource.Location = new Point(223, 139);
        txtItemResource.Name = "txtItemResource";
        txtItemResource.Size = new Size(644, 27);
        txtItemResource.TabIndex = 15;
        // 
        // lblSkillResource
        // 
        lblSkillResource.Anchor = AnchorStyles.Left;
        lblSkillResource.AutoSize = true;
        lblSkillResource.Location = new Point(3, 178);
        lblSkillResource.Name = "lblSkillResource";
        lblSkillResource.Size = new Size(93, 20);
        lblSkillResource.TabIndex = 16;
        lblSkillResource.Text = "SkillResource";
        // 
        // txtSkillResource
        // 
        txtSkillResource.Dock = DockStyle.Fill;
        txtSkillResource.Location = new Point(223, 173);
        txtSkillResource.Name = "txtSkillResource";
        txtSkillResource.Size = new Size(644, 27);
        txtSkillResource.TabIndex = 17;
        // 
        // lblStateResource
        // 
        lblStateResource.Anchor = AnchorStyles.Left;
        lblStateResource.AutoSize = true;
        lblStateResource.Location = new Point(3, 212);
        lblStateResource.Name = "lblStateResource";
        lblStateResource.Size = new Size(98, 20);
        lblStateResource.TabIndex = 18;
        lblStateResource.Text = "StateResource";
        // 
        // txtStateResource
        // 
        txtStateResource.Dock = DockStyle.Fill;
        txtStateResource.Location = new Point(223, 207);
        txtStateResource.Name = "txtStateResource";
        txtStateResource.Size = new Size(644, 27);
        txtStateResource.TabIndex = 19;
        // 
        // lblNpcResource
        // 
        lblNpcResource.Anchor = AnchorStyles.Left;
        lblNpcResource.AutoSize = true;
        lblNpcResource.Location = new Point(3, 246);
        lblNpcResource.Name = "lblNpcResource";
        lblNpcResource.Size = new Size(90, 20);
        lblNpcResource.TabIndex = 20;
        lblNpcResource.Text = "NpcResource";
        // 
        // txtNpcResource
        // 
        txtNpcResource.Dock = DockStyle.Fill;
        txtNpcResource.Location = new Point(223, 241);
        txtNpcResource.Name = "txtNpcResource";
        txtNpcResource.Size = new Size(644, 27);
        txtNpcResource.TabIndex = 21;
        // 
        // lblSummonResource
        // 
        lblSummonResource.Anchor = AnchorStyles.Left;
        lblSummonResource.AutoSize = true;
        lblSummonResource.Location = new Point(3, 280);
        lblSummonResource.Name = "lblSummonResource";
        lblSummonResource.Size = new Size(119, 20);
        lblSummonResource.TabIndex = 22;
        lblSummonResource.Text = "SummonResource";
        // 
        // txtSummonResource
        // 
        txtSummonResource.Dock = DockStyle.Fill;
        txtSummonResource.Location = new Point(223, 275);
        txtSummonResource.Name = "txtSummonResource";
        txtSummonResource.Size = new Size(644, 27);
        txtSummonResource.TabIndex = 23;
        // 
        // lblStatus
        // 
        lblStatus.AutoSize = true;
        lblStatus.Dock = DockStyle.Fill;
        lblStatus.Location = new Point(3, 571);
        lblStatus.Name = "lblStatus";
        lblStatus.Padding = new Padding(0, 8, 0, 8);
        lblStatus.Size = new Size(894, 36);
        lblStatus.TabIndex = 1;
        lblStatus.Text = "";
        // 
        // flpButtons
        // 
        flpButtons.AutoSize = true;
        flpButtons.Controls.Add(btnTestConnection);
        flpButtons.Controls.Add(btnSave);
        flpButtons.Controls.Add(btnCancel);
        flpButtons.Dock = DockStyle.Right;
        flpButtons.FlowDirection = FlowDirection.RightToLeft;
        flpButtons.Location = new Point(557, 610);
        flpButtons.Name = "flpButtons";
        flpButtons.Size = new Size(343, 35);
        flpButtons.TabIndex = 2;
        // 
        // btnTestConnection
        // 
        btnTestConnection.Location = new Point(237, 3);
        btnTestConnection.Name = "btnTestConnection";
        btnTestConnection.Size = new Size(100, 29);
        btnTestConnection.TabIndex = 0;
        btnTestConnection.Text = "Test";
        btnTestConnection.UseVisualStyleBackColor = true;
        btnTestConnection.Click += btnTestConnection_Click;
        // 
        // btnSave
        // 
        btnSave.Location = new Point(131, 3);
        btnSave.Name = "btnSave";
        btnSave.Size = new Size(100, 29);
        btnSave.TabIndex = 1;
        btnSave.Text = "Save";
        btnSave.UseVisualStyleBackColor = true;
        btnSave.Click += btnSave_Click;
        // 
        // btnCancel
        // 
        btnCancel.Location = new Point(25, 3);
        btnCancel.Name = "btnCancel";
        btnCancel.Size = new Size(100, 29);
        btnCancel.TabIndex = 2;
        btnCancel.Text = "Cancel";
        btnCancel.UseVisualStyleBackColor = true;
        btnCancel.Click += btnCancel_Click;
        // 
        // SettingsForm
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(924, 661);
        Controls.Add(tlpRoot);
        MinimumSize = new Size(900, 680);
        Name = "SettingsForm";
        Padding = new Padding(12);
        ShowIcon = true;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Settings";
        tlpRoot.ResumeLayout(false);
        tlpRoot.PerformLayout();
        tabSettings.ResumeLayout(false);
        tabConnection.ResumeLayout(false);
        tlpConnection.ResumeLayout(false);
        tlpConnection.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)nudPort).EndInit();
        tabTables.ResumeLayout(false);
        tabGeneral.ResumeLayout(false);
        tlpGeneral.ResumeLayout(false);
        tlpGeneral.PerformLayout();
        tlpTables.ResumeLayout(false);
        tlpTables.PerformLayout();
        flpButtons.ResumeLayout(false);
        ResumeLayout(false);
    }
}
