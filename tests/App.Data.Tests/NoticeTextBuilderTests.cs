using App.Core.Commands;

namespace App.Data.Tests;

public class NoticeTextBuilderTests
{
    [Fact]
    public void Plain_NoTogglesProducesNoTags()
    {
        Assert.Equal("Hello", NoticeTextBuilder.Build("Hello", bold: false, colorHex: null, size: null));
    }

    [Fact]
    public void MultiLine_JoinsWithBrTag()
    {
        Assert.Equal("a<BR>b<BR>c", NoticeTextBuilder.Build("a\nb\nc", bold: false, colorHex: null, size: null));
    }

    [Fact]
    public void CrlfAndCr_NormalizeToBr()
    {
        Assert.Equal("a<BR>b<BR>c", NoticeTextBuilder.Build("a\r\nb\rc", bold: false, colorHex: null, size: null));
    }

    [Fact]
    public void Bold_WrapsWholeBody()
    {
        Assert.Equal("<B>a<BR>b</B>", NoticeTextBuilder.Build("a\nb", bold: true, colorHex: null, size: null));
    }

    [Fact]
    public void ColorOnly_PrefixesColorTag()
    {
        Assert.Equal("<#ff0000>Hi", NoticeTextBuilder.Build("Hi", bold: false, colorHex: "ff0000", size: null));
    }

    [Fact]
    public void SizeOnly_PrefixesSizeTag()
    {
        Assert.Equal("<size:20>Hi", NoticeTextBuilder.Build("Hi", bold: false, colorHex: null, size: 20));
    }

    [Fact]
    public void ColorSizeBold_ComposeInOrder()
    {
        // Prefix = color then size; bold wraps the body; both precede the wrapped body.
        Assert.Equal("<#00ff00><size:18><B>Event<BR>Now</B>",
            NoticeTextBuilder.Build("Event\nNow", bold: true, colorHex: "00ff00", size: 18));
    }

    [Fact]
    public void EmptyText_ProducesEmptyBody()
    {
        Assert.Equal(string.Empty, NoticeTextBuilder.Build("", bold: false, colorHex: null, size: null));
    }
}
