using App.Core.Models.Entities;

namespace App.Desktop.Infrastructure;

/// <summary>Opens a modal item-effect picker and returns the chosen effect (or null if cancelled).</summary>
public interface IItemEffectPickerService
{
    Task<ItemEffectRecord?> PickAsync(CancellationToken cancellationToken = default);
}
