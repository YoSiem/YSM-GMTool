namespace App.WinForms.Controls;

partial class ItemsActionsControl
{
    private System.ComponentModel.IContainer components = null;
    private GroupBox gbItemActions;
    private TabControl tabItemsActions;
    private TabPage tabItem;
    private TabPage tabRandomOption;
    private TabPage tabItemUseFlag;
    private Label lblRandomOptionPlaceholder;
    private Label lblItemUseFlagPlaceholder;
    private TableLayoutPanel tlpItemRoot;
    private GroupBox gbInsertItem;
    private TableLayoutPanel tlpInsert;
    private Label lblItemId;
    private NumericUpDown nudItemId;
    private Label lblItemName;
    private TextBox txtItemName;
    private Label lblAmount;
    private NumericUpDown nudAmount;
    private Label lblEnhance;
    private NumericUpDown nudEnhance;
    private Label lblLevel;
    private NumericUpDown nudLevel;
    private CheckBox chkUseStatusFlag;
    private NumericUpDown nudStatusFlag;
    private Button btnAddYourself;
    private Button btnGiveOtherPlayer;
    private GroupBox gbModifyItem;
    private TableLayoutPanel tlpModify;
    private Label lblWearSlot;
    private ComboBox cmbWearSlot;
    private RadioButton rbTargetOwn;
    private RadioButton rbTargetOther;
    private Label lblModifyLevel;
    private NumericUpDown nudModifyLevel;
    private Label lblModifyEnhance;
    private NumericUpDown nudModifyEnhance;
    private Label lblModifyItemCode;
    private NumericUpDown nudModifyItemCode;
    private FlowLayoutPanel flpModifyButtons;
    private Button btnEditLevel;
    private Button btnEditEnhance;
    private Button btnChangeAppearance;
    private Button btnChangeItemCode;

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
        gbItemActions = new GroupBox();
        tabItemsActions = new TabControl();
        tabItem = new TabPage();
        tlpItemRoot = new TableLayoutPanel();
        gbInsertItem = new GroupBox();
        tlpInsert = new TableLayoutPanel();
        lblItemId = new Label();
        nudItemId = new NumericUpDown();
        lblItemName = new Label();
        txtItemName = new TextBox();
        lblAmount = new Label();
        nudAmount = new NumericUpDown();
        lblEnhance = new Label();
        nudEnhance = new NumericUpDown();
        lblLevel = new Label();
        nudLevel = new NumericUpDown();
        chkUseStatusFlag = new CheckBox();
        nudStatusFlag = new NumericUpDown();
        btnAddYourself = new Button();
        btnGiveOtherPlayer = new Button();
        gbModifyItem = new GroupBox();
        tlpModify = new TableLayoutPanel();
        lblWearSlot = new Label();
        cmbWearSlot = new ComboBox();
        rbTargetOwn = new RadioButton();
        rbTargetOther = new RadioButton();
        lblModifyLevel = new Label();
        nudModifyLevel = new NumericUpDown();
        lblModifyEnhance = new Label();
        nudModifyEnhance = new NumericUpDown();
        lblModifyItemCode = new Label();
        nudModifyItemCode = new NumericUpDown();
        flpModifyButtons = new FlowLayoutPanel();
        btnEditLevel = new Button();
        btnEditEnhance = new Button();
        btnChangeAppearance = new Button();
        btnChangeItemCode = new Button();
        tabRandomOption = new TabPage();
        lblRandomOptionPlaceholder = new Label();
        tabItemUseFlag = new TabPage();
        lblItemUseFlagPlaceholder = new Label();
        gbItemActions.SuspendLayout();
        tabItemsActions.SuspendLayout();
        tabItem.SuspendLayout();
        tlpItemRoot.SuspendLayout();
        gbInsertItem.SuspendLayout();
        tlpInsert.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudItemId).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudAmount).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudEnhance).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudLevel).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudStatusFlag).BeginInit();
        gbModifyItem.SuspendLayout();
        tlpModify.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudModifyLevel).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudModifyEnhance).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudModifyItemCode).BeginInit();
        flpModifyButtons.SuspendLayout();
        tabRandomOption.SuspendLayout();
        tabItemUseFlag.SuspendLayout();
        SuspendLayout();
        // 
        // gbItemActions
        // 
        gbItemActions.Controls.Add(tabItemsActions);
        gbItemActions.Dock = DockStyle.Fill;
        gbItemActions.Location = new Point(0, 0);
        gbItemActions.Name = "gbItemActions";
        gbItemActions.Size = new Size(460, 560);
        gbItemActions.TabIndex = 0;
        gbItemActions.TabStop = false;
        gbItemActions.Text = "Items";
        // 
        // tabItemsActions
        // 
        tabItemsActions.Controls.Add(tabItem);
        tabItemsActions.Controls.Add(tabRandomOption);
        tabItemsActions.Controls.Add(tabItemUseFlag);
        tabItemsActions.Dock = DockStyle.Fill;
        tabItemsActions.Location = new Point(3, 23);
        tabItemsActions.Name = "tabItemsActions";
        tabItemsActions.SelectedIndex = 0;
        tabItemsActions.Size = new Size(454, 534);
        tabItemsActions.TabIndex = 0;
        // 
        // tabItem
        // 
        tabItem.Controls.Add(tlpItemRoot);
        tabItem.Location = new Point(4, 29);
        tabItem.Name = "tabItem";
        tabItem.Padding = new Padding(3);
        tabItem.Size = new Size(446, 501);
        tabItem.TabIndex = 0;
        tabItem.Text = "Item";
        tabItem.UseVisualStyleBackColor = true;
        // 
        // tlpItemRoot
        // 
        tlpItemRoot.ColumnCount = 1;
        tlpItemRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpItemRoot.Controls.Add(gbInsertItem, 0, 0);
        tlpItemRoot.Controls.Add(gbModifyItem, 0, 1);
        tlpItemRoot.Dock = DockStyle.Fill;
        tlpItemRoot.Location = new Point(3, 3);
        tlpItemRoot.Name = "tlpItemRoot";
        tlpItemRoot.RowCount = 3;
        tlpItemRoot.RowStyles.Add(new RowStyle());
        tlpItemRoot.RowStyles.Add(new RowStyle());
        tlpItemRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpItemRoot.Size = new Size(440, 495);
        tlpItemRoot.TabIndex = 0;
        // 
        // gbInsertItem
        // 
        gbInsertItem.Controls.Add(tlpInsert);
        gbInsertItem.Dock = DockStyle.Top;
        gbInsertItem.Location = new Point(3, 3);
        gbInsertItem.Name = "gbInsertItem";
        gbInsertItem.Size = new Size(434, 200);
        gbInsertItem.TabIndex = 0;
        gbInsertItem.TabStop = false;
        gbInsertItem.Text = "Give / Insert Commands";
        // 
        // tlpInsert
        // 
        tlpInsert.ColumnCount = 4;
        tlpInsert.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        tlpInsert.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));
        tlpInsert.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        tlpInsert.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55F));
        tlpInsert.Controls.Add(lblItemId, 0, 0);
        tlpInsert.Controls.Add(nudItemId, 1, 0);
        tlpInsert.Controls.Add(lblItemName, 2, 0);
        tlpInsert.Controls.Add(txtItemName, 3, 0);
        tlpInsert.Controls.Add(lblAmount, 0, 1);
        tlpInsert.Controls.Add(nudAmount, 1, 1);
        tlpInsert.Controls.Add(lblEnhance, 2, 1);
        tlpInsert.Controls.Add(nudEnhance, 3, 1);
        tlpInsert.Controls.Add(lblLevel, 0, 2);
        tlpInsert.Controls.Add(nudLevel, 1, 2);
        tlpInsert.Controls.Add(chkUseStatusFlag, 2, 2);
        tlpInsert.Controls.Add(nudStatusFlag, 3, 2);
        tlpInsert.Controls.Add(btnAddYourself, 0, 3);
        tlpInsert.Controls.Add(btnGiveOtherPlayer, 2, 3);
        tlpInsert.Dock = DockStyle.Fill;
        tlpInsert.Location = new Point(3, 23);
        tlpInsert.Name = "tlpInsert";
        tlpInsert.RowCount = 4;
        tlpInsert.RowStyles.Add(new RowStyle());
        tlpInsert.RowStyles.Add(new RowStyle());
        tlpInsert.RowStyles.Add(new RowStyle());
        tlpInsert.RowStyles.Add(new RowStyle());
        tlpInsert.Size = new Size(428, 174);
        tlpInsert.TabIndex = 0;
        // 
        // lblItemId
        // 
        lblItemId.Anchor = AnchorStyles.Left;
        lblItemId.AutoSize = true;
        lblItemId.Location = new Point(3, 8);
        lblItemId.Name = "lblItemId";
        lblItemId.Size = new Size(26, 20);
        lblItemId.TabIndex = 0;
        lblItemId.Text = "ID:";
        // 
        // nudItemId
        // 
        nudItemId.Dock = DockStyle.Fill;
        nudItemId.Location = new Point(35, 3);
        nudItemId.Maximum = new decimal(new int[] { 1000000000, 0, 0, 0 });
        nudItemId.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        nudItemId.Name = "nudItemId";
        nudItemId.Size = new Size(146, 27);
        nudItemId.TabIndex = 1;
        nudItemId.Value = new decimal(new int[] { 1, 0, 0, 0 });
        // 
        // lblItemName
        // 
        lblItemName.Anchor = AnchorStyles.Left;
        lblItemName.AutoSize = true;
        lblItemName.Location = new Point(187, 8);
        lblItemName.Name = "lblItemName";
        lblItemName.Size = new Size(53, 20);
        lblItemName.TabIndex = 2;
        lblItemName.Text = "Name:";
        // 
        // txtItemName
        // 
        txtItemName.Dock = DockStyle.Fill;
        txtItemName.Location = new Point(246, 3);
        txtItemName.Name = "txtItemName";
        txtItemName.ReadOnly = true;
        txtItemName.Size = new Size(179, 27);
        txtItemName.TabIndex = 3;
        // 
        // lblAmount
        // 
        lblAmount.Anchor = AnchorStyles.Left;
        lblAmount.AutoSize = true;
        lblAmount.Location = new Point(3, 42);
        lblAmount.Name = "lblAmount";
        lblAmount.Size = new Size(67, 20);
        lblAmount.TabIndex = 4;
        lblAmount.Text = "Amount:";
        // 
        // nudAmount
        // 
        nudAmount.Dock = DockStyle.Fill;
        nudAmount.Location = new Point(35, 37);
        nudAmount.Maximum = new decimal(new int[] { 99999, 0, 0, 0 });
        nudAmount.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        nudAmount.Name = "nudAmount";
        nudAmount.Size = new Size(146, 27);
        nudAmount.TabIndex = 5;
        nudAmount.Value = new decimal(new int[] { 1, 0, 0, 0 });
        // 
        // lblEnhance
        // 
        lblEnhance.Anchor = AnchorStyles.Left;
        lblEnhance.AutoSize = true;
        lblEnhance.Location = new Point(187, 42);
        lblEnhance.Name = "lblEnhance";
        lblEnhance.Size = new Size(67, 20);
        lblEnhance.TabIndex = 6;
        lblEnhance.Text = "Enhance:";
        // 
        // nudEnhance
        // 
        nudEnhance.Dock = DockStyle.Fill;
        nudEnhance.Location = new Point(246, 37);
        nudEnhance.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
        nudEnhance.Name = "nudEnhance";
        nudEnhance.Size = new Size(179, 27);
        nudEnhance.TabIndex = 7;
        // 
        // lblLevel
        // 
        lblLevel.Anchor = AnchorStyles.Left;
        lblLevel.AutoSize = true;
        lblLevel.Location = new Point(3, 76);
        lblLevel.Name = "lblLevel";
        lblLevel.Size = new Size(46, 20);
        lblLevel.TabIndex = 8;
        lblLevel.Text = "Level:";
        // 
        // nudLevel
        // 
        nudLevel.Dock = DockStyle.Fill;
        nudLevel.Location = new Point(35, 71);
        nudLevel.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
        nudLevel.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        nudLevel.Name = "nudLevel";
        nudLevel.Size = new Size(146, 27);
        nudLevel.TabIndex = 9;
        nudLevel.Value = new decimal(new int[] { 1, 0, 0, 0 });
        // 
        // chkUseStatusFlag
        // 
        chkUseStatusFlag.Anchor = AnchorStyles.Left;
        chkUseStatusFlag.AutoSize = true;
        chkUseStatusFlag.Location = new Point(187, 73);
        chkUseStatusFlag.Name = "chkUseStatusFlag";
        chkUseStatusFlag.Size = new Size(93, 24);
        chkUseStatusFlag.TabIndex = 10;
        chkUseStatusFlag.Text = "Statusflag";
        chkUseStatusFlag.UseVisualStyleBackColor = true;
        chkUseStatusFlag.CheckedChanged += chkUseStatusFlag_CheckedChanged;
        // 
        // nudStatusFlag
        // 
        nudStatusFlag.Dock = DockStyle.Fill;
        nudStatusFlag.Location = new Point(246, 71);
        nudStatusFlag.Maximum = new decimal(new int[] { 999999, 0, 0, 0 });
        nudStatusFlag.Name = "nudStatusFlag";
        nudStatusFlag.Size = new Size(179, 27);
        nudStatusFlag.TabIndex = 11;
        // 
        // btnAddYourself
        // 
        tlpInsert.SetColumnSpan(btnAddYourself, 2);
        btnAddYourself.Dock = DockStyle.Fill;
        btnAddYourself.Location = new Point(3, 105);
        btnAddYourself.Name = "btnAddYourself";
        btnAddYourself.Size = new Size(178, 36);
        btnAddYourself.TabIndex = 12;
        btnAddYourself.Text = "Add yourself";
        btnAddYourself.UseVisualStyleBackColor = true;
        btnAddYourself.Click += btnAddYourself_Click;
        // 
        // btnGiveOtherPlayer
        // 
        tlpInsert.SetColumnSpan(btnGiveOtherPlayer, 2);
        btnGiveOtherPlayer.Dock = DockStyle.Fill;
        btnGiveOtherPlayer.Location = new Point(187, 105);
        btnGiveOtherPlayer.Name = "btnGiveOtherPlayer";
        btnGiveOtherPlayer.Size = new Size(238, 36);
        btnGiveOtherPlayer.TabIndex = 13;
        btnGiveOtherPlayer.Text = "Give other player";
        btnGiveOtherPlayer.UseVisualStyleBackColor = true;
        btnGiveOtherPlayer.Click += btnGiveOtherPlayer_Click;
        // 
        // gbModifyItem
        // 
        gbModifyItem.Controls.Add(tlpModify);
        gbModifyItem.Dock = DockStyle.Top;
        gbModifyItem.Location = new Point(3, 209);
        gbModifyItem.Name = "gbModifyItem";
        gbModifyItem.Size = new Size(434, 236);
        gbModifyItem.TabIndex = 1;
        gbModifyItem.TabStop = false;
        gbModifyItem.Text = "Modify Equipped Item";
        // 
        // tlpModify
        // 
        tlpModify.ColumnCount = 4;
        tlpModify.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        tlpModify.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));
        tlpModify.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        tlpModify.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 55F));
        tlpModify.Controls.Add(lblWearSlot, 0, 0);
        tlpModify.Controls.Add(cmbWearSlot, 1, 0);
        tlpModify.Controls.Add(rbTargetOwn, 2, 0);
        tlpModify.Controls.Add(rbTargetOther, 3, 0);
        tlpModify.Controls.Add(lblModifyLevel, 0, 1);
        tlpModify.Controls.Add(nudModifyLevel, 1, 1);
        tlpModify.Controls.Add(lblModifyEnhance, 2, 1);
        tlpModify.Controls.Add(nudModifyEnhance, 3, 1);
        tlpModify.Controls.Add(lblModifyItemCode, 0, 2);
        tlpModify.Controls.Add(nudModifyItemCode, 1, 2);
        tlpModify.Controls.Add(flpModifyButtons, 0, 3);
        tlpModify.Dock = DockStyle.Fill;
        tlpModify.Location = new Point(3, 23);
        tlpModify.Name = "tlpModify";
        tlpModify.RowCount = 4;
        tlpModify.RowStyles.Add(new RowStyle());
        tlpModify.RowStyles.Add(new RowStyle());
        tlpModify.RowStyles.Add(new RowStyle());
        tlpModify.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpModify.Size = new Size(428, 210);
        tlpModify.TabIndex = 0;
        // 
        // lblWearSlot
        // 
        lblWearSlot.Anchor = AnchorStyles.Left;
        lblWearSlot.AutoSize = true;
        lblWearSlot.Location = new Point(3, 8);
        lblWearSlot.Name = "lblWearSlot";
        lblWearSlot.Size = new Size(112, 20);
        lblWearSlot.TabIndex = 0;
        lblWearSlot.Text = "Targeted Wear-Slot:";
        // 
        // cmbWearSlot
        // 
        cmbWearSlot.DropDownWidth = 320;
        cmbWearSlot.Dock = DockStyle.Fill;
        cmbWearSlot.DropDownStyle = ComboBoxStyle.DropDownList;
        cmbWearSlot.FormattingEnabled = true;
        cmbWearSlot.Location = new Point(121, 3);
        cmbWearSlot.Name = "cmbWearSlot";
        cmbWearSlot.Size = new Size(89, 28);
        cmbWearSlot.TabIndex = 1;
        // 
        // rbTargetOwn
        // 
        rbTargetOwn.Anchor = AnchorStyles.Left;
        rbTargetOwn.AutoSize = true;
        rbTargetOwn.Location = new Point(216, 5);
        rbTargetOwn.Name = "rbTargetOwn";
        rbTargetOwn.Size = new Size(60, 24);
        rbTargetOwn.TabIndex = 2;
        rbTargetOwn.TabStop = true;
        rbTargetOwn.Text = "Own";
        rbTargetOwn.UseVisualStyleBackColor = true;
        // 
        // rbTargetOther
        // 
        rbTargetOther.Anchor = AnchorStyles.Left;
        rbTargetOther.AutoSize = true;
        rbTargetOther.Location = new Point(282, 5);
        rbTargetOther.Name = "rbTargetOther";
        rbTargetOther.Size = new Size(68, 24);
        rbTargetOther.TabIndex = 3;
        rbTargetOther.TabStop = true;
        rbTargetOther.Text = "Other";
        rbTargetOther.UseVisualStyleBackColor = true;
        // 
        // lblModifyLevel
        // 
        lblModifyLevel.Anchor = AnchorStyles.Left;
        lblModifyLevel.AutoSize = true;
        lblModifyLevel.Location = new Point(3, 42);
        lblModifyLevel.Name = "lblModifyLevel";
        lblModifyLevel.Size = new Size(46, 20);
        lblModifyLevel.TabIndex = 4;
        lblModifyLevel.Text = "Level:";
        // 
        // nudModifyLevel
        // 
        nudModifyLevel.Dock = DockStyle.Fill;
        nudModifyLevel.Location = new Point(121, 37);
        nudModifyLevel.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
        nudModifyLevel.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        nudModifyLevel.Name = "nudModifyLevel";
        nudModifyLevel.Size = new Size(89, 27);
        nudModifyLevel.TabIndex = 5;
        nudModifyLevel.Value = new decimal(new int[] { 1, 0, 0, 0 });
        // 
        // lblModifyEnhance
        // 
        lblModifyEnhance.Anchor = AnchorStyles.Left;
        lblModifyEnhance.AutoSize = true;
        lblModifyEnhance.Location = new Point(216, 42);
        lblModifyEnhance.Name = "lblModifyEnhance";
        lblModifyEnhance.Size = new Size(67, 20);
        lblModifyEnhance.TabIndex = 6;
        lblModifyEnhance.Text = "Enhance:";
        // 
        // nudModifyEnhance
        // 
        nudModifyEnhance.Dock = DockStyle.Fill;
        nudModifyEnhance.Location = new Point(282, 37);
        nudModifyEnhance.Maximum = new decimal(new int[] { 999, 0, 0, 0 });
        nudModifyEnhance.Name = "nudModifyEnhance";
        nudModifyEnhance.Size = new Size(143, 27);
        nudModifyEnhance.TabIndex = 7;
        // 
        // lblModifyItemCode
        // 
        lblModifyItemCode.Anchor = AnchorStyles.Left;
        lblModifyItemCode.AutoSize = true;
        lblModifyItemCode.Location = new Point(3, 76);
        lblModifyItemCode.Name = "lblModifyItemCode";
        lblModifyItemCode.Size = new Size(76, 20);
        lblModifyItemCode.TabIndex = 8;
        lblModifyItemCode.Text = "Itemcode:";
        // 
        // nudModifyItemCode
        // 
        tlpModify.SetColumnSpan(nudModifyItemCode, 3);
        nudModifyItemCode.Dock = DockStyle.Fill;
        nudModifyItemCode.Location = new Point(121, 71);
        nudModifyItemCode.Maximum = new decimal(new int[] { 1000000000, 0, 0, 0 });
        nudModifyItemCode.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
        nudModifyItemCode.Name = "nudModifyItemCode";
        nudModifyItemCode.Size = new Size(304, 27);
        nudModifyItemCode.TabIndex = 9;
        nudModifyItemCode.Value = new decimal(new int[] { 1, 0, 0, 0 });
        // 
        // flpModifyButtons
        // 
        tlpModify.SetColumnSpan(flpModifyButtons, 4);
        flpModifyButtons.Controls.Add(btnEditLevel);
        flpModifyButtons.Controls.Add(btnEditEnhance);
        flpModifyButtons.Controls.Add(btnChangeAppearance);
        flpModifyButtons.Controls.Add(btnChangeItemCode);
        flpModifyButtons.Dock = DockStyle.Fill;
        flpModifyButtons.Location = new Point(3, 105);
        flpModifyButtons.Name = "flpModifyButtons";
        flpModifyButtons.Size = new Size(422, 102);
        flpModifyButtons.TabIndex = 10;
        // 
        // btnEditLevel
        // 
        btnEditLevel.Location = new Point(3, 3);
        btnEditLevel.Name = "btnEditLevel";
        btnEditLevel.Size = new Size(115, 36);
        btnEditLevel.TabIndex = 0;
        btnEditLevel.Text = "Edit level";
        btnEditLevel.UseVisualStyleBackColor = true;
        btnEditLevel.Click += btnEditLevel_Click;
        // 
        // btnEditEnhance
        // 
        btnEditEnhance.Location = new Point(124, 3);
        btnEditEnhance.Name = "btnEditEnhance";
        btnEditEnhance.Size = new Size(115, 36);
        btnEditEnhance.TabIndex = 1;
        btnEditEnhance.Text = "Edit Enhance";
        btnEditEnhance.UseVisualStyleBackColor = true;
        btnEditEnhance.Click += btnEditEnhance_Click;
        // 
        // btnChangeAppearance
        // 
        btnChangeAppearance.Location = new Point(245, 3);
        btnChangeAppearance.Name = "btnChangeAppearance";
        btnChangeAppearance.Size = new Size(174, 36);
        btnChangeAppearance.TabIndex = 2;
        btnChangeAppearance.Text = "Change appearance";
        btnChangeAppearance.UseVisualStyleBackColor = true;
        btnChangeAppearance.Click += btnChangeAppearance_Click;
        // 
        // btnChangeItemCode
        // 
        btnChangeItemCode.Location = new Point(3, 43);
        btnChangeItemCode.Name = "btnChangeItemCode";
        btnChangeItemCode.Size = new Size(236, 36);
        btnChangeItemCode.TabIndex = 3;
        btnChangeItemCode.Text = "Change itemcode of targeted wear-slot";
        btnChangeItemCode.UseVisualStyleBackColor = true;
        btnChangeItemCode.Click += btnChangeItemCode_Click;
        // 
        // tabRandomOption
        // 
        tabRandomOption.Controls.Add(lblRandomOptionPlaceholder);
        tabRandomOption.Location = new Point(4, 29);
        tabRandomOption.Name = "tabRandomOption";
        tabRandomOption.Padding = new Padding(3);
        tabRandomOption.Size = new Size(446, 501);
        tabRandomOption.TabIndex = 1;
        tabRandomOption.Text = "Random Option";
        tabRandomOption.UseVisualStyleBackColor = true;
        // 
        // lblRandomOptionPlaceholder
        // 
        lblRandomOptionPlaceholder.AutoSize = true;
        lblRandomOptionPlaceholder.Location = new Point(16, 20);
        lblRandomOptionPlaceholder.Name = "lblRandomOptionPlaceholder";
        lblRandomOptionPlaceholder.Size = new Size(172, 20);
        lblRandomOptionPlaceholder.TabIndex = 0;
        lblRandomOptionPlaceholder.Text = "Random Option (coming soon)";
        // 
        // tabItemUseFlag
        // 
        tabItemUseFlag.Controls.Add(lblItemUseFlagPlaceholder);
        tabItemUseFlag.Location = new Point(4, 29);
        tabItemUseFlag.Name = "tabItemUseFlag";
        tabItemUseFlag.Padding = new Padding(3);
        tabItemUseFlag.Size = new Size(446, 501);
        tabItemUseFlag.TabIndex = 2;
        tabItemUseFlag.Text = "Itemuseflag";
        tabItemUseFlag.UseVisualStyleBackColor = true;
        // 
        // lblItemUseFlagPlaceholder
        // 
        lblItemUseFlagPlaceholder.AutoSize = true;
        lblItemUseFlagPlaceholder.Location = new Point(16, 20);
        lblItemUseFlagPlaceholder.Name = "lblItemUseFlagPlaceholder";
        lblItemUseFlagPlaceholder.Size = new Size(158, 20);
        lblItemUseFlagPlaceholder.TabIndex = 0;
        lblItemUseFlagPlaceholder.Text = "Itemuseflag (coming soon)";
        // 
        // ItemsActionsControl
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(gbItemActions);
        Name = "ItemsActionsControl";
        Size = new Size(460, 560);
        AutoSize = true;
        AutoSizeMode = AutoSizeMode.GrowAndShrink;
        gbItemActions.ResumeLayout(false);
        tabItemsActions.ResumeLayout(false);
        tabItem.ResumeLayout(false);
        tlpItemRoot.ResumeLayout(false);
        gbInsertItem.ResumeLayout(false);
        tlpInsert.ResumeLayout(false);
        tlpInsert.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)nudItemId).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudAmount).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudEnhance).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudLevel).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudStatusFlag).EndInit();
        gbModifyItem.ResumeLayout(false);
        tlpModify.ResumeLayout(false);
        tlpModify.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)nudModifyLevel).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudModifyEnhance).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudModifyItemCode).EndInit();
        flpModifyButtons.ResumeLayout(false);
        tabRandomOption.ResumeLayout(false);
        tabRandomOption.PerformLayout();
        tabItemUseFlag.ResumeLayout(false);
        tabItemUseFlag.PerformLayout();
        ResumeLayout(false);
    }
}
