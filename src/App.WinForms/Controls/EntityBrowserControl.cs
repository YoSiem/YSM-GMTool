using App.WinForms.Models;
using System.Reflection;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Drawing2D;

namespace App.WinForms.Controls;

public partial class EntityBrowserControl : UserControl
{
    private static readonly string[] IconExtensions = [".png", ".jpg", ".jpeg", ".bmp", ".gif", ".webp"];

    private List<BrowserRow> _rows = [];
    private int _sortColumnIndex = -1;
    private SortOrder _sortOrder = SortOrder.None;
    private readonly Dictionary<int, int> _imageColumnSizes = [];
    private readonly Dictionary<string, Image?> _iconCache = new(StringComparer.OrdinalIgnoreCase);
    private string? _iconsRootPath;
    private bool _iconsEnabled;
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
        Disposed += EntityBrowserControl_Disposed;
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

    public void ConfigureIdSearch(bool enabled)
    {
        rbSearchById.Visible = enabled;
        rbSearchById.Enabled = enabled;
        if (!enabled && rbSearchById.Checked)
        {
            rbSearchByName.Checked = true;
        }
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

    public void ConfigureIconLookup(bool enabled, string? iconsRootPath)
    {
        var normalizedPath = string.IsNullOrWhiteSpace(iconsRootPath)
            ? null
            : iconsRootPath.Trim();

        var configChanged = _iconsEnabled != enabled
            || !string.Equals(_iconsRootPath, normalizedPath, StringComparison.OrdinalIgnoreCase);

        _iconsEnabled = enabled;
        _iconsRootPath = normalizedPath;

        if (!configChanged)
        {
            return;
        }

        ClearIconCache();
        gridRecords.Invalidate();
    }

    public void ConfigureColumns(IReadOnlyList<BrowserColumnDefinition> columns)
    {
        gridRecords.SuspendLayout();
        gridRecords.Columns.Clear();
        _imageColumnSizes.Clear();
        gridRecords.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        foreach (var column in columns)
        {
            if (column.IsImage)
            {
                var imageColumn = new DataGridViewImageColumn
                {
                    Name = column.Name,
                    HeaderText = column.HeaderText,
                    Width = column.Width,
                    ReadOnly = true,
                    SortMode = DataGridViewColumnSortMode.NotSortable,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                    MinimumWidth = column.Width,
                    ImageLayout = DataGridViewImageCellLayout.Normal
                };
                imageColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                imageColumn.DefaultCellStyle.NullValue = null;

                var imageColumnIndex = gridRecords.Columns.Add(imageColumn);
                _imageColumnSizes[imageColumnIndex] = Math.Max(8, column.ImageSize);
                continue;
            }

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
                SortMode = DataGridViewColumnSortMode.Programmatic,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = fillWeight,
                MinimumWidth = minimumWidth
            };

            gridRecords.Columns.Add(dataGridColumn);
        }

        var maxImageSize = _imageColumnSizes.Count == 0 ? 0 : _imageColumnSizes.Values.Max();
        gridRecords.RowTemplate.Height = maxImageSize > 0
            ? Math.Max(26, maxImageSize + 8)
            : 28;

        gridRecords.ResumeLayout();
    }

    public void SetRows(IReadOnlyList<BrowserRow> rows)
    {
        _rows = rows.ToList();
        _sortColumnIndex = -1;
        _sortOrder = SortOrder.None;

        foreach (DataGridViewColumn col in gridRecords.Columns)
        {
            col.HeaderCell.SortGlyphDirection = SortOrder.None;
        }

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
        gridRecords.MultiSelect = true;
        gridRecords.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        gridRecords.ReadOnly = true;
        gridRecords.RowHeadersVisible = false;
        gridRecords.AllowUserToResizeRows = false;
        gridRecords.RowTemplate.Resizable = DataGridViewTriState.False;
        gridRecords.RowTemplate.Height = 28;
        gridRecords.AutoGenerateColumns = false;
        gridRecords.EnableHeadersVisualStyles = false;
        gridRecords.ColumnHeadersDefaultCellStyle.SelectionBackColor = gridRecords.ColumnHeadersDefaultCellStyle.BackColor;
        gridRecords.ColumnHeadersDefaultCellStyle.SelectionForeColor = gridRecords.ColumnHeadersDefaultCellStyle.ForeColor;
        gridRecords.AllowUserToOrderColumns = false;
        gridRecords.AllowUserToResizeColumns = false;
        gridRecords.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
        gridRecords.CellDoubleClick += gridRecords_CellDoubleClick;
        gridRecords.ColumnHeaderMouseClick += gridRecords_ColumnHeaderMouseClick;
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

        if (_imageColumnSizes.TryGetValue(e.ColumnIndex, out var iconSize))
        {
            var iconKey = row.Values[e.ColumnIndex]?.ToString();
            e.Value = ResolveIcon(iconKey, iconSize);
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
            if (_imageColumnSizes.ContainsKey(e.ColumnIndex))
            {
                return;
            }

            var value = gridRecords.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            if (value != null && !string.IsNullOrWhiteSpace(value.ToString()))
            {
                Clipboard.SetText(value.ToString()!);
            }
        }
    }

    private void gridRecords_ColumnHeaderMouseClick(object? sender, DataGridViewCellMouseEventArgs e)
    {
        if (_rows.Count == 0)
        {
            return;
        }

        var columnIndex = e.ColumnIndex;
        if (_imageColumnSizes.ContainsKey(columnIndex))
        {
            return;
        }

        if (_sortColumnIndex == columnIndex)
        {
            _sortOrder = _sortOrder == SortOrder.Ascending ? SortOrder.Descending : SortOrder.Ascending;
        }
        else
        {
            _sortColumnIndex = columnIndex;
            _sortOrder = SortOrder.Ascending;
        }

        var ascending = _sortOrder == SortOrder.Ascending;
        _rows.Sort((a, b) =>
        {
            var aVal = columnIndex < a.Values.Length ? a.Values[columnIndex]?.ToString() ?? string.Empty : string.Empty;
            var bVal = columnIndex < b.Values.Length ? b.Values[columnIndex]?.ToString() ?? string.Empty : string.Empty;

            if (double.TryParse(aVal, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var aNum)
                && double.TryParse(bVal, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var bNum))
            {
                return ascending ? aNum.CompareTo(bNum) : bNum.CompareTo(aNum);
            }

            var cmp = string.Compare(aVal, bVal, StringComparison.OrdinalIgnoreCase);
            return ascending ? cmp : -cmp;
        });

        foreach (DataGridViewColumn col in gridRecords.Columns)
        {
            col.HeaderCell.SortGlyphDirection = col.Index == columnIndex ? _sortOrder : SortOrder.None;
        }

        gridRecords.CurrentCell = null;
        gridRecords.ClearSelection();
        gridRecords.Invalidate();

        SelectedRowChanged?.Invoke(this, null);
    }

    private void EntityBrowserControl_Disposed(object? sender, EventArgs e)
    {
        _debounceCts?.Cancel();
        _debounceCts?.Dispose();
        _debounceCts = null;

        ClearIconCache();
    }

    private void ClearIconCache()
    {
        foreach (var image in _iconCache.Values)
        {
            image?.Dispose();
        }

        _iconCache.Clear();
    }

    private Image? ResolveIcon(string? iconKey, int iconSize)
    {
        if (!_iconsEnabled
            || string.IsNullOrWhiteSpace(_iconsRootPath)
            || string.IsNullOrWhiteSpace(iconKey))
        {
            return null;
        }

        var normalizedKey = iconKey.Trim();
        var cacheKey = $"{iconSize}:{normalizedKey}";
        if (_iconCache.TryGetValue(cacheKey, out var cached))
        {
            return cached;
        }

        var path = ResolveIconFilePath(normalizedKey);
        if (string.IsNullOrWhiteSpace(path))
        {
            _iconCache[cacheKey] = null;
            return null;
        }

        try
        {
            using var source = Image.FromFile(path);
            var resized = ResizeWithLetterboxing(source, iconSize);
            _iconCache[cacheKey] = resized;
            return resized;
        }
        catch
        {
            _iconCache[cacheKey] = null;
            return null;
        }
    }

    private string? ResolveIconFilePath(string iconKey)
    {
        if (string.IsNullOrWhiteSpace(_iconsRootPath) || !Directory.Exists(_iconsRootPath))
        {
            return null;
        }

        if (Path.IsPathRooted(iconKey))
        {
            return File.Exists(iconKey) ? iconKey : null;
        }

        if (Path.HasExtension(iconKey))
        {
            var direct = Path.Combine(_iconsRootPath, iconKey);
            return File.Exists(direct) ? direct : null;
        }

        foreach (var extension in IconExtensions)
        {
            var candidate = Path.Combine(_iconsRootPath, iconKey + extension);
            if (File.Exists(candidate))
            {
                return candidate;
            }
        }

        return null;
    }

    private static Image ResizeWithLetterboxing(Image source, int iconSize)
    {
        var bitmap = new Bitmap(iconSize, iconSize);

        using var graphics = Graphics.FromImage(bitmap);
        graphics.Clear(Color.Transparent);
        graphics.CompositingQuality = CompositingQuality.HighQuality;
        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        graphics.SmoothingMode = SmoothingMode.HighQuality;
        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

        var fitRect = GetFitRectangle(source.Width, source.Height, iconSize, iconSize);
        graphics.DrawImage(source, fitRect);
        return bitmap;
    }

    private static Rectangle GetFitRectangle(int sourceWidth, int sourceHeight, int targetWidth, int targetHeight)
    {
        if (sourceWidth <= 0 || sourceHeight <= 0)
        {
            return new Rectangle(0, 0, targetWidth, targetHeight);
        }

        // Keep icons crisp: never upscale above native resolution.
        var scale = Math.Min(1d, Math.Min((double)targetWidth / sourceWidth, (double)targetHeight / sourceHeight));
        var width = Math.Max(1, (int)Math.Round(sourceWidth * scale));
        var height = Math.Max(1, (int)Math.Round(sourceHeight * scale));
        var x = (targetWidth - width) / 2;
        var y = (targetHeight - height) / 2;

        return new Rectangle(x, y, width, height);
    }
}
