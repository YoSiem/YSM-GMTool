using System.Globalization;
using App.Core.Interfaces;
using App.WinForms.Controls;
using App.WinForms.Models;

namespace App.WinForms.Presenters;

public sealed class EntityBrowserPresenter<TRecord>
{
    private readonly EntityBrowserControl _view;
    private readonly Func<CancellationToken, Task<IReadOnlyList<TRecord>>> _loadAllAsync;
    private readonly Func<TRecord, int> _idSelector;
    private readonly Func<TRecord, string?> _nameSelector;
    private readonly Func<TRecord, IEnumerable<string?>>? _searchableTextSelector;
    private readonly Func<TRecord, string?>? _secondarySearchTextSelector;
    private readonly Func<TRecord, object?[]> _rowValuesSelector;
    private readonly INameNormalizer _normalizer;

    // When provided, typing in the search box triggers a SQL-level query instead of in-memory filtering.
    private readonly Func<string, SearchMode, CancellationToken, Task<IReadOnlyList<TRecord>>>? _sqlSearchAsync;
    private readonly Func<int?>? _maxRowsSelector;

    private List<BrowserRow> _allRows = [];
    private List<SearchIndexedRecord<TRecord>> _index = [];
    private string? _lastFilterQuery;
    private SearchMode _lastFilterMode = SearchMode.ByName;
    private int _dataVersion;
    private int _lastFilteredVersion = -1;

    private CancellationTokenSource? _loadCts;
    private CancellationTokenSource? _filterCts;

    public EntityBrowserPresenter(
        EntityBrowserControl view,
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
        _view = view;
        _loadAllAsync = loadAllAsync;
        _idSelector = idSelector;
        _nameSelector = nameSelector;
        _rowValuesSelector = rowValuesSelector;
        _normalizer = normalizer;
        _searchableTextSelector = searchableTextSelector;
        _secondarySearchTextSelector = secondarySearchTextSelector;
        _sqlSearchAsync = sqlSearchAsync;
        _maxRowsSelector = maxRowsSelector;

        _view.LoadAllRequested += OnLoadAllRequested;
        _view.FilterRequested += OnFilterRequested;
        _view.SelectedRowChanged += OnSelectedRowChanged;
    }

    public event EventHandler<TRecord?>? SelectedRecordChanged;

    public event EventHandler<Exception>? ErrorOccurred;

    public TRecord? SelectedRecord { get; private set; }

    private async void OnLoadAllRequested(object? sender, EventArgs e)
    {
        await LoadAllAsync();
    }

    private async void OnFilterRequested(object? sender, EventArgs e)
    {
        await ApplyFilterAsync(_view.SearchText, _view.CurrentSearchMode);
    }

    private void OnSelectedRowChanged(object? sender, BrowserRow? row)
    {
        SelectedRecord = row?.Tag is TRecord typed ? typed : default;
        SelectedRecordChanged?.Invoke(this, SelectedRecord);
    }

    private async Task LoadAllAsync()
    {
        _loadCts?.Cancel();
        _loadCts?.Dispose();
        _loadCts = new CancellationTokenSource();

        try
        {
            _view.SetStatus("Loading data from database...");
            var records = await _loadAllAsync(_loadCts.Token);
            _index = BuildIndex(records);
            _allRows = _index.Select(x => x.Row).ToList();
            _dataVersion++;
            _lastFilterQuery = null;

            await ApplyFilterAsync(_view.SearchText, _view.CurrentSearchMode);
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
                    _view.SetRows([]);
                    _view.SetStatus("Enter a search term and press Search.");
                    return;
                }

                _view.SetStatus("Searching...");
                var records = await _sqlSearchAsync(trimmedQuery, mode, token);
                _index = BuildIndex(records);
                _allRows = _index.Select(x => x.Row).ToList();
                _dataVersion++;

                var resultRows = (IEnumerable<BrowserRow>)_allRows;
                var maxRows = _maxRowsSelector?.Invoke();
                if (maxRows.HasValue && maxRows.Value > 0)
                {
                    resultRows = resultRows.Take(maxRows.Value);
                }

                var finalRows = resultRows.ToList();
                _view.SetRows(finalRows);
                _view.SetStatus($"Found {_allRows.Count.ToString("N0", CultureInfo.InvariantCulture)} record(s). Showing {finalRows.Count.ToString("N0", CultureInfo.InvariantCulture)}.");
            }
            catch (Exception ex) when (ex is OperationCanceledException || ex.Message.Contains("Operation cancelled by user.", StringComparison.OrdinalIgnoreCase))
            {
                // Ignore stale filter operations. Some SQL providers surface this as a SqlException instead of OperationCanceledException
            }
            catch (Exception ex)
            {
                ErrorOccurred?.Invoke(this, ex);
            }

            return;
        }

        // In-memory filter path (existing logic).
        if (_index.Count == 0)
        {
            _view.SetRows([]);
            _view.SetStatus("No data loaded. Click Load All.");
            return;
        }

        _filterCts?.Cancel();
        _filterCts?.Dispose();
        _filterCts = new CancellationTokenSource();

        try
        {
            var normalizedQuery = _normalizer.NormalizeForSearch(query);
            var token = _filterCts.Token;

            if (_lastFilteredVersion == _dataVersion
                && string.Equals(_lastFilterQuery, normalizedQuery, StringComparison.Ordinal)
                && _lastFilterMode == mode)
            {
                return;
            }

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
                        .WithCancellation(token)
                        .Where(indexed => mode switch
                        {
                            SearchMode.ById => indexed.NormalizedId.Contains(normalizedQuery, StringComparison.Ordinal),
                            SearchMode.ByContactScript => indexed.NormalizedSecondarySearchText.Contains(normalizedQuery, StringComparison.Ordinal),
                            _ => indexed.NormalizedSearchText.Contains(normalizedQuery, StringComparison.Ordinal)
                        })
                        .Select(x => x.Row);
                }

                var maxRows = _maxRowsSelector?.Invoke();
                if (maxRows.HasValue && maxRows.Value > 0)
                {
                    resultList = resultList.Take(maxRows.Value);
                }

                return (IReadOnlyList<BrowserRow>)resultList.ToList();
            }, token);

            _view.SetRows(rows);
            _view.SetStatus($"Loaded {_allRows.Count.ToString("N0", CultureInfo.InvariantCulture)} records. Showing {rows.Count.ToString("N0", CultureInfo.InvariantCulture)}.");
            _lastFilterQuery = normalizedQuery;
            _lastFilterMode = mode;
            _lastFilteredVersion = _dataVersion;
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

    private List<SearchIndexedRecord<TRecord>> BuildIndex(IEnumerable<TRecord> records)
    {
        var index = new List<SearchIndexedRecord<TRecord>>();

        foreach (var record in records)
        {
            var row = new BrowserRow(record!, _rowValuesSelector(record));
            var searchableTextParts = (_searchableTextSelector?.Invoke(record) ?? [_nameSelector(record)])
                .Where(x => !string.IsNullOrWhiteSpace(x));
            var searchableText = string.Join(' ', searchableTextParts);
            var secondaryText = _secondarySearchTextSelector?.Invoke(record);

            index.Add(new SearchIndexedRecord<TRecord>(
                record,
                _normalizer.NormalizeForSearch(_idSelector(record).ToString(CultureInfo.InvariantCulture), removeDiacritics: false),
                _normalizer.NormalizeForSearch(searchableText),
                _normalizer.NormalizeForSearch(secondaryText),
                row));
        }

        return index;
    }
}
