using App.Core.RandomOptions;
using Xunit;

namespace App.Data.Tests;

public class RandomOptionCalculatorTests
{
    [Fact]
    public void ComputeMask_StrengthAndVitality_Returns3()
    {
        Assert.Equal(3L, RandomOptionCalculator.ComputeMask([0, 1]));
    }

    [Fact]
    public void ComputeMask_Bit31_DoesNotOverflow()
    {
        Assert.Equal(2147483648L, RandomOptionCalculator.ComputeMask([31]));
    }

    [Fact]
    public void ComputeMask_AllPart1Bits_Returns4294967295()
    {
        var allBits = new List<int>();
        for (var b = 0; b < 32; b++) allBits.Add(b);
        Assert.Equal(4294967295L, RandomOptionCalculator.ComputeMask(allBits));
    }

    [Fact]
    public void BitsFromMask_RoundTripsWithComputeMask()
    {
        var mask = RandomOptionCalculator.ComputeMask([0, 7, 31]);
        var bits = RandomOptionCalculator.BitsFromMask(mask, RandomOptionCatalog.Part1);
        Assert.Equal(new[] { 0, 7, 31 }, bits);
    }

    [Fact]
    public void BitsFromMask_IgnoresBitsNotInCatalog()
    {
        // Bit 7 has no Part 2 flag; a mask with bit 7 set must not surface it.
        var mask = RandomOptionCalculator.ComputeMask([1, 7]);
        var bits = RandomOptionCalculator.BitsFromMask(mask, RandomOptionCatalog.Part2);
        Assert.Equal(new[] { 1 }, bits);
    }

    [Theory]
    [InlineData(false, false, 96)]
    [InlineData(false, true, 98)]
    [InlineData(true, false, 97)]
    [InlineData(true, true, 99)]
    public void DeriveStatType_MatchesMatrix(bool isPart2, bool isPercentage, int expected)
    {
        Assert.Equal(expected, RandomOptionCalculator.DeriveStatType(isPart2, isPercentage));
    }

    [Fact]
    public void Catalogs_HaveExpectedCounts()
    {
        Assert.Equal(32, RandomOptionCatalog.Part1.Count);
        Assert.Equal(30, RandomOptionCatalog.Part2.Count);   // bit 7 missing
        Assert.Equal(31, RandomOptionCatalog.Equipparts.Count); // 0..30
        Assert.DoesNotContain(RandomOptionCatalog.Part2, f => f.Bit == 7);
    }
}
