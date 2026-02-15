namespace App.WinForms.Controls;

partial class BuffsActionsControl
{
    private System.ComponentModel.IContainer components = null;
    private GroupBox gbBuffActions;
    private TableLayoutPanel tlp;
    private Label lblDuration;
    private NumericUpDown nudDurationSeconds;
    private Label lblWorldState;
    private TextBox txtWorldStateValue;
    private Button btnAddBuff;
    private Button btnRemoveBuff;
    private Button btnSetTimedWorldState;

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
        gbBuffActions = new GroupBox();
        tlp = new TableLayoutPanel();
        lblDuration = new Label();
        nudDurationSeconds = new NumericUpDown();
        lblWorldState = new Label();
        txtWorldStateValue = new TextBox();
        btnAddBuff = new Button();
        btnRemoveBuff = new Button();
        btnSetTimedWorldState = new Button();
        gbBuffActions.SuspendLayout();
        tlp.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)nudDurationSeconds).BeginInit();
        SuspendLayout();
        // 
        // gbBuffActions
        // 
        gbBuffActions.Controls.Add(tlp);
        gbBuffActions.Dock = DockStyle.Fill;
        gbBuffActions.Location = new Point(0, 0);
        gbBuffActions.Name = "gbBuffActions";
        gbBuffActions.Size = new Size(430, 230);
        gbBuffActions.TabIndex = 0;
        gbBuffActions.TabStop = false;
        gbBuffActions.Text = "Buff / State Commands";
        // 
        // tlp
        // 
        tlp.ColumnCount = 2;
        tlp.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlp.Controls.Add(lblDuration, 0, 0);
        tlp.Controls.Add(nudDurationSeconds, 1, 0);
        tlp.Controls.Add(lblWorldState, 0, 1);
        tlp.Controls.Add(txtWorldStateValue, 1, 1);
        tlp.Controls.Add(btnAddBuff, 0, 2);
        tlp.Controls.Add(btnRemoveBuff, 0, 3);
        tlp.Controls.Add(btnSetTimedWorldState, 0, 4);
        tlp.Dock = DockStyle.Fill;
        tlp.Location = new Point(3, 23);
        tlp.Name = "tlp";
        tlp.RowCount = 5;
        tlp.RowStyles.Add(new RowStyle());
        tlp.RowStyles.Add(new RowStyle());
        tlp.RowStyles.Add(new RowStyle());
        tlp.RowStyles.Add(new RowStyle());
        tlp.RowStyles.Add(new RowStyle());
        tlp.Size = new Size(424, 204);
        tlp.TabIndex = 0;
        // 
        // lblDuration
        // 
        lblDuration.Anchor = AnchorStyles.Left;
        lblDuration.AutoSize = true;
        lblDuration.Location = new Point(3, 9);
        lblDuration.Name = "lblDuration";
        lblDuration.Size = new Size(119, 20);
        lblDuration.TabIndex = 0;
        lblDuration.Text = "Duration (seconds)";
        // 
        // nudDurationSeconds
        // 
        nudDurationSeconds.Dock = DockStyle.Fill;
        nudDurationSeconds.Location = new Point(128, 3);
        nudDurationSeconds.Maximum = new decimal(new int[] { 86400, 0, 0, 0 });
        nudDurationSeconds.Name = "nudDurationSeconds";
        nudDurationSeconds.Size = new Size(293, 27);
        nudDurationSeconds.TabIndex = 1;
        // 
        // lblWorldState
        // 
        lblWorldState.Anchor = AnchorStyles.Left;
        lblWorldState.AutoSize = true;
        lblWorldState.Location = new Point(3, 43);
        lblWorldState.Name = "lblWorldState";
        lblWorldState.Size = new Size(97, 20);
        lblWorldState.TabIndex = 2;
        lblWorldState.Text = "World state val";
        // 
        // txtWorldStateValue
        // 
        txtWorldStateValue.Dock = DockStyle.Fill;
        txtWorldStateValue.Location = new Point(128, 37);
        txtWorldStateValue.Name = "txtWorldStateValue";
        txtWorldStateValue.PlaceholderText = "e.g. 1";
        txtWorldStateValue.Size = new Size(293, 27);
        txtWorldStateValue.TabIndex = 3;
        // 
        // btnAddBuff
        // 
        tlp.SetColumnSpan(btnAddBuff, 2);
        btnAddBuff.Dock = DockStyle.Fill;
        btnAddBuff.Location = new Point(3, 71);
        btnAddBuff.Name = "btnAddBuff";
        btnAddBuff.Size = new Size(418, 29);
        btnAddBuff.TabIndex = 4;
        btnAddBuff.Text = "Add selected buff";
        btnAddBuff.UseVisualStyleBackColor = true;
        btnAddBuff.Click += btnAddBuff_Click;
        // 
        // btnRemoveBuff
        // 
        tlp.SetColumnSpan(btnRemoveBuff, 2);
        btnRemoveBuff.Dock = DockStyle.Fill;
        btnRemoveBuff.Location = new Point(3, 106);
        btnRemoveBuff.Name = "btnRemoveBuff";
        btnRemoveBuff.Size = new Size(418, 29);
        btnRemoveBuff.TabIndex = 5;
        btnRemoveBuff.Text = "Remove selected buff";
        btnRemoveBuff.UseVisualStyleBackColor = true;
        btnRemoveBuff.Click += btnRemoveBuff_Click;
        // 
        // btnSetTimedWorldState
        // 
        tlp.SetColumnSpan(btnSetTimedWorldState, 2);
        btnSetTimedWorldState.Dock = DockStyle.Fill;
        btnSetTimedWorldState.Location = new Point(3, 141);
        btnSetTimedWorldState.Name = "btnSetTimedWorldState";
        btnSetTimedWorldState.Size = new Size(418, 29);
        btnSetTimedWorldState.TabIndex = 6;
        btnSetTimedWorldState.Text = "Set timed world state";
        btnSetTimedWorldState.UseVisualStyleBackColor = true;
        btnSetTimedWorldState.Click += btnSetTimedWorldState_Click;
        // 
        // BuffsActionsControl
        // 
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(gbBuffActions);
        Name = "BuffsActionsControl";
        Size = new Size(430, 230);
        gbBuffActions.ResumeLayout(false);
        tlp.ResumeLayout(false);
        tlp.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)nudDurationSeconds).EndInit();
        ResumeLayout(false);
    }
}
