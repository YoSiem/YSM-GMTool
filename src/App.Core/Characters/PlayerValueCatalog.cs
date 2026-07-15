namespace App.Core.Characters;

/// <summary>
/// Curated list of common player value keys for the Character tab's Set/Add builder. Keys map to
/// <c>StructPlayer::BindProperty</c> bound properties on the server; the UI's free-text "custom
/// key" field covers any other bound key (e.g. <c>jlv</c>, <c>immoral</c>, <c>pk_count</c>).
/// </summary>
public static class PlayerValueCatalog
{
    public static IReadOnlyList<PlayerValueAttribute> Attributes { get; } =
    [
        new("level", "Level"),
        new("exp", "EXP"),
        new("jp", "JP (Job Points)"),
        new("tp", "TP (Talent Points)"),
        new("gold", "Gold"),
        new("hp", "HP"),
        new("mp", "MP"),
        new("str", "STR"),
        new("agi", "AGI"),
        new("dex", "DEX"),
        new("int", "INT"),
        new("luck", "LUCK"),
        new("vital", "VITAL"),
        new("mental", "MENTAL"),
        new("free_statpoints", "Free stat points"),
        new("charisma", "Charisma"),
    ];
}
