using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using App.Core.Abstractions;
using ReactiveUI;

namespace App.Desktop.ViewModels;

/// <summary>
/// Non-generic surface the <c>EntityBrowserView</c> uses to build columns, sort, and drive the
/// search mode without knowing the concrete record type.
/// </summary>
public interface IEntityBrowser
{
    IReadOnlyList<BrowserColumn> Columns { get; }

    SearchMode SearchMode { get; set; }

    void SortByColumn(int columnIndex);
}

/// <summary>
/// Generic, reusable browser view model: load-all / search / sort / row-limit over a list of records.
/// In-memory PLINQ filter, optional SQL search path, ID-range search, exact status strings,
/// provider-agnostic cancellation guard.
/// </summary>
public sealed class EntityBrowserViewModel<TRecord> : ReactiveObject, IEntityBrowser
{
    private static readonly Regex IdRangeRegex = new(
        @"^\s*(?<from>\d+)\s*-\s*(?<to>\d+)\s*$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

    private readonly Func<CancellationToken, Task<IReadOnlyList<TRecord>>> _loadAllAsync;
    private readonly Func<TRecord, int> _idSelector;
    private readonly Func<TRecord, string?> _nameSelector;
    private readonly Func<TRecord, object?[]> _rowValuesSelector;
    private readonly INameNormalizer _normalizer;
    private readonly Func<TRecord, IEnumerable<string?>>? _searchableTextSelector;
    private readonly Func<TRecord, string?>? _secondarySearchTextSelector;
    private readonly Func<string, SearchMode, CancellationToken, Task<IReadOnlyList<TRecord>>>? _sqlSearchAsync;
    private readonly Func<int?>? _maxRowsSelector;

    private List<SearchIndexedRecord> _index = [];
    private List<BrowserRow> _allRows = [];

    private CancellationTokenSource? _loadCts;
    private CancellationTokenSource? _filterCts;

    private int _sortColumnIndex = -1;
    private bool _sortAscending = true;

    private string _searchText = string.Empty;
    private SearchMode _searchMode = SearchMode.ByName;
    private bool _realtimeEnabled;
    private string _status = "No data loaded. Click Load All.";
    private BrowserRow? _selectedRow;
    private TRecord? _selectedRecord;

    public EntityBrowserViewModel(
        Func<CancellationToken, Task<IReadOnlyList<TRecord>>> loadAllAsync,
        Func<TRecord, int> idSelector,
        Func<TRecord, string?> nameSelector,
        Func<TRecord, object?[]> rowValuesSelector,
        INameNormalizer normalizer,
        Func<TRecord, IEnumerable<string?>>? searchableTextSelector = null,
        Func<TRecord, string?>? secondarySearchTextSelector = null,
        Func<string, SearchMode, CancellationToken, Task<IReadOnlyList<TRecord>>>? sqlSearchAsync = null,
        Func<int?>? maxRowsSelector = null)
    {
        _loadAllAsync = loadAllAsync;
        _idSelector = idSelector;
        _nameSelector = nameSelector;
        _rowValuesSelector = rowValuesSelector;
        _normalizer = normalizer;
        _searchableTextSelector = searchableTextSelector;
        _secondarySearchTextSelector = secondarySearchTextSelector;
        _sqlSearchAsync = sqlSearchAsync;
        _maxRowsSelector = maxRowsSelector;

        LoadAll = ReactiveCommand.CreateFromTask(() => LoadAllAsync());
        Filter = ReactiveCommand.CreateFromTask(() => ApplyFilterAsync(SearchText, SearchMode));

        LoadAll.ThrownExceptions.Subscribe(ex => ErrorOccurred?.Invoke(this, ex));
        Filter.ThrownExceptions.Subscribe(ex => ErrorOccurred?.Invoke(this, ex));

        // Debounced realtime filtering: only fires when RealtimeEnabled.
        this.WhenAnyValue(x => x.SearchText)
            .Throttle(TimeSpan.FromMilliseconds(DebounceMs), RxApp.MainThreadScheduler)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Where(_ => RealtimeEnabled)
            .Subscribe(_ => Filter.Execute().Subscribe());

        // Keep SelectedRecord in sync with SelectedRow.
        this.WhenAnyValue(x => x.SelectedRow)
            .Subscribe(row => SelectedRecord = row?.Tag is TRecord typed ? typed : default);
    }

    /// <summary>Raised with the underlying exception when a load/filter operation fails.</summary>
    public event EventHandler<Exception>? ErrorOccurred;

    // --- Configuration (set by the owning tab VM after construction). ---

    public IReadOnlyList<BrowserColumn> Columns { get; set; } = [];

    public string ByIdLabel { get; set; } = "Search by ID";

    public string ByNameLabel { get; set; } = "Search by Name";

    public bool IdSearchEnabled { get; set; } = true;

    public bool SecondarySearchEnabled { get; set; }

    public string SecondaryLabel { get; set; } = "Search by Contact script";

    public bool RealtimeVisible { get; set; } = true;

    public bool LoadAllVisible { get; set; } = true;

    public int DebounceMs { get; set; } = 250;

    // --- Observable state. ---

    public string SearchText
    {
        get => _searchText;
        set => this.RaiseAndSetIfChanged(ref _searchText, value);
    }

    public SearchMode SearchMode
    {
        get => _searchMode;
        set => this.RaiseAndSetIfChanged(ref _searchMode, value);
    }

    public bool RealtimeEnabled
    {
        get => _realtimeEnabled;
        set => this.RaiseAndSetIfChanged(ref _realtimeEnabled, value);
    }

    public string Status
    {
        get => _status;
        set => this.RaiseAndSetIfChanged(ref _status, value);
    }

    public ObservableCollection<BrowserRow> Rows { get; } = [];

    public BrowserRow? SelectedRow
    {
        get => _selectedRow;
        set => this.RaiseAndSetIfChanged(ref _selectedRow, value);
    }

    public TRecord? SelectedRecord
    {
        get => _selectedRecord;
        private set => this.RaiseAndSetIfChanged(ref _selectedRecord, value);
    }

    public ReactiveCommand<Unit, Unit> LoadAll { get; }

    public ReactiveCommand<Unit, Unit> Filter { get; }

    /// <summary>Emits whenever the selected record changes (including to null).</summary>
    public IObservable<TRecord?> WhenSelectedRecordChanged => this.WhenAnyValue(x => x.SelectedRecord);

    /// <summary>Loads records from an external source (e.g. a one-off bulk query) and shows them directly.</summary>
    public async Task LoadExternalAsync(Func<CancellationToken, Task<IReadOnlyList<TRecord>>> loader)
    {
        _loadCts?.Cancel();
        _loadCts?.Dispose();
        _loadCts = new CancellationTokenSource();
        var token = _loadCts.Token;

        try
        {
            Status = "Loading data from database...";
            var records = await loader(token);
            _index = BuildIndex(records);
            _allRows = _index.Select(x => x.Row).ToList();
            var finalRows = ApplyRowLimit(_allRows);
            SetRows(finalRows);
            Status = $"Loaded {Format(_allRows.Count)} record(s). Showing {Format(finalRows.Count)}.";
        }
        catch (OperationCanceledException)
        {
            // Ignore stale load operations.
        }
        catch (Exception ex)
        {
            ErrorOccurred?.Invoke(this, ex);
        }
    }

    /// <summary>
    /// Startup auto-load: same as the <see cref="LoadAll"/> command, but a failure only updates
    /// <see cref="Status"/> instead of raising <see cref="ErrorOccurred"/>. This keeps an unconfigured
    /// or unreachable database from popping an error dialog for every tab when the app starts.
    /// </summary>
    public Task AutoLoadAsync() => LoadAllAsync(silent: true);

    /// <summary>Re-sorts the current rows by the given column (toggles asc/desc on repeat clicks).</summary>
    public void SortByColumn(int columnIndex)
    {
        if (Rows.Count == 0)
        {
            return;
        }

        if (columnIndex >= 0 && columnIndex < Columns.Count && Columns[columnIndex].IsImage)
        {
            return;
        }

        if (_sortColumnIndex == columnIndex)
        {
            _sortAscending = !_sortAscending;
        }
        else
        {
            _sortColumnIndex = columnIndex;
            _sortAscending = true;
        }

        var ascending = _sortAscending;
        var sorted = Rows.ToList();
        sorted.Sort((a, b) =>
        {
            var aVal = columnIndex < a.Values.Length ? a.Values[columnIndex]?.ToString() ?? string.Empty : string.Empty;
            var bVal = columnIndex < b.Values.Length ? b.Values[columnIndex]?.ToString() ?? string.Empty : string.Empty;

            if (double.TryParse(aVal, NumberStyles.Any, CultureInfo.InvariantCulture, out var aNum)
                && double.TryParse(bVal, NumberStyles.Any, CultureInfo.InvariantCulture, out var bNum))
            {
                return ascending ? aNum.CompareTo(bNum) : bNum.CompareTo(aNum);
            }

            var cmp = string.Compare(aVal, bVal, StringComparison.OrdinalIgnoreCase);
            return ascending ? cmp : -cmp;
        });

        Rows.Clear();
        foreach (var row in sorted)
        {
            Rows.Add(row);
        }

        SelectedRow = null;
    }

    private async Task LoadAllAsync(bool silent = false)
    {
        _loadCts?.Cancel();
        _loadCts?.Dispose();
        _loadCts = new CancellationTokenSource();

        try
        {
            Status = "Loading data from database...";
            var records = await _loadAllAsync(_loadCts.Token);
            _index = BuildIndex(records);
            _allRows = _index.Select(x => x.Row).ToList();

            await ApplyFilterAsync(SearchText, SearchMode);
        }
        catch (OperationCanceledException)
        {
            // Ignore stale load operations.
        }
        catch (Exception ex) when (silent)
        {
            // Auto-load: surface the failure in the status line only (no dialog), so an
            // unconfigured/unreachable DB doesn't pop a modal per tab at startup.
            Status = $"Auto-load failed: {ex.Message} Click Load All to retry.";
        }
        catch (Exception ex)
        {
            ErrorOccurred?.Invoke(this, ex);
        }
    }

    private async Task ApplyFilterAsync(string query, SearchMode mode)
    {
        // SQL-level search path: bypass the in-memory index entirely.
        if (_sqlSearchAsync != null)
        {
            _filterCts?.Cancel();
            _filterCts?.Dispose();
            _filterCts = new CancellationTokenSource();

            try
            {
                var token = _filterCts.Token;
                var trimmedQuery = query.Trim();

                if (string.IsNullOrWhiteSpace(trimmedQuery))
                {
                    SetRows([]);
                    Status = "Enter a search term and press Search.";
                    return;
                }

                Status = "Searching...";
                var records = await _sqlSearchAsync(trimmedQuery, mode, token);
                _index = BuildIndex(records);
                _allRows = _index.Select(x => x.Row).ToList();
                var finalRows = ApplyRowLimit(_allRows);
                SetRows(finalRows);
                Status = $"Found {Format(_allRows.Count)} record(s). Showing {Format(finalRows.Count)}.";
            }
            catch (Exception ex) when (ex is OperationCanceledException
                || ex.Message.Contains("Operation cancelled by user.", StringComparison.OrdinalIgnoreCase))
            {
                // Ignore stale filter operations. Some SQL providers surface this as a SqlException.
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, ex);
            }

            return;
        }

        // In-memory filter path.
        if (_index.Count == 0)
        {
            SetRows([]);
            Status = "No data loaded. Click Load All.";
            return;
        }

        _filterCts?.Cancel();
        _filterCts?.Dispose();
        _filterCts = new CancellationTokenSource();

        try
        {
            var trimmedQuery = query.Trim();
            var normalizedQuery = _normalizer.NormalizeForSearch(query);
            var token = _filterCts.Token;
            var from = 0;
            var to = 0;
            var hasIdRange = mode == SearchMode.ById && TryParseIdRange(trimmedQuery, out from, out to);

            var rows = await Task.Run(() =>
            {
                token.ThrowIfCancellationRequested();

                IEnumerable<BrowserRow> resultList;
                if (string.IsNullOrWhiteSpace(normalizedQuery))
                {
                    resultList = _allRows;
                }
                else
                {
                    resultList = _index
                        .AsParallel()
                        .AsOrdered()
                        .WithCancellation(token)
                        .Where(indexed => mode switch
                        {
                            SearchMode.ById when hasIdRange => IsInRange(_idSelector(indexed.Item), from, to),
                            SearchMode.ById => indexed.NormalizedId.Contains(normalizedQuery, StringComparison.Ordinal),
                            SearchMode.ByContactScript => indexed.NormalizedSecondarySearchText.Contains(normalizedQuery, StringComparison.Ordinal),
                            _ => indexed.NormalizedSearchText.Contains(normalizedQuery, StringComparison.Ordinal),
                        })
                        .Select(x => x.Row);
                }

                return ApplyRowLimit(resultList);
            }, token);

            SetRows(rows);
            Status = $"Loaded {Format(_allRows.Count)} records. Showing {Format(rows.Count)}.";
        }
        catch (OperationCanceledException)
        {
            // Ignore stale filter operations.
        }
        catch (Exception ex)
        {
            ErrorOccurred?.Invoke(this, ex);
        }
    }

    private List<SearchIndexedRecord> BuildIndex(IEnumerable<TRecord> records)
    {
        var index = new List<SearchIndexedRecord>();

        foreach (var record in records.OrderBy(_idSelector))
        {
            var row = new BrowserRow(record!, _rowValuesSelector(record));
            var searchableTextParts = (_searchableTextSelector?.Invoke(record) ?? [_nameSelector(record)])
                .Where(x => !string.IsNullOrWhiteSpace(x));
            var searchableText = string.Join(' ', searchableTextParts);
            var secondaryText = _secondarySearchTextSelector?.Invoke(record);

            index.Add(new SearchIndexedRecord(
                record,
                _normalizer.NormalizeForSearch(_idSelector(record).ToString(CultureInfo.InvariantCulture), removeDiacritics: false),
                _normalizer.NormalizeForSearch(searchableText),
                _normalizer.NormalizeForSearch(secondaryText),
                row));
        }

        return index;
    }

    private void SetRows(IReadOnlyList<BrowserRow> rows)
    {
        _sortColumnIndex = -1;
        _sortAscending = true;

        Rows.Clear();
        foreach (var row in rows)
        {
            Rows.Add(row);
        }

        SelectedRow = null;
    }

    private IReadOnlyList<BrowserRow> ApplyRowLimit(IEnumerable<BrowserRow> rows)
    {
        var maxRows = _maxRowsSelector?.Invoke();
        if (maxRows.HasValue && maxRows.Value > 0)
        {
            return rows.Take(maxRows.Value).ToList();
        }

        return rows.ToList();
    }

    private static bool TryParseIdRange(string query, out int from, out int to)
    {
        from = 0;
        to = 0;

        if (string.IsNullOrWhiteSpace(query))
        {
            return false;
        }

        var match = IdRangeRegex.Match(query);
        if (!match.Success)
        {
            return false;
        }

        if (!int.TryParse(match.Groups["from"].Value, NumberStyles.None, CultureInfo.InvariantCulture, out var parsedFrom)
            || !int.TryParse(match.Groups["to"].Value, NumberStyles.None, CultureInfo.InvariantCulture, out var parsedTo))
        {
            return false;
        }

        from = Math.Min(parsedFrom, parsedTo);
        to = Math.Max(parsedFrom, parsedTo);
        return true;
    }

    private static bool IsInRange(int value, int from, int to) => value >= from && value <= to;

    private static string Format(int count) => count.ToString("N0", CultureInfo.InvariantCulture);

    private readonly record struct SearchIndexedRecord(
        TRecord Item,
        string NormalizedId,
        string NormalizedSearchText,
        string NormalizedSecondarySearchText,
        BrowserRow Row);
}
