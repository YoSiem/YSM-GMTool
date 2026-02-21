namespace App.WinForms.Controls;

partial class EntityBrowserControl
{
    private System.ComponentModel.IContainer components = null;
    private SplitContainer splitMain;
    private DataGridView gridRecords;
    private TableLayoutPanel tlpCenter;
    private GroupBox gbSearch;
    private RadioButton rbSearchByContactScript;
    private RadioButton rbSearchByName;
    private RadioButton rbSearchById;
    private TextBox txtSearch;
    private Button btnSearch;
    private Button btnLoadAll;
    private Panel pnlActionsHost;
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
        splitMain = new SplitContainer();
        gridRecords = new DataGridView();
        tlpCenter = new TableLayoutPanel();
        gbSearch = new GroupBox();
        btnLoadAll = new Button();
        btnSearch = new Button();
        txtSearch = new TextBox();
        rbSearchByContactScript = new RadioButton();
        rbSearchByName = new RadioButton();
        rbSearchById = new RadioButton();
        pnlActionsHost = new Panel();
        lblStatus = new Label();
        ((System.ComponentModel.ISupportInitialize)splitMain).BeginInit();
        splitMain.Panel1.SuspendLayout();
        splitMain.Panel2.SuspendLayout();
        splitMain.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)gridRecords).BeginInit();
        tlpCenter.SuspendLayout();
        gbSearch.SuspendLayout();
        SuspendLayout();
        // 
        // splitMain
        // 
        splitMain.Dock = DockStyle.Fill;
        splitMain.Location = new Point(0, 0);
        splitMain.Name = "splitMain";
        splitMain.Panel1MinSize = 320;
        splitMain.Panel2MinSize = 360;
        // 
        // splitMain.Panel1
        // 
        splitMain.Panel1.Controls.Add(gridRecords);
        // 
        // splitMain.Panel2
        // 
        splitMain.Panel2.Controls.Add(tlpCenter);
        splitMain.Size = new Size(1040, 640);
        splitMain.SplitterDistance = 610;
        splitMain.TabIndex = 0;
        // 
        // gridRecords
        // 
        gridRecords.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        gridRecords.Dock = DockStyle.Fill;
        gridRecords.Location = new Point(0, 0);
        gridRecords.Name = "gridRecords";
        gridRecords.Size = new Size(560, 640);
        gridRecords.TabIndex = 0;
        gridRecords.CellValueNeeded += gridRecords_CellValueNeeded;
        gridRecords.SelectionChanged += gridRecords_SelectionChanged;
        // 
        // tlpCenter
        // 
        tlpCenter.ColumnCount = 1;
        tlpCenter.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpCenter.Controls.Add(gbSearch, 0, 0);
        tlpCenter.Controls.Add(pnlActionsHost, 0, 1);
        tlpCenter.Controls.Add(lblStatus, 0, 2);
        tlpCenter.Dock = DockStyle.Fill;
        tlpCenter.Location = new Point(0, 0);
        tlpCenter.Name = "tlpCenter";
        tlpCenter.RowCount = 3;
        tlpCenter.RowStyles.Add(new RowStyle());
        tlpCenter.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpCenter.RowStyles.Add(new RowStyle());
        tlpCenter.Size = new Size(476, 640);
        tlpCenter.TabIndex = 0;
        // 
        // gbSearch
        // 
        gbSearch.Controls.Add(btnLoadAll);
        gbSearch.Controls.Add(btnSearch);
        gbSearch.Controls.Add(txtSearch);
        gbSearch.Controls.Add(rbSearchByContactScript);
        gbSearch.Controls.Add(rbSearchByName);
        gbSearch.Controls.Add(rbSearchById);
        gbSearch.Dock = DockStyle.Fill;
        gbSearch.Location = new Point(3, 3);
        gbSearch.Name = "gbSearch";
        gbSearch.Size = new Size(470, 113);
        gbSearch.TabIndex = 0;
        gbSearch.TabStop = false;
        gbSearch.Text = "Search";
        // 
        // btnLoadAll
        // 
        btnLoadAll.Location = new Point(329, 72);
        btnLoadAll.Name = "btnLoadAll";
        btnLoadAll.Size = new Size(124, 29);
        btnLoadAll.TabIndex = 4;
        btnLoadAll.Text = "Load All";
        btnLoadAll.UseVisualStyleBackColor = true;
        btnLoadAll.Click += btnLoadAll_Click;
        // 
        // btnSearch
        // 
        btnSearch.Location = new Point(199, 72);
        btnSearch.Name = "btnSearch";
        btnSearch.Size = new Size(124, 29);
        btnSearch.TabIndex = 3;
        btnSearch.Text = "Search";
        btnSearch.UseVisualStyleBackColor = true;
        btnSearch.Click += btnSearch_Click;
        // 
        // txtSearch
        // 
        txtSearch.Location = new Point(17, 73);
        txtSearch.Name = "txtSearch";
        txtSearch.PlaceholderText = "Type to filter...";
        txtSearch.Size = new Size(176, 27);
        txtSearch.TabIndex = 2;
        txtSearch.TextChanged += txtSearch_TextChanged;
        // 
        // rbSearchByContactScript
        // 
        rbSearchByContactScript.AutoSize = true;
        rbSearchByContactScript.Location = new Point(264, 34);
        rbSearchByContactScript.Name = "rbSearchByContactScript";
        rbSearchByContactScript.Size = new Size(191, 24);
        rbSearchByContactScript.TabIndex = 5;
        rbSearchByContactScript.Text = "Search by Contact script";
        rbSearchByContactScript.UseVisualStyleBackColor = true;
        rbSearchByContactScript.Visible = false;
        rbSearchByContactScript.CheckedChanged += rbSearchBy_CheckedChanged;
        // 
        // rbSearchByName
        // 
        rbSearchByName.AutoSize = true;
        rbSearchByName.Checked = true;
        rbSearchByName.Location = new Point(134, 34);
        rbSearchByName.Name = "rbSearchByName";
        rbSearchByName.Size = new Size(139, 24);
        rbSearchByName.TabIndex = 1;
        rbSearchByName.TabStop = true;
        rbSearchByName.Text = "Search by Name";
        rbSearchByName.UseVisualStyleBackColor = true;
        rbSearchByName.CheckedChanged += rbSearchBy_CheckedChanged;
        // 
        // rbSearchById
        // 
        rbSearchById.AutoSize = true;
        rbSearchById.Location = new Point(17, 34);
        rbSearchById.Name = "rbSearchById";
        rbSearchById.Size = new Size(108, 24);
        rbSearchById.TabIndex = 0;
        rbSearchById.Text = "Search by ID";
        rbSearchById.UseVisualStyleBackColor = true;
        rbSearchById.CheckedChanged += rbSearchBy_CheckedChanged;
        // 
        // pnlActionsHost
        // 
        pnlActionsHost.AutoScroll = true;
        pnlActionsHost.Dock = DockStyle.Fill;
        pnlActionsHost.Location = new Point(3, 122);
        pnlActionsHost.Name = "pnlActionsHost";
        pnlActionsHost.Padding = new Padding(8);
        pnlActionsHost.Size = new Size(470, 488);
        pnlActionsHost.TabIndex = 1;
        // 
        // lblStatus
        // 
        lblStatus.AutoSize = true;
        lblStatus.Dock = DockStyle.Fill;
        lblStatus.Location = new Point(3, 613);
        lblStatus.Name = "lblStatus";
        lblStatus.Padding = new Padding(0, 8, 0, 8);
        lblStatus.Size = new Size(470, 27);
        lblStatus.TabIndex = 2;
        lblStatus.Text = "No data loaded. Click Load All.";
        // 
        // EntityBrowserControl
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(splitMain);
        Name = "EntityBrowserControl";
        Size = new Size(1040, 640);
        splitMain.Panel1.ResumeLayout(false);
        splitMain.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitMain).EndInit();
        splitMain.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)gridRecords).EndInit();
        tlpCenter.ResumeLayout(false);
        tlpCenter.PerformLayout();
        gbSearch.ResumeLayout(false);
        gbSearch.PerformLayout();
        ResumeLayout(false);
    }
}
