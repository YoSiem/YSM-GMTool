namespace App.Core.RandomOptions;

/// <summary>Pure bitmask + option-type math for the Random Option tab (no UI dependencies).</summary>
public static class RandomOptionCalculator
{
    /// <summary>OR of (1L &lt;&lt; bit) over the given bits. Uses long so bit 31 doesn't overflow.</summary>
    public static long ComputeMask(IEnumerable<int> checkedBits)
    {
        long mask = 0;
        foreach (var bit in checkedBits)
        {
            if (bit is >= 0 and < 32)
            {
                mask |= 1L << bit;
            }
        }

        return mask;
    }

    /// <summary>The bits from <paramref name="catalog"/> that are set in <paramref name="mask"/>.</summary>
    public static IReadOnlyList<int> BitsFromMask(long mask, IReadOnlyList<RandomOptionFlag> catalog)
    {
        var bits = new List<int>();
        foreach (var flag in catalog)
        {
            if ((mask & (1L << flag.Bit)) != 0)
            {
                bits.Add(flag.Bit);
            }
        }

        return bits;
    }

    /// <summary>Stat-effect option type: Part1 fix=96/pct=98, Part2 fix=97/pct=99.</summary>
    public static int DeriveStatType(bool isPart2, bool isPercentage) => (isPart2, isPercentage) switch
    {
        (false, false) => 96,
        (false, true) => 98,
        (true, false) => 97,
        (true, true) => 99,
    };
}
