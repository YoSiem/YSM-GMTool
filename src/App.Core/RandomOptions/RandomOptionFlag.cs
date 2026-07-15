namespace App.Core.RandomOptions;

/// <summary>One selectable stat bitflag: its bit position and display label.</summary>
public readonly record struct RandomOptionFlag(int Bit, string Label);

/// <summary>One equippable wear slot: its slot index (emitted in the command) and display label.</summary>
public readonly record struct RandomOptionEquippart(int Index, string Label);
