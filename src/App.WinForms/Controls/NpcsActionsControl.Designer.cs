namespace App.WinForms.Controls;

partial class NpcsActionsControl
{
    private System.ComponentModel.IContainer components = null;
    private GroupBox gbNpcActions;
    private TableLayoutPanel tlpRoot;
    private GroupBox gbSelectedNpc;
    private GroupBox gbCommands;
    private TableLayoutPanel tlpSelected;
    private Label lblNpcId;
    private NumericUpDown nudNpcId;
    private Label lblNpcName;
    private TextBox txtNpcName;
    private Label lblContactScript;
    private TextBox txtContactScript;
    private Label lblX;
    private NumericUpDown nudX;
    private Label lblY;
    private NumericUpDown nudY;
    private Label lblLayer;
    private NumericUpDown nudLayer;
    private CheckBox chkHideNpc;
    private TableLayoutPanel tlpCommands;
    private Button btnAddNpcToWorld;
    private Button btnShowNpc;
    private Button btnWarpToNpc;

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
        gbNpcActions = new GroupBox();
        tlpRoot = new TableLayoutPanel();
        gbSelectedNpc = new GroupBox();
        tlpSelected = new TableLayoutPanel();
        lblNpcId = new Label();
        nudNpcId = new NumericUpDown();
        lblNpcName = new Label();
        txtNpcName = new TextBox();
        lblContactScript = new Label();
        txtContactScript = new TextBox();
        lblX = new Label();
        nudX = new NumericUpDown();
        lblY = new Label();
        nudY = new NumericUpDown();
        lblLayer = new Label();
        nudLayer = new NumericUpDown();
        chkHideNpc = new CheckBox();
        gbCommands = new GroupBox();
        tlpCommands = new TableLayoutPanel();
        btnAddNpcToWorld = new Button();
        btnShowNpc = new Button();
        btnWarpToNpc = new Button();
        gbNpcActions.SuspendLayout();
        tlpRoot.SuspendLayout();
        gbSelectedNpc.SuspendLayout();
        tlpSelected.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudNpcId).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudX).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudY).BeginInit();
        ((System.ComponentModel.ISupportInitialize)nudLayer).BeginInit();
        gbCommands.SuspendLayout();
        tlpCommands.SuspendLayout();
        SuspendLayout();
        // 
        // gbNpcActions
        // 
        gbNpcActions.Controls.Add(tlpRoot);
        gbNpcActions.Dock = DockStyle.Fill;
        gbNpcActions.Location = new Point(0, 0);
        gbNpcActions.Name = "gbNpcActions";
        gbNpcActions.Size = new Size(580, 430);
        gbNpcActions.TabIndex = 0;
        gbNpcActions.TabStop = false;
        gbNpcActions.Text = "NPC Commands";
        // 
        // tlpRoot
        // 
        tlpRoot.ColumnCount = 1;
        tlpRoot.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpRoot.Controls.Add(gbSelectedNpc, 0, 0);
        tlpRoot.Controls.Add(gbCommands, 0, 1);
        tlpRoot.Dock = DockStyle.Fill;
        tlpRoot.Location = new Point(3, 23);
        tlpRoot.Name = "tlpRoot";
        tlpRoot.RowCount = 2;
        tlpRoot.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpRoot.RowStyles.Add(new RowStyle());
        tlpRoot.Size = new Size(574, 404);
        tlpRoot.TabIndex = 0;
        // 
        // gbSelectedNpc
        // 
        gbSelectedNpc.Controls.Add(tlpSelected);
        gbSelectedNpc.Dock = DockStyle.Fill;
        gbSelectedNpc.Location = new Point(3, 3);
        gbSelectedNpc.Name = "gbSelectedNpc";
        gbSelectedNpc.Size = new Size(568, 194);
        gbSelectedNpc.TabIndex = 0;
        gbSelectedNpc.TabStop = false;
        gbSelectedNpc.Text = "Selected NPC";
        // 
        // tlpSelected
        // 
        tlpSelected.ColumnCount = 2;
        tlpSelected.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
        tlpSelected.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpSelected.Controls.Add(lblNpcId, 0, 0);
        tlpSelected.Controls.Add(nudNpcId, 1, 0);
        tlpSelected.Controls.Add(lblNpcName, 0, 1);
        tlpSelected.Controls.Add(txtNpcName, 1, 1);
        tlpSelected.Controls.Add(lblContactScript, 0, 2);
        tlpSelected.Controls.Add(txtContactScript, 1, 2);
        tlpSelected.Controls.Add(lblX, 0, 3);
        tlpSelected.Controls.Add(nudX, 1, 3);
        tlpSelected.Controls.Add(lblY, 0, 4);
        tlpSelected.Controls.Add(nudY, 1, 4);
        tlpSelected.Controls.Add(lblLayer, 0, 5);
        tlpSelected.Controls.Add(nudLayer, 1, 5);
        tlpSelected.Controls.Add(chkHideNpc, 1, 6);
        tlpSelected.Dock = DockStyle.Fill;
        tlpSelected.Location = new Point(3, 23);
        tlpSelected.Name = "tlpSelected";
        tlpSelected.RowCount = 7;
        tlpSelected.RowStyles.Add(new RowStyle());
        tlpSelected.RowStyles.Add(new RowStyle());
        tlpSelected.RowStyles.Add(new RowStyle());
        tlpSelected.RowStyles.Add(new RowStyle());
        tlpSelected.RowStyles.Add(new RowStyle());
        tlpSelected.RowStyles.Add(new RowStyle());
        tlpSelected.RowStyles.Add(new RowStyle());
        tlpSelected.Size = new Size(562, 168);
        tlpSelected.TabIndex = 0;
        // 
        // lblNpcId
        // 
        lblNpcId.Anchor = AnchorStyles.Left;
        lblNpcId.AutoSize = true;
        lblNpcId.Location = new Point(3, 7);
        lblNpcId.Name = "lblNpcId";
        lblNpcId.Size = new Size(54, 20);
        lblNpcId.TabIndex = 0;
        lblNpcId.Text = "NPC ID";
        // 
        // nudNpcId
        // 
        nudNpcId.Dock = DockStyle.Fill;
        nudNpcId.Location = new Point(123, 3);
        nudNpcId.Maximum = new decimal(new int[] { 2000000000, 0, 0, 0 });
        nudNpcId.Name = "nudNpcId";
        nudNpcId.Size = new Size(436, 27);
        nudNpcId.TabIndex = 1;
        // 
        // lblNpcName
        // 
        lblNpcName.Anchor = AnchorStyles.Left;
        lblNpcName.AutoSize = true;
        lblNpcName.Location = new Point(3, 41);
        lblNpcName.Name = "lblNpcName";
        lblNpcName.Size = new Size(49, 20);
        lblNpcName.TabIndex = 2;
        lblNpcName.Text = "Name";
        // 
        // txtNpcName
        // 
        txtNpcName.Dock = DockStyle.Fill;
        txtNpcName.Location = new Point(123, 37);
        txtNpcName.Name = "txtNpcName";
        txtNpcName.ReadOnly = true;
        txtNpcName.Size = new Size(436, 27);
        txtNpcName.TabIndex = 3;
        // 
        // lblContactScript
        // 
        lblContactScript.Anchor = AnchorStyles.Left;
        lblContactScript.AutoSize = true;
        lblContactScript.Location = new Point(3, 75);
        lblContactScript.Name = "lblContactScript";
        lblContactScript.Size = new Size(98, 20);
        lblContactScript.TabIndex = 4;
        lblContactScript.Text = "Contact script";
        // 
        // txtContactScript
        // 
        txtContactScript.Dock = DockStyle.Fill;
        txtContactScript.Location = new Point(123, 71);
        txtContactScript.Name = "txtContactScript";
        txtContactScript.ReadOnly = true;
        txtContactScript.Size = new Size(436, 27);
        txtContactScript.TabIndex = 5;
        // 
        // lblX
        // 
        lblX.Anchor = AnchorStyles.Left;
        lblX.AutoSize = true;
        lblX.Location = new Point(3, 109);
        lblX.Name = "lblX";
        lblX.Size = new Size(17, 20);
        lblX.TabIndex = 6;
        lblX.Text = "X";
        // 
        // nudX
        // 
        nudX.Dock = DockStyle.Fill;
        nudX.Location = new Point(123, 105);
        nudX.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
        nudX.Minimum = new decimal(new int[] { 1000000, 0, 0, int.MinValue });
        nudX.Name = "nudX";
        nudX.Size = new Size(436, 27);
        nudX.TabIndex = 7;
        // 
        // lblY
        // 
        lblY.Anchor = AnchorStyles.Left;
        lblY.AutoSize = true;
        lblY.Location = new Point(3, 143);
        lblY.Name = "lblY";
        lblY.Size = new Size(16, 20);
        lblY.TabIndex = 8;
        lblY.Text = "Y";
        // 
        // nudY
        // 
        nudY.Dock = DockStyle.Fill;
        nudY.Location = new Point(123, 139);
        nudY.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
        nudY.Minimum = new decimal(new int[] { 1000000, 0, 0, int.MinValue });
        nudY.Name = "nudY";
        nudY.Size = new Size(436, 27);
        nudY.TabIndex = 9;
        // 
        // lblLayer
        // 
        lblLayer.Anchor = AnchorStyles.Left;
        lblLayer.AutoSize = true;
        lblLayer.Location = new Point(3, 177);
        lblLayer.Name = "lblLayer";
        lblLayer.Size = new Size(44, 20);
        lblLayer.TabIndex = 10;
        lblLayer.Text = "Layer";
        // 
        // nudLayer
        // 
        nudLayer.Dock = DockStyle.Fill;
        nudLayer.Location = new Point(123, 173);
        nudLayer.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
        nudLayer.Minimum = new decimal(new int[] { 1000, 0, 0, int.MinValue });
        nudLayer.Name = "nudLayer";
        nudLayer.Size = new Size(436, 27);
        nudLayer.TabIndex = 11;
        // 
        // chkHideNpc
        // 
        chkHideNpc.AutoSize = true;
        chkHideNpc.Location = new Point(123, 207);
        chkHideNpc.Name = "chkHideNpc";
        chkHideNpc.Size = new Size(138, 24);
        chkHideNpc.TabIndex = 12;
        chkHideNpc.Text = "Hide (visible = 1)";
        chkHideNpc.UseVisualStyleBackColor = true;
        // 
        // gbCommands
        // 
        gbCommands.Controls.Add(tlpCommands);
        gbCommands.Dock = DockStyle.Fill;
        gbCommands.Location = new Point(3, 203);
        gbCommands.Name = "gbCommands";
        gbCommands.Size = new Size(568, 62);
        gbCommands.TabIndex = 1;
        gbCommands.TabStop = false;
        gbCommands.Text = "Actions";
        // 
        // tlpCommands
        // 
        tlpCommands.ColumnCount = 3;
        tlpCommands.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
        tlpCommands.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
        tlpCommands.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
        tlpCommands.Controls.Add(btnAddNpcToWorld, 0, 0);
        tlpCommands.Controls.Add(btnShowNpc, 1, 0);
        tlpCommands.Controls.Add(btnWarpToNpc, 2, 0);
        tlpCommands.Dock = DockStyle.Fill;
        tlpCommands.Location = new Point(3, 23);
        tlpCommands.Name = "tlpCommands";
        tlpCommands.RowCount = 1;
        tlpCommands.RowStyles.Add(new RowStyle());
        tlpCommands.Size = new Size(562, 40);
        tlpCommands.TabIndex = 0;
        // 
        // btnAddNpcToWorld
        // 
        btnAddNpcToWorld.Dock = DockStyle.Top;
        btnAddNpcToWorld.Location = new Point(3, 3);
        btnAddNpcToWorld.Name = "btnAddNpcToWorld";
        btnAddNpcToWorld.Size = new Size(181, 36);
        btnAddNpcToWorld.TabIndex = 0;
        btnAddNpcToWorld.Text = "Add NPC to world";
        btnAddNpcToWorld.UseVisualStyleBackColor = true;
        btnAddNpcToWorld.Click += btnAddNpcToWorld_Click;
        // 
        // btnShowNpc
        // 
        btnShowNpc.Dock = DockStyle.Top;
        btnShowNpc.Location = new Point(190, 3);
        btnShowNpc.Name = "btnShowNpc";
        btnShowNpc.Size = new Size(181, 36);
        btnShowNpc.TabIndex = 1;
        btnShowNpc.Text = "Show/Hide NPC";
        btnShowNpc.UseVisualStyleBackColor = true;
        btnShowNpc.Click += btnShowNpc_Click;
        // 
        // btnWarpToNpc
        // 
        btnWarpToNpc.Dock = DockStyle.Top;
        btnWarpToNpc.Location = new Point(377, 3);
        btnWarpToNpc.Name = "btnWarpToNpc";
        btnWarpToNpc.Size = new Size(182, 36);
        btnWarpToNpc.TabIndex = 2;
        btnWarpToNpc.Text = "Warp to NPC";
        btnWarpToNpc.UseVisualStyleBackColor = true;
        btnWarpToNpc.Click += btnWarpToNpc_Click;
        // 
        // NpcsActionsControl
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(gbNpcActions);
        MinimumSize = new Size(400, 0);
        Name = "NpcsActionsControl";
        Size = new Size(580, 380);
        AutoSize = true;
        AutoSizeMode = AutoSizeMode.GrowAndShrink;
        gbNpcActions.ResumeLayout(false);
        tlpRoot.ResumeLayout(false);
        gbSelectedNpc.ResumeLayout(false);
        tlpSelected.ResumeLayout(false);
        tlpSelected.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)nudNpcId).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudX).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudY).EndInit();
        ((System.ComponentModel.ISupportInitialize)nudLayer).EndInit();
        gbCommands.ResumeLayout(false);
        tlpCommands.ResumeLayout(false);
        ResumeLayout(false);
    }
}
