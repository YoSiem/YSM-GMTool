using App.Core.Models.Entities;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace App.Desktop.Features.RandomOption;

public partial class ItemEffectPickerWindow : Window
{
    public ItemEffectPickerWindow()
    {
        InitializeComponent();
        EffectsGrid.DoubleTapped += (_, _) => Confirm();
    }

    private void OnSelectClick(object? sender, RoutedEventArgs e) => Confirm();

    private void OnCancelClick(object? sender, RoutedEventArgs e) => Close(null);

    private void Confirm()
    {
        if (DataContext is ItemEffectPickerViewModel { Selected: { } record })
        {
            Close(record);
        }
    }
}
