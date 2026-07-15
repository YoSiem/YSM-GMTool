using static System.FormattableString;

namespace App.Core.Commands;

public static class LuaCommands
{
    // Monster
    public static string MonsterRegenerate(int monsterId, int count)
        => Invariant($"//regenerate {monsterId} {count}");
    public static string MonsterAddNpcAtPlayer(string playerName, int monsterId, int count, int minutesLifetime)
    {
        var p = LuaEscape.Single(playerName);
        return Invariant($"add_npc( gv('x','{p}'), gv('y','{p}'),{monsterId},{count},{minutesLifetime}, gv('layer','{p}'))");
    }
    public static string MonsterAddNpcAtCoords(int x, int y, int monsterId, int count, int minutesLifetime, int layer)
        => Invariant($"add_npc( {x},{y},{monsterId},{count},{minutesLifetime},{layer})");

    // Items
    public static string InsertItemSelf(int itemId, int amount, int enhance, int level, int statusFlag)
        => Invariant($"insert_item({itemId},{amount},{enhance},{level},{statusFlag})");
    public static string InsertItemPlayer(int itemId, int amount, int enhance, int level, int statusFlag, string playerName)
        => Invariant($"insert_item({itemId},{amount},{enhance},{level},{statusFlag},'{LuaEscape.Single(playerName)}')");
    public static string SetWearItemLevelOwn(int wearSlot, int level)
        => Invariant($"set_item_level(get_wear_item_handle({wearSlot}),{level})");
    public static string SetWearItemLevelPlayer(int wearSlot, string playerName, int level)
        => Invariant($"set_item_level(get_wear_item_handle({wearSlot},'{LuaEscape.Single(playerName)}'),{level})");
    public static string SetWearItemEnhanceOwn(int wearSlot, int enhance)
        => Invariant($"set_item_enhance(get_wear_item_handle({wearSlot}),{enhance})");
    public static string SetWearItemEnhancePlayer(int wearSlot, string playerName, int enhance)
        => Invariant($"set_item_enhance(get_wear_item_handle({wearSlot},'{LuaEscape.Single(playerName)}'),{enhance})");
    public static string SetWearItemAppearanceOwn(int wearSlot, int itemCode)
        => Invariant($"set_item_appearance_code(get_wear_item_handle({wearSlot}),{itemCode})");
    public static string SetWearItemAppearancePlayer(int wearSlot, string playerName, int itemCode)
        => Invariant($"set_item_appearance_code(get_wear_item_handle({wearSlot},'{LuaEscape.Single(playerName)}'),{itemCode})");
    public static string ChangeWearItemCodeOwn(int wearSlot, int itemCode)
        => Invariant($"change_item_code(get_wear_item_handle({wearSlot}),{itemCode})");
    public static string ChangeWearItemCodePlayer(int wearSlot, string playerName, int itemCode)
        => Invariant($"change_item_code(get_wear_item_handle({wearSlot},'{LuaEscape.Single(playerName)}'),{itemCode})");

    // Random options (set_item_random_option). options is a long to hold the 32-bit stat
    // bitmask (bit 31 = FLAG_FINAL_DMG_REDUCTION) without int overflow.
    public static string SetItemRandomOptionOwn(int wearSlot, int line, int type, long options, double value)
        => Invariant($"set_item_random_option(get_wear_item_handle({wearSlot}),{line},{type},{options},{FormatValue(value)})");
    public static string SetItemRandomOptionPlayer(int wearSlot, string playerName, int line, int type, long options, double value)
        => Invariant($"set_item_random_option(get_wear_item_handle({wearSlot},'{LuaEscape.Single(playerName)}'),{line},{type},{options},{FormatValue(value)})");

    // Value (fValue2) is a double; emit whole numbers without a decimal point and trim trailing zeros.
    private static string FormatValue(double value)
        => value.ToString("0.######", System.Globalization.CultureInfo.InvariantCulture);

    // Skills
    public static string LearnSkill(int skillId) => Invariant($"learn_skill({skillId})");
    public static string LearnSkillForPlayer(int skillId, string playerName)
        => Invariant($"learn_skill({skillId},'{LuaEscape.Single(playerName)}')");
    public static string SetSkill(int skillId, int level) => Invariant($"set_skill({skillId},{level})");
    public static string SetSkillForPlayer(int skillId, int level, string playerName)
        => Invariant($"set_skill({skillId},{level},'{LuaEscape.Single(playerName)}')");
    public static string LearnAllSkill(string playerName)
        => Invariant($"learn_all_skill('{LuaEscape.Single(playerName)}')");
    public static string RemoveSkill(int skillId) => Invariant($"remove_skill({skillId})");
    public static string RemoveSkillForPlayer(int skillId, string playerName)
        => Invariant($"remove_skill({skillId},'{LuaEscape.Single(playerName)}')");
    public static string LearnCreatureSkillSelf(int skillId)
        => Invariant($"creature_learn_skill({skillId}, gcv(get_creature_handle(0), \"handle\"))");
    public static string LearnCreatureSkill(int skillId, string playerName)
        => Invariant($"creature_learn_skill({skillId}, gcv(get_creature_handle(0), \"handle\"), '{LuaEscape.Single(playerName)}')");
    public static string LearnCreatureAllSkill(int slotIndex)
        => Invariant($"learn_creature_all_skill({slotIndex})");
    public static string LearnCreatureAllSkillForPlayer(int slotIndex, string playerName)
        => Invariant($"learn_creature_all_skill({slotIndex},'{LuaEscape.Single(playerName)}')");

    // Buffs / states (duration emitted literally as N*100*60)
    public static string CastWorldState(int stateId, int level, int durationMinutes)
        => Invariant($"cast_world_state({stateId},{level},{durationMinutes}*100*60)");
    public static string AddEventState(int stateId, int level)
        => Invariant($"add_event_state({stateId},{level})");
    public static string RemoveEventState(int stateId)
        => Invariant($"remove_event_state({stateId},get_state_level({stateId}))");
    public static string AddPlayerState(int stateId, int level, int durationMinutes, string playerName)
        => Invariant($"add_state({stateId},{level},{durationMinutes}*100*60,'{LuaEscape.Single(playerName)}')");
    public static string RemovePlayerState(int stateId, string playerName)
    { var p = LuaEscape.Single(playerName); return Invariant($"remove_state({stateId},get_state_level({stateId},'{p}'),'{p}')"); }
    public static string AddCreatureState(int stateId, int level, int durationMinutes, string playerName)
        => Invariant($"add_cstate({stateId},{level},{durationMinutes}*100*60,'{LuaEscape.Single(playerName)}')");
    public static string RemoveCreatureState(int stateId, string playerName)
    { var p = LuaEscape.Single(playerName); return Invariant($"remove_cstate({stateId},get_state_level({stateId},'{p}'),'{p}')"); }

    // NPC
    public static string AddNpcToWorld(int x, int y, int layer, int npcId)
        => Invariant($"add_npc_to_world({x},{y},{layer},{npcId})");
    public static string AddNpcToWorldForPlayer(int x, int y, int layer, string playerName, int npcId)
        => Invariant($"add_npc_to_world({x},{y},{layer},'{LuaEscape.Single(playerName)}',{npcId})");
    public static string ShowNpc(int x, int y, int npcId, int layer, int visible)
        => Invariant($"show_npc({x},{y},{npcId},{layer},{visible})");
    public static string WarpToNpcCoordinates(int x, int y, string playerName) // NOTE: double-quote escaping
        => Invariant($"warp({x},{y},\"{LuaEscape.Double(playerName)}\")");

    // Warp
    public static string WarpToLocationForPlayer(int x, int y, string playerName)
        => Invariant($"warp({x},{y},'{LuaEscape.Single(playerName)}')");
    public static string WarpPlayerToYou(string playerName)
        => Invariant($"warp(gv(\"x\"),gv(\"y\"),'{LuaEscape.Single(playerName)}')");
    public static string WarpYouToPlayer(string playerName)
    { var p = LuaEscape.Single(playerName); return Invariant($"warp(gv(\"x\",'{p}'),gv(\"y\",'{p}'))"); }

    // Summons
    public static string InsertSummonById(int summonId)
        => Invariant($"insert_summon_by_summon_id({summonId})");
    public static string InsertSummonByIdWithStage(int summonId, int stage)
        => Invariant($"insert_summon_by_summon_id({summonId},{stage})");
    public static string StageSummon(int slot, int stage)
        => Invariant($"creature_enhance({slot},{stage})");

    // Player value editing (sv = set_value, av = add_value). Value emitted as an integer
    // literal; exp/gold/jp are __int64 server-side (StructPlayer), so it is carried as a long.
    // Setting 'level' makes the server set EXP to the threshold for that level
    // (StructPlayer::onChangeProperty). The optional player arg targets another character;
    // omitted = the command's owner (self).
    public static string SetValueOwn(string key, long value)
        => Invariant($"sv('{LuaEscape.Single(key)}',{value})");
    public static string SetValuePlayer(string key, long value, string playerName)
        => Invariant($"sv('{LuaEscape.Single(key)}',{value},'{LuaEscape.Single(playerName)}')");
    public static string AddValueOwn(string key, long value)
        => Invariant($"av('{LuaEscape.Single(key)}',{value})");
    public static string AddValuePlayer(string key, long value, string playerName)
        => Invariant($"av('{LuaEscape.Single(key)}',{value},'{LuaEscape.Single(playerName)}')");

    // Notice / announce broadcasts (global; every online player sees them). Single text arg.
    // Freeform text -> Lua double-quoted literal (text often contains apostrophes). The client
    // routes by the @NOTICE/@NOTICE_LEFT/@NOTICE_RIGHT/@ANNOUNCE sender tag the server supplies.
    public static string Notice(string text)
        => Invariant($"notice(\"{LuaEscape.Double(text)}\")");
    public static string NoticeLeft(string text)
        => Invariant($"notice_left(\"{LuaEscape.Double(text)}\")");
    public static string NoticeRight(string text)
        => Invariant($"notice_right(\"{LuaEscape.Double(text)}\")");
    public static string Announce(string text)
        => Invariant($"announce(\"{LuaEscape.Double(text)}\")");
}
