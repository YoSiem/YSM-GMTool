namespace App.Core.Models.Entities;

public sealed class PlayerRecord
{
    public int PlayerId { get; init; }

    public string PlayerName { get; init; } = string.Empty;

    public int Level { get; init; }

    public string? JobName { get; init; }
}
