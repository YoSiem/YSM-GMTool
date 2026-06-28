using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reactive.Linq;
using App.Core.Models.Entities;
using ReactiveUI;

namespace App.Desktop.Features.RandomOption;

/// <summary>Backs the modal item-effect picker: a filterable list of (Effect_id, Effecttext) rows.</summary>
public sealed class ItemEffectPickerViewModel : ReactiveObject
{
    private readonly IReadOnlyList<ItemEffectRecord> _all;
    private string _searchText = string.Empty;
    private ItemEffectRecord? _selected;

    public ItemEffectPickerViewModel(IReadOnlyList<ItemEffectRecord> effects)
    {
        _all = effects;
        Rows = new ObservableCollection<ItemEffectRecord>(effects);

        this.WhenAnyValue(x => x.SearchText)
            .Throttle(TimeSpan.FromMilliseconds(150), RxApp.MainThreadScheduler)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(_ => ApplyFilter());
    }

    public ObservableCollection<ItemEffectRecord> Rows { get; }

    public string SearchText
    {
        get => _searchText;
        set => this.RaiseAndSetIfChanged(ref _searchText, value);
    }

    public ItemEffectRecord? Selected
    {
        get => _selected;
        set
        {
            this.RaiseAndSetIfChanged(ref _selected, value);
            this.RaisePropertyChanged(nameof(HasSelection));
        }
    }

    public bool HasSelection => _selected is not null;

    private void ApplyFilter()
    {
        var q = SearchText.Trim();
        IEnumerable<ItemEffectRecord> filtered = _all;

        if (!string.IsNullOrWhiteSpace(q))
        {
            filtered = _all.Where(e =>
                e.EffectId.ToString(CultureInfo.InvariantCulture).Contains(q, StringComparison.OrdinalIgnoreCase)
                || (e.EffectText?.Contains(q, StringComparison.OrdinalIgnoreCase) ?? false));
        }

        Rows.Clear();
        foreach (var e in filtered)
        {
            Rows.Add(e);
        }
    }
}
