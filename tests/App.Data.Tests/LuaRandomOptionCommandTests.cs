using App.Core.Commands;
using Xunit;

namespace App.Data.Tests;

public class LuaRandomOptionCommandTests
{
    [Fact]
    public void Own_MatchesLegacyScreenshotCase()
    {
        var cmd = LuaCommands.SetItemRandomOptionOwn(wearSlot: 0, line: 1, type: 96, options: 0, value: 0);
        Assert.Equal("set_item_random_option(get_wear_item_handle(0),1,96,0,0)", cmd);
    }

    [Fact]
    public void Own_EmitsLargeBitmaskAndDecimalValue()
    {
        var cmd = LuaCommands.SetItemRandomOptionOwn(wearSlot: 3, line: 2, type: 98, options: 2147483648, value: 1.5);
        Assert.Equal("set_item_random_option(get_wear_item_handle(3),2,98,2147483648,1.5)", cmd);
    }

    [Fact]
    public void Player_AddsEscapedPlayerNameToHandle()
    {
        var cmd = LuaCommands.SetItemRandomOptionPlayer(wearSlot: 3, playerName: "Hero", line: 1, type: 130, options: 1, value: 0);
        Assert.Equal("set_item_random_option(get_wear_item_handle(3,'Hero'),1,130,1,0)", cmd);
    }

    [Fact]
    public void Player_EscapesSingleQuoteInName()
    {
        var cmd = LuaCommands.SetItemRandomOptionPlayer(wearSlot: 2, playerName: "O'Hara", line: 1, type: 133, options: 5, value: 1);
        Assert.Equal("set_item_random_option(get_wear_item_handle(2,'O\\'Hara'),1,133,5,1)", cmd);
    }
}
