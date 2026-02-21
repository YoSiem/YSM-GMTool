using App.WinForms.Models;
using System.Reflection;
using System.Drawing;
using System.ComponentModel;

namespace App.WinForms.Controls;

public partial class EntityBrowserControl : UserControl
{
    private IReadOnlyList<BrowserRow> _rows = [];
    private CancellationTokenSource? _debounceCts;
    private bool _splitterInitialized;
    private bool _splitterUserAdjusted;
    private int _debounceMs = 250;

    public EntityBrowserControl()
    {
        InitializeComponent();
        ConfigureGridDefaults();
        ApplyReadabilityPalette();
        InitializeLayoutDefaults();
    }

    public event EventHandler? LoadAllRequested;

    public event EventHandler? FilterRequested;

    public event EventHandler<BrowserRow?>? SelectedRowChanged;

    public Panel ActionsHostPanel => pnlActionsHost;

    public string SearchText => txtSearch.Text;

    public SearchMode CurrentSearchMode
    {
        get
        {
            if (rbSearchById.Checked)
            {
                return SearchMode.ById;
            }

            if (rbSearchByContactScript.Visible && rbSearchByContactScript.Checked)
            {
                return SearchMode.ByContactScript;
            }

            return SearchMode.ByName;
        }
    }

    public void ConfigureSearchLabels(string byIdLabel, string byNameLabel)
    {
        rbSearchById.Text = byIdLabel;
        rbSearchByName.Text = byNameLabel;
    }

    public void HideLoadAllButton()
    {
        btnLoadAll.Visible = false;
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool RealtimeFilteringEnabled
    {
        get => chkRealtime.Checked;
        set => chkRealtime.Checked = value;
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool RealtimeFilteringVisible
    {
        get => chkRealtime.Visible;
        set => chkRealtime.Visible = value;
    }

    public void SetDebounceDelay(int milliseconds)
    {
        _debounceMs = Math.Max(50, milliseconds);
    }

    public void ConfigureSecondarySearch(bool enabled, string label = "Search by Contact script")
    {
        rbSearchByContactScript.Text = label;
        rbSearchByContactScript.Visible = enabled;
        rbSearchByContactScript.Enabled = enabled;

        if (!enabled && rbSearchByContactScript.Checked)
        {
            rbSearchByName.Checked = true;
        }
    }

    public void ConfigureColumns(IReadOnlyList<BrowserColumnDefinition> columns)
    {
        gridRecords.SuspendLayout();
        gridRecords.Columns.Clear();
        gridRecords.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        foreach (var column in columns)
        {
            var fillWeight = column.Fill
                ? Math.Max(140f, column.Width)
                : Math.Max(55f, column.Width * 0.75f);
            var minimumWidth = column.Fill
                ? Math.Max(170, column.Width / 2)
                : Math.Max(60, Math.Min(column.Width, 150));

            var dataGridColumn = new DataGridViewTextBoxColumn
            {
                Name = column.Name,
                HeaderText = column.HeaderText,
                Width = column.Width,
                ReadOnly = true,
                SortMode = DataGridViewColumnSortMode.NotSortable,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = fillWeight,
                MinimumWidth = minimumWidth
            };

            gridRecords.Columns.Add(dataGridColumn);
        }

        gridRecords.ResumeLayout();
    }

    public void SetRows(IReadOnlyList<BrowserRow> rows)
    {
        _rows = rows;

        gridRecords.RowCount = _rows.Count;
        gridRecords.CurrentCell = null;
        gridRecords.ClearSelection();

        // In VirtualMode visible cells can keep stale values when row count stays unchanged.
        gridRecords.Invalidate();

        SelectedRowChanged?.Invoke(this, null);
    }

    public void SetStatus(string status)
    {
        lblStatus.Text = status;
    }

    private void ConfigureGridDefaults()
    {
        gridRecords.VirtualMode = true;
        gridRecords.AllowUserToAddRows = false;
        gridRecords.AllowUserToDeleteRows = false;
        gridRecords.MultiSelect = false;
        gridRecords.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        gridRecords.ReadOnly = true;
        gridRecords.RowHeadersVisible = false;
        gridRecords.AllowUserToResizeRows = false;
        gridRecords.RowTemplate.Resizable = DataGridViewTriState.False;
        gridRecords.AutoGenerateColumns = false;
        gridRecords.EnableHeadersVisualStyles = false;
        gridRecords.ColumnHeadersDefaultCellStyle.SelectionBackColor = gridRecords.ColumnHeadersDefaultCellStyle.BackColor;
        gridRecords.ColumnHeadersDefaultCellStyle.SelectionForeColor = gridRecords.ColumnHeadersDefaultCellStyle.ForeColor;
        gridRecords.AllowUserToOrderColumns = false;
        gridRecords.AllowUserToResizeColumns = false;
        gridRecords.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
        gridRecords.CellDoubleClick += gridRecords_CellDoubleClick;
        TryEnableDoubleBuffering(gridRecords);
    }

    private void InitializeLayoutDefaults()
    {
        splitMain.SplitterMoved += splitMain_SplitterMoved;
        splitMain.SizeChanged += splitMain_SizeChanged;
        ApplyTableFocusedSplitRatio();
    }

    private void ApplyReadabilityPalette()
    {
        var page = Color.FromArgb(23, 25, 30);
        var panel = Color.FromArgb(30, 34, 41);
        var panelAlt = Color.FromArgb(35, 40, 49);
        var header = Color.FromArgb(48, 55, 67);
        var border = Color.FromArgb(68, 78, 95);
        var accent = Color.FromArgb(72, 118, 196);
        var text = Color.FromArgb(235, 238, 245);
        var muted = Color.FromArgb(187, 194, 206);

        BackColor = page;
        splitMain.BackColor = page;
        tlpCenter.BackColor = page;
        pnlActionsHost.BackColor = panel;
        gbSearch.BackColor = panel;
        gbSearch.ForeColor = text;
        lblStatus.ForeColor = muted;

        gridRecords.BackgroundColor = panel;
        gridRecords.GridColor = border;
        gridRecords.BorderStyle = BorderStyle.FixedSingle;
        gridRecords.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        gridRecords.DefaultCellStyle.BackColor = panel;
        gridRecords.DefaultCellStyle.ForeColor = text;
        gridRecords.DefaultCellStyle.SelectionBackColor = accent;
        gridRecords.DefaultCellStyle.SelectionForeColor = Color.White;
        gridRecords.AlternatingRowsDefaultCellStyle.BackColor = panelAlt;
        gridRecords.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
        gridRecords.ColumnHeadersDefaultCellStyle.BackColor = header;
        gridRecords.ColumnHeadersDefaultCellStyle.ForeColor = text;
        gridRecords.ColumnHeadersDefaultCellStyle.SelectionBackColor = header;
        gridRecords.ColumnHeadersDefaultCellStyle.SelectionForeColor = text;
    }

    private void splitMain_SplitterMoved(object? sender, SplitterEventArgs e)
    {
        if (!_splitterInitialized)
        {
            return;
        }

        _splitterUserAdjusted = true;
    }

    private void splitMain_SizeChanged(object? sender, EventArgs e)
    {
        if (_splitterUserAdjusted)
        {
            return;
        }

        ApplyTableFocusedSplitRatio();
    }

    private void ApplyTableFocusedSplitRatio()
    {
        if (splitMain.Width <= splitMain.Panel1MinSize + splitMain.Panel2MinSize)
        {
            return;
        }

        var target = (int)(splitMain.Width * 0.72);
        var min = splitMain.Panel1MinSize;
        var max = splitMain.Width - splitMain.Panel2MinSize;
        splitMain.SplitterDistance = Math.Clamp(target, min, max);
        _splitterInitialized = true;
    }

    private static void TryEnableDoubleBuffering(DataGridView grid)
    {
        // DataGridView.DoubleBuffered is protected - enable via reflection for smoother scrolling/repaint.
        var property = typeof(DataGridView).GetProperty(
            "DoubleBuffered",
            BindingFlags.Instance | BindingFlags.NonPublic);

        property?.SetValue(grid, true);
    }

    private BrowserRow? GetSelectedRow()
    {
        if (gridRecords.CurrentCell is null)
        {
            return null;
        }

        var rowIndex = gridRecords.CurrentCell.RowIndex;
        if (rowIndex < 0 || rowIndex >= _rows.Count)
        {
            return null;
        }

        return _rows[rowIndex];
    }

    private async void txtSearch_TextChanged(object sender, EventArgs e)
    {
        _debounceCts?.Cancel();
        _debounceCts?.Dispose();
        _debounceCts = new CancellationTokenSource();

        try
        {
            await Task.Delay(_debounceMs, _debounceCts.Token);
            if (chkRealtime.Checked)
            {
                FilterRequested?.Invoke(this, EventArgs.Empty);
            }
        }
        catch (OperationCanceledException)
        {
            // Ignore stale text changes.
        }
    }

    private void btnSearch_Click(object sender, EventArgs e)
    {
        FilterRequested?.Invoke(this, EventArgs.Empty);
    }

    private void btnLoadAll_Click(object sender, EventArgs e)
    {
        LoadAllRequested?.Invoke(this, EventArgs.Empty);
    }

    private void gridRecords_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
    {
        if (e.RowIndex < 0 || e.RowIndex >= _rows.Count)
        {
            return;
        }

        var row = _rows[e.RowIndex];
        if (e.ColumnIndex < 0 || e.ColumnIndex >= row.Values.Length)
        {
            return;
        }

        e.Value = row.Values[e.ColumnIndex];
    }

    private void gridRecords_SelectionChanged(object sender, EventArgs e)
    {
        SelectedRowChanged?.Invoke(this, GetSelectedRow());
    }

    private void rbSearchBy_CheckedChanged(object sender, EventArgs e)
    {
        if (chkRealtime.Checked)
        {
            FilterRequested?.Invoke(this, EventArgs.Empty);
        }
    }

    private void gridRecords_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
        {
            var value = gridRecords.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            if (value != null && !string.IsNullOrWhiteSpace(value.ToString()))
            {
                Clipboard.SetText(value.ToString()!);
            }
        }
    }
}
