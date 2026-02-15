namespace App.Core.Models.Entities;

public sealed class NpcRecord
{
    public int NpcId { get; init; }

    public string NpcTitle { get; init; } = string.Empty;

    public double? X { get; init; }

    public double? Y { get; init; }

    public int? LocalFlag { get; init; }

    public string? ContactScript { get; init; }
}
