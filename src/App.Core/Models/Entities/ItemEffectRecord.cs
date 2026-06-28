namespace App.Core.Models.Entities;

/// <summary>One row of the item-effect picker (ItemEffectResource joined to the string table).</summary>
public sealed class ItemEffectRecord
{
    public int EffectId { get; init; }

    public string EffectText { get; init; } = string.Empty;
}
