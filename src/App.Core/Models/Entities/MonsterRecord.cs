namespace App.Core.Models.Entities;

public sealed class MonsterRecord
{
    public int Id { get; init; }

    public string Name { get; init; } = string.Empty;

    public int? Level { get; init; }
}
