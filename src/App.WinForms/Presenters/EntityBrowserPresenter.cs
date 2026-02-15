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
    private readonly Func<TRecord, object?[]> _columnsSelector;
    private readonly INameNormalizer _normalizer;

    private List<TRecord> _allRecords = [];
    private List<SearchIndexedRecord<TRecord>> _index = [];

    private CancellationTokenSource? _loadCts;
    private CancellationTokenSource? _filterCts;

    public EntityBrowserPresenter(
        EntityBrowserControl view,
        Func<CancellationToken, Task<IReadOnlyList<TRecord>>> loadAllAsync,
        Func<TRecord, int> idSelector,
        Func<TRecord, string?> nameSelector,
        Func<TRecord, object?[]> columnsSelector,
        INameNormalizer normalizer)
    {
        _view = view;
        _loadAllAsync = loadAllAsync;
        _idSelector = idSelector;
        _nameSelector = nameSelector;
        _columnsSelector = columnsSelector;
        _normalizer = normalizer;

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
            _allRecords = records.ToList();
            _index = BuildIndex(_allRecords);

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
        if (_allRecords.Count == 0)
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

            var filtered = await Task.Run(() =>
            {
                token.ThrowIfCancellationRequested();

                if (string.IsNullOrWhiteSpace(normalizedQuery))
                {
                    return _allRecords;
                }

                var result = new List<TRecord>();

                foreach (var indexed in _index)
                {
                    token.ThrowIfCancellationRequested();

                    var match = mode == SearchMode.ById
                        ? indexed.NormalizedId.Contains(normalizedQuery, StringComparison.Ordinal)
                        : indexed.NormalizedName.Contains(normalizedQuery, StringComparison.Ordinal);

                    if (match)
                    {
                        result.Add(indexed.Item);
                    }
                }

                return result;
            }, token);

            var rows = filtered
                .Select(record => new BrowserRow(record!, _columnsSelector(record)))
                .ToList();

            _view.SetRows(rows);
            _view.SetStatus($"Loaded {_allRecords.Count.ToString("N0", CultureInfo.InvariantCulture)} records. Showing {rows.Count.ToString("N0", CultureInfo.InvariantCulture)}.");
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
            index.Add(new SearchIndexedRecord<TRecord>(
                record,
                _normalizer.NormalizeForSearch(_idSelector(record).ToString(CultureInfo.InvariantCulture), removeDiacritics: false),
                _normalizer.NormalizeForSearch(_nameSelector(record))));
        }

        return index;
    }
}
