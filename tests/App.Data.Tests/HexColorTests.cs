using App.Core.Commands;

namespace App.Data.Tests;

public class HexColorTests
{
    [Theory]
    [InlineData("#FFEA00", "ffea00")]
    [InlineData("ff0000", "ff0000")]
    [InlineData("#abc", "aabbcc")]       // 3-digit shorthand expands
    [InlineData("0a84ff", "0a84ff")]
    [InlineData("#FF000080", "ff000080")] // 8-digit RGBA passes through (game supports <#rrggbbaa>)
    public void Normalize_AcceptsValidHex(string input, string expected)
    {
        Assert.Equal(expected, HexColor.Normalize(input));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("zzz")]          // no hex digits
    [InlineData("12")]           // 2 digits
    [InlineData("12345")]        // 5 digits
    [InlineData("1234567")]      // 7 digits (neither RGB nor RGBA)
    [InlineData("123456789")]    // 9 digits
    public void Normalize_RejectsInvalidHex(string? input)
    {
        Assert.Null(HexColor.Normalize(input));
    }

    [Fact]
    public void Normalize_DoesNotTruncate8DigitToFirst6()
    {
        // Regression: an 8-digit value must NOT silently become its first 6 digits.
        Assert.Equal("80ffea00", HexColor.Normalize("#80FFEA00"));
    }
}
