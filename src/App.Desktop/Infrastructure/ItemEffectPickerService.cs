using App.Core.Abstractions;
using App.Core.Models.Entities;
using App.Desktop.Features.RandomOption;
using App.Desktop.Services;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;

namespace App.Desktop.Infrastructure;

/// <summary>Loads item effects from the live DB and shows them in a modal picker window.</summary>
public sealed class ItemEffectPickerService(
    IGameDataRepository repository,
    ConnectionStringResolver connection,
    IDialogService dialog) : IItemEffectPickerService
{
    public async Task<ItemEffectRecord?> PickAsync(CancellationToken cancellationToken = default)
    {
        IReadOnlyList<ItemEffectRecord> effects;
        try
        {
            effects = await repository.GetItemEffectsAsync(
                connection.Provider, connection.Resolve(), connection.Tokens(), cancellationToken);
        }
        catch (Exception ex)
        {
            await dialog.ShowErrorAsync("Item Effect", ex.Message);
            return null;
        }

        var window = new ItemEffectPickerWindow
        {
            DataContext = new ItemEffectPickerViewModel(effects),
        };

        var owner = (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow;
        return owner is not null ? await window.ShowDialog<ItemEffectRecord?>(owner) : null;
    }
}
