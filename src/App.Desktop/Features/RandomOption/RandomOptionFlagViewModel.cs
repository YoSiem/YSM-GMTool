using System;
using ReactiveUI;

namespace App.Desktop.Features.RandomOption;

/// <summary>One checkbox in the stat grid. Toggling notifies the owner so the mask recomputes;
/// <see cref="SetCheckedSilently"/> updates the box during import without firing per-box rebuilds.</summary>
public sealed class RandomOptionFlagViewModel : ReactiveObject
{
    private readonly Action _onToggled;
    private bool _isChecked;
    private bool _suppress;

    public RandomOptionFlagViewModel(int bit, string label, Action onToggled)
    {
        Bit = bit;
        Label = label;
        _onToggled = onToggled;
    }

    public int Bit { get; }

    public string Label { get; }

    public bool IsChecked
    {
        get => _isChecked;
        set
        {
            this.RaiseAndSetIfChanged(ref _isChecked, value);
            if (!_suppress)
            {
                _onToggled();
            }
        }
    }

    public void SetCheckedSilently(bool value)
    {
        _suppress = true;
        IsChecked = value;
        _suppress = false;
    }
}
