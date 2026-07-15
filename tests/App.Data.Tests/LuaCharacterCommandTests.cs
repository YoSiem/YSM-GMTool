using App.Core.Commands;

namespace App.Data.Tests;

public class LuaCharacterCommandTests
{
    [Fact]
    public void SetValueOwn_EmitsSvWithoutPlayer()
    {
        Assert.Equal("sv('level',150)", LuaCommands.SetValueOwn("level", 150));
    }

    [Fact]
    public void AddValueOwn_EmitsAvWithoutPlayer()
    {
        Assert.Equal("av('exp',1000000)", LuaCommands.AddValueOwn("exp", 1_000_000));
    }

    [Fact]
    public void SetValuePlayer_AppendsEscapedPlayerName()
    {
        Assert.Equal("sv('free_statpoints',999,'Foo')", LuaCommands.SetValuePlayer("free_statpoints", 999, "Foo"));
    }

    [Fact]
    public void AddValuePlayer_AppendsEscapedPlayerName()
    {
        Assert.Equal("av('gold',5000000,'BarMage')", LuaCommands.AddValuePlayer("gold", 5_000_000, "BarMage"));
    }

    [Fact]
    public void AddValueOwn_HandlesInt64BeyondInt32()
    {
        // exp/gold are int64 server-side; a value above int.MaxValue must round-trip intact.
        Assert.Equal("av('exp',9000000000)", LuaCommands.AddValueOwn("exp", 9_000_000_000L));
    }

    [Fact]
    public void SetValueOwn_EmitsNegativeValue()
    {
        Assert.Equal("av('gold',-500)", LuaCommands.AddValueOwn("gold", -500));
    }

    [Fact]
    public void Player_EscapesSingleQuoteInName()
    {
        Assert.Equal("sv('level',50,'O\\'Hara')", LuaCommands.SetValuePlayer("level", 50, "O'Hara"));
    }
}
