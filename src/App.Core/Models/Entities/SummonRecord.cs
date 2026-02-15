namespace App.Core.Models.Entities;

public sealed class SummonRecord
{
    public int SummonId { get; init; }

    public string SummonName { get; init; } = string.Empty;

    public string? CardName { get; init; }
}
