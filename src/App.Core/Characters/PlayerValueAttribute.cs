namespace App.Core.Characters;

/// <summary>A selectable player attribute for the Character tab: the Lua value key (used by
/// <c>sv</c>/<c>av</c>) and its human-readable UI label.</summary>
public readonly record struct PlayerValueAttribute(string Key, string Label);
