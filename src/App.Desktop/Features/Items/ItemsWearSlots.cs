using System.Globalization;

namespace App.Desktop.Features.Items;

/// <summary>Wear-slot dropdown entries.</summary>
public static class ItemsWearSlots
{
    public static readonly string[] All =
    [
        "-1 - WEAR_NONE / WEAR_CANTWEAR",
        "0 - WEAR_WEAPON",
        "1 - WEAR_SHIELD",
        "2 - WEAR_ARMOR",
        "3 - WEAR_HELM",
        "4 - WEAR_GLOVE",
        "5 - WEAR_BOOTS",
        "6 - WEAR_BELT",
        "7 - WEAR_MANTLE",
        "8 - WEAR_ARMULET",
        "9 - WEAR_RING",
        "10 - WEAR_SECOND_RING",
        "11 - WEAR_EAR",
        "12 - WEAR_FACE",
        "13 - WEAR_DECO_BACKPACK",
        "14 - WEAR_DECO_WEAPON",
        "15 - WEAR_DECO_SHIELD",
        "16 - WEAR_DECO_ARMOR",
        "17 - WEAR_DECO_HELM",
        "18 - WEAR_DECO_GLOVE",
        "19 - WEAR_DECO_BOOTS",
        "20 - WEAR_DECO_MANTLE",
        "21 - WEAR_DECO_SHOULDER",
        "22 - WEAR_RIDE_ITEM",
        "23 - WEAR_BAG_SLOT",
        "24 - WEAR_DECO_BOOSTER",
        "25 - WEAR_DECO_EMBLEM",
        "26 - WEAR_SECOND_EAR",
        "27 - WEAR_CHAOSSTONE",
        "28 - WEAR_MEDAL",
        "29 - WEAR_MASK",
        "30 - WEAR_WINGS",
        "31 - WEAR_SPARE_WEAPON",
        "32 - WEAR_SPARE_SHIELD",
        "33 - WEAR_SPARE_DECO_WEAPON",
        "34 - WEAR_SPARE_DECO_SHIELD",
        "94 - WEAR_TWOFINGER_RING",
        "99 - WEAR_TWOHAND",
        "100 - WEAR_SKILL",
        "200 - WEAR_SUMMON_ONLY",
    ];

    public const string Default = "0 - WEAR_WEAPON";

    /// <summary>The integer slot index = the number before the first space (fallback 0).</summary>
    public static int ParseIndex(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return 0;
        }

        var separator = value.IndexOf(' ');
        if (separator <= 0)
        {
            return 0;
        }

        var left = value[..separator];
        return int.TryParse(left, NumberStyles.Integer, CultureInfo.InvariantCulture, out var slot) ? slot : 0;
    }
}
