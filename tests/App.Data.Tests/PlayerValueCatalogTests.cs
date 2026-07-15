using App.Core.Characters;

namespace App.Data.Tests;

public class PlayerValueCatalogTests
{
    [Fact]
    public void Attributes_AreNonEmpty()
    {
        Assert.NotEmpty(PlayerValueCatalog.Attributes);
    }

    [Fact]
    public void Keys_AreLowercaseNonBlankAndUnique()
    {
        var keys = PlayerValueCatalog.Attributes.Select(a => a.Key).ToList();

        Assert.All(keys, k =>
        {
            Assert.False(string.IsNullOrWhiteSpace(k));
            Assert.Equal(k.ToLowerInvariant(), k);
        });

        Assert.Equal(keys.Count, keys.Distinct().Count());
    }

    [Fact]
    public void Labels_AreNonBlank()
    {
        Assert.All(PlayerValueCatalog.Attributes, a => Assert.False(string.IsNullOrWhiteSpace(a.Label)));
    }

    [Fact]
    public void Contains_TheHeadlineKeys()
    {
        var keys = PlayerValueCatalog.Attributes.Select(a => a.Key).ToHashSet();
        Assert.Contains("level", keys);
        Assert.Contains("exp", keys);
        Assert.Contains("jp", keys);
        Assert.Contains("gold", keys);
    }
}
