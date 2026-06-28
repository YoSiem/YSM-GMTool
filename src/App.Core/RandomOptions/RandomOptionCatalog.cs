namespace App.Core.RandomOptions;

/// <summary>
/// Static catalogs for the Random Option tab: the Part 1 / Part 2 stat bitflags (ported from the
/// Rappelz FLAG_* / FLAG_ET_* enums) and the equippable wear-slot list. Labels are readable, not
/// the verbatim enum names.
/// </summary>
public static class RandomOptionCatalog
{
    /// <summary>Stat option flags, bits 0..31 (FLAG_* enum).</summary>
    public static readonly IReadOnlyList<RandomOptionFlag> Part1 =
    [
        new(0, "Strength"), new(1, "Vitality"), new(2, "Agility"), new(3, "Dexterity"),
        new(4, "Intelligence"), new(5, "Wisdom"), new(6, "Luck"), new(7, "p.Atk"),
        new(8, "m.Atk"), new(9, "P.Def"), new(10, "M.Def"), new(11, "Atk Speed"),
        new(12, "Cast Speed"), new(13, "Move Speed"), new(14, "Accuracy"), new(15, "m.Acc"),
        new(16, "Crit Chance"), new(17, "Block Chance"), new(18, "Block Def"), new(19, "Evasion"),
        new(20, "m.Res"), new(21, "Max HP"), new(22, "Max MP"), new(23, "Max SP"),
        new(24, "HP Regen"), new(25, "MP Regen"), new(26, "SP Regen"), new(27, "HP Regen Ratio"),
        new(28, "MP Regen Ratio"), new(29, "Final Dmg Increase"), new(30, "Max Weight"),
        new(31, "Final Dmg Reduction"),
    ];

    /// <summary>Elemental / additional option flags (FLAG_ET_* enum). Bit 7 has no flag.</summary>
    public static readonly IReadOnlyList<RandomOptionFlag> Part2 =
    [
        new(0, "None Resist"), new(1, "Fire Resist"), new(2, "Water Resist"), new(3, "Wind Resist"),
        new(4, "Earth Resist"), new(5, "Light Resist"), new(6, "Dark Resist"),
        new(8, "Attack Range"), new(9, "Perfect Block"), new(10, "Ignore P.Def"), new(11, "Ignore M.Def"),
        new(12, "Physical Penetration"), new(13, "Magical Penetration"), new(14, "None Damage"),
        new(15, "Fire Damage"), new(16, "Water Damage"), new(17, "Wind Damage"), new(18, "Earth Damage"),
        new(19, "Light Damage"), new(20, "Dark Damage"), new(21, "None Add. Damage"), new(22, "Fire Add. Damage"),
        new(23, "Water Add. Damage"), new(24, "Wind Add. Damage"), new(25, "Earth Add. Damage"),
        new(26, "Light Add. Damage"), new(27, "Dark Add. Damage"), new(28, "Crit Damage"),
        new(29, "HP Regen Stop"), new(30, "MP Regen Stop"),
    ];

    /// <summary>Equippable wear slots (readable name -> slot index emitted in the command).</summary>
    public static readonly IReadOnlyList<RandomOptionEquippart> Equipparts =
    [
        new(0, "Weapon"), new(1, "Shield"), new(2, "Armor"), new(3, "Helmet"), new(4, "Gloves"),
        new(5, "Boots"), new(6, "Belt"), new(7, "Mantle"), new(8, "Amulet"), new(9, "Ring"),
        new(10, "Second Ring"), new(11, "Earring"), new(12, "Face"), new(13, "Backpack"),
        new(14, "Deco Weapon"), new(15, "Deco Shield"), new(16, "Deco Armor"), new(17, "Deco Helmet"),
        new(18, "Deco Gloves"), new(19, "Deco Boots"), new(20, "Deco Mantle"), new(21, "Deco Shoulder"),
        new(22, "Ride Item"), new(23, "Bag Slot"), new(24, "Deco Booster"), new(25, "Deco Emblem"),
        new(26, "Second Earring"), new(27, "Chaos Stone"), new(28, "Medal"), new(29, "Mask"), new(30, "Wings"),
    ];
}
