using App.Core.Commands;

namespace App.Data.Tests;

public class LuaNoticeCommandTests
{
    [Fact]
    public void Notice_WrapsTextInDoubleQuotes()
    {
        Assert.Equal("notice(\"Welcome!\")", LuaCommands.Notice("Welcome!"));
    }

    [Fact]
    public void NoticeLeft_UsesLeftFunction()
    {
        Assert.Equal("notice_left(\"Event<BR>Now\")", LuaCommands.NoticeLeft("Event<BR>Now"));
    }

    [Fact]
    public void NoticeRight_UsesRightFunction()
    {
        Assert.Equal("notice_right(\"Right side\")", LuaCommands.NoticeRight("Right side"));
    }

    [Fact]
    public void Announce_UsesAnnounceFunction()
    {
        Assert.Equal("announce(\"Server restart in 5m\")", LuaCommands.Announce("Server restart in 5m"));
    }

    [Fact]
    public void Notice_EscapesDoubleQuoteAndBackslash()
    {
        Assert.Equal("notice(\"say \\\"hi\\\" \\\\o/\")", LuaCommands.Notice("say \"hi\" \\o/"));
    }

    [Fact]
    public void Notice_KeepsApostropheUnescaped()
    {
        // Double-quoted Lua literal: apostrophes need no escaping.
        Assert.Equal("notice(\"GM's note\")", LuaCommands.Notice("GM's note"));
    }
}
