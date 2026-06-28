# Random Option Tab — Design

**Date:** 2026-06-28
**Status:** Approved (UI mockup confirmed) — pending spec review
**Branch:** `feat/random-option-tab`

## 1. Goal

Add a new shell tab **"Random Option"** that ports an external standalone tool into the
Avalonia GM-Tool. The tab builds a Rappelz `set_item_random_option(...)` Lua command from a
form and copies it to the clipboard (the app's universal "dispatch = copy, optionally
prefixed with `/run`" model). It supports three random-option kinds (Stat / Socket / Item
effect), a live-updating stat bitflag grid, a clipboard import, and a database-backed picker
for item effects.

The UI was prototyped and approved as an interactive mockup
(`scratchpad/random-option-mockup.html`); this spec is the source of truth for behaviour.

## 2. Source-of-truth: the Lua command

Verified against the Rappelz server source
(`Game/Script/ScriptItem.cpp` → `SCRIPT_SetItemIdentifiedOption`):

```
set_item_random_option(handle, index, opt1, opt2, val)
  handle = AR_HANDLE of the worn item  (from get_wear_item_handle(slot[, 'player']))
  index  = 1-based random-option line   (1..MAX_RANDOM_OPTION_NUMBER)
  opt1   = OptionInfo[index-1].nType     (int)     -> our "Type"
  opt2   = OptionInfo[index-1].fValue1   (double)  -> our "Options"
  val    = OptionInfo[index-1].fValue2   (double)  -> our "Value"
```

`get_wear_item_handle(slot)` reads the player from Lua arg 2
(`SCRIPT_GetWearItemHandle` → `getPlayer(L, 2)`), so targeting another player is done by
`get_wear_item_handle(slot, 'name')` — the same pattern the existing Items tab already uses
(`LuaCommands.SetWearItemLevelPlayer`). This matches the WinForms parity convention; we do
**not** pass an extra player argument to `set_item_random_option` itself.

Full template:

```
set_item_random_option(get_wear_item_handle(SLOT[,'PLAYER']),LINE,TYPE,OPTIONS,VALUE)
```

The screenshot from the old tool (`...get_wear_item_handle(0),1,96,0,0`) matches exactly:
SLOT=0, LINE=1, TYPE=96, OPTIONS=0, VALUE=0.

## 3. Placement & architecture

Follow the established per-tab pattern verbatim:

- `src/App.Desktop/Features/RandomOption/RandomOptionTabViewModel.cs` —
  `TabModuleViewModel`, discovered by reflection (`AddTabModules`).
  - `Title => "Random Option"`, `IconKey => "fa-solid fa-dice"`, `Order => 35`
    (slots between Items=30 and Skills=40).
- `src/App.Desktop/Features/RandomOption/RandomOptionTabView.axaml` (+ `.axaml.cs`).
  - **Not** wrapped in `EntityBrowserView` — this tab has no primary entity list; it is a
    standalone builder form. (The item-effect picker is a separate modal window.)
- Resolved by `ReactiveViewLocator` (ViewModel→View name swap). No manual registration
  needed beyond the reflection scan.

Pure, testable logic (flag catalog, bitmask math, type derivation, command building) lives in
**App.Core** so `App.Data.Tests` (which references App.Core) can unit-test it. UI-only data
(line list, equippart list bound directly) lives in the feature folder.

## 4. UI / field model (per mode)

Reference layout: the approved mockup. Dark, dense desktop styling using the existing theme
brushes and style classes (`sectionHeader`, `sectionDivider`, `fieldLabel`, `field`,
`primary`).

### Universal controls (always visible)
- **Random Type** radio: `Stat-Effect` (default) | `Socket-Effect` | `Item-Effect`.
- **Equippart (equipped)** dropdown — readable English names, value = slot index (see §7).
  Default = `Weapon` (0).
- **Line** dropdown `1..10` (default 1).
- **Target** radio: `Own` (default) | `Other`. `Other` reveals/uses the right-sidebar
  selected player (`IPlayerContext`); none selected → warning dialog, no copy.
- **Type:** read-only, auto-derived.
- **Options:** read-only / derived in every mode (never free-typed — driven by checkboxes,
  picker, or socket dropdown).
- **Value:** depends on mode.
- **Command** preview — read-only, live; shows exactly what Copy puts on the clipboard
  (including the `/run ` prefix when the global setting is on).
- **Copy Command to Clipboard** — primary button; dispatches via `ICommandDispatcher`.

### Stat-Effect mode
- **Options** radio: `Part 1` (default) | `Part 2`.
- **OptTypes** radio: `Fix Value` (default) | `Percentage`.
- **Type** auto: `Part1+Fix=96`, `Part1+Pct=98`, `Part2+Fix=97`, `Part2+Pct=99`.
- **Checkbox grid** (two columns). Labels switch with Part 1/2 (see §6).
- **Options** field = OR of `(1L << bit)` over checked boxes; recomputed on every click;
  read-only.
- **Value** field = manual numeric magnitude (`fValue2`), decimals allowed, default 0.
- **Import options** button: reads the clipboard (`IClipboardService.GetTextAsync`), parses
  an unsigned integer, and sets the checkboxes to that bitmask (checking matching bits,
  unchecking the rest) → recomputes Options. Invalid/empty clipboard → warning dialog.

### Socket-Effect mode
- **Type** forced to `130`.
- **Socket Type** dropdown `{1, 2}` → Options.
- **Amount** dropdown `{1, 2}` → Value.
- Stat grid / Part / OptType hidden.

### Item-Effect mode
- **Type** forced to `133`.
- **Options** = selected `Effect_id` (read-only display; default empty → treated as 0 until
  picked, with Copy disabled/blocked until an effect is chosen).
- **Pick effect…** button opens the modal picker (§8); on select sets Options = Effect_id and
  shows the chosen `Effecttext`.
- **Value** field = manual numeric, default 1.
- Stat grid / Part / OptType hidden.

## 5. Command construction (`App.Core/Commands/LuaCommands.cs`)

Add:

```csharp
// Random options (set_item_random_option). options carried as long to hold the 32-bit
// stat bitmask without int overflow (bit 31 = FLAG_FINAL_DMG_REDUCTION).
public static string SetItemRandomOptionOwn(int wearSlot, int line, int type, long options, double value)
    => Invariant($"set_item_random_option(get_wear_item_handle({wearSlot}),{line},{type},{options},{FormatValue(value)})");

public static string SetItemRandomOptionPlayer(int wearSlot, string playerName, int line, int type, long options, double value)
    => Invariant($"set_item_random_option(get_wear_item_handle({wearSlot},'{LuaEscape.Single(playerName)}'),{line},{type},{options},{FormatValue(value)})");
```

`FormatValue(double)` formats with invariant `0.######` (whole numbers emit no decimals:
`0 -> "0"`, `1.5 -> "1.5"`).

Live preview shows the prefixed string. To keep the preview truthful and DRY, expose the
dispatcher's prefix logic: refactor `CommandDispatcher.ApplyRunPrefix` into a public
`string Format(string luaCommand)` on `ICommandDispatcher`; `DispatchAsync` uses it, and the
VM uses it to render the preview.

## 6. Stat bitflag catalog (`App.Core/RandomOptions/`)

`RandomOptionFlag { int Bit; string Label; }` and `RandomOptionCatalog.Part1` / `.Part2`.
Labels are readable (derived from the enum, not verbatim). **Part 2 has a gap at bit 7**
(no flag) and includes the penetration flags (bits 12/13) — included by default since this
server build uses them.

**Part 1** (bits 0–31):

| Bit | Label | Bit | Label |
|----|-------|----|-------|
|0|Strength|16|Crit Chance|
|1|Vitality|17|Block Chance|
|2|Agility|18|Block Def|
|3|Dexterity|19|Evasion|
|4|Intelligence|20|m.Res|
|5|Wisdom|21|Max HP|
|6|Luck|22|Max MP|
|7|p.Atk|23|Max SP|
|8|m.Atk|24|HP Regen|
|9|P.Def|25|MP Regen|
|10|M.Def|26|SP Regen|
|11|Atk Speed|27|HP Regen Ratio|
|12|Cast Speed|28|MP Regen Ratio|
|13|Move Speed|29|Final Dmg Increase|
|14|Accuracy|30|Max Weight|
|15|m.Acc|31|Final Dmg Reduction|

**Part 2** (bits 0–6, 8–30; **no bit 7**):

| Bit | Label | Bit | Label |
|----|-------|----|-------|
|0|None Resist|17|Wind Damage|
|1|Fire Resist|18|Earth Damage|
|2|Water Resist|19|Light Damage|
|3|Wind Resist|20|Dark Damage|
|4|Earth Resist|21|None Add. Damage|
|5|Light Resist|22|Fire Add. Damage|
|6|Dark Resist|23|Water Add. Damage|
|8|Attack Range|24|Wind Add. Damage|
|9|Perfect Block|25|Earth Add. Damage|
|10|Ignore P.Def|26|Light Add. Damage|
|11|Ignore M.Def|27|Dark Add. Damage|
|12|Physical Penetration|28|Crit Damage|
|13|Magical Penetration|29|HP Regen Stop|
|14|None Damage|30|MP Regen Stop|
|15|Fire Damage| | |
|16|Water Damage| | |

`RandomOptionCalculator`:
- `long ComputeMask(IEnumerable<int> checkedBits)` → OR of `1L << bit`.
- `IReadOnlyList<int> BitsFromMask(long mask, IReadOnlyList<RandomOptionFlag> catalog)` →
  bits present in `mask` and known to the catalog (used by Import to set checkboxes).
- `int DeriveStatType(bool isPart2, bool isPercentage)` → 96/97/98/99.

## 7. Equippart catalog

Readable English names → slot index (the value emitted in the command). Source enum is the
`WEAR_*` list the user supplied (0..30 used):

```
0 Weapon, 1 Shield, 2 Armor, 3 Helmet, 4 Gloves, 5 Boots, 6 Belt, 7 Mantle,
8 Amulet, 9 Ring, 10 Second Ring, 11 Earring, 12 Face, 13 Backpack,
14 Deco Weapon, 15 Deco Shield, 16 Deco Armor, 17 Deco Helmet, 18 Deco Gloves,
19 Deco Boots, 20 Deco Mantle, 21 Deco Shoulder, 22 Ride Item, 23 Bag Slot,
24 Deco Booster, 25 Deco Emblem, 26 Second Earring, 27 Chaos Stone, 28 Medal,
29 Mask, 30 Wings
```

Bound as `(int Index, string Label)`; the dropdown's `SelectedValue` is the index, so no
parsing is needed.

## 8. Item-Effect picker (data + window + service)

### Data
- New entity `App.Core/Models/Entities/ItemEffectRecord.cs`:
  `{ int EffectId; string EffectText; }` (Dapper column names `EffectId`, `EffectText`).
- New `QueryEntity.ItemEffects`.
- New repo method on `IGameDataRepository` / `GameDataRepository`:
  `Task<IReadOnlyList<ItemEffectRecord>> GetItemEffectsAsync(provider, connectionString, tokens, ct)`
  (uses the standard `QueryAsync<T>` path; no parameters).
- `queries.json` entries for **MSSQL** and **MySQL** (Sqlite/local-cache **not** supported —
  the picker requires a live DB):

  MSSQL:
  ```sql
  SELECT i.[id] AS EffectId, n.[value_en] AS EffectText
  FROM [{{ArcadiaName}}].dbo.ItemEffectResource i
  LEFT JOIN [{{ArcadiaName}}].dbo.{{StringResource}} n ON i.[tooltip_id] = n.[code]
  ORDER BY i.[id];
  ```
  MySQL (backtick-quoted, same shape).

  Per the user's decision: `ItemEffectResource` is **hardcoded** (always the same table);
  `StringResourceFull` maps to the existing **`{{StringResource}}`** token. ⚠️ **Assumption to
  confirm (§13):** column kept as `value_en` (the user's original query) even though every
  other query selects `value`.

### Window + VM + service
- `ItemEffectPickerWindow.axaml` (+ `.cs`) — modal `Window`, reuses `EntityBrowserView`
  bound to an `EntityBrowserViewModel<ItemEffectRecord>` (free search/sort), with **Select**
  / **Cancel** buttons and row double-click. Returns the chosen record via
  `ShowDialog<ItemEffectRecord?>`.
- `IItemEffectPickerService` + `ItemEffectPickerService` (Desktop infra, registered in
  `ServiceCollectionExtensions`): `Task<ItemEffectRecord?> PickAsync(CancellationToken)`.
  Resolves the connection via `ConnectionStringResolver`, loads rows through the repo, builds
  the VM + window, shows it modally, returns the selection. DB/connection errors → error
  dialog + `null`. Keeps the tab VM free of any `Window`/Avalonia coupling (testability).

## 9. Infrastructure extensions

- `IClipboardService`: add `Task<string?> GetTextAsync()`; implement in
  `AvaloniaClipboardService` via the top-level window clipboard `GetTextAsync()`.
- `ICommandDispatcher`: add `string Format(string luaCommand)` (prefix logic), reused by
  `DispatchAsync` and the live preview.
- `IItemEffectPickerService` (new, §8).

## 10. Error handling

- `Other` target with no selected player → `IDialogService.ShowWarningAsync`, no copy
  (mirrors Items/Warp tabs).
- Item-Effect mode with no effect picked → block Copy with a warning.
- Import with invalid/empty clipboard → warning.
- Picker DB failures → error dialog, picker returns null (tab unchanged).
- All dialogs go through the existing `IDialogService`.

## 11. Testing strategy (`tests/App.Data.Tests`, references App.Core)

Unit tests for the pure logic:
1. `ComputeMask` — selected bits → expected mask, including bit 31 → `2147483648`, and all
   Part 1 bits → `4294967295`.
2. `BitsFromMask` — round-trips with ComputeMask; ignores bits not in the catalog (e.g. bit 7
   for Part 2).
3. `DeriveStatType` — the full 96/97/98/99 matrix.
4. `LuaCommands.SetItemRandomOptionOwn/Player` — exact strings for representative inputs
   (incl. the screenshot case `get_wear_item_handle(0),1,96,0,0`, an "other player" case, and
   a decimal Value).
5. `FormatValue` — `0 -> "0"`, `1.5 -> "1.5"`, whole double → no decimals.

VM/UI is not unit-tested (no desktop test project); the picker/clipboard/dispatcher are
exercised through their interfaces.

## 12. Out of scope (YAGNI)

- No Sqlite/local-cache support for item effects (picker is live-DB only).
- No persistence of the last-used form state.
- No awaken/socket *resource* pickers beyond the Socket Type/Amount dropdowns the user
  specified.
- No new configurable table-name settings (ItemEffectResource stays hardcoded).

## 13. Decisions & assumptions to confirm

- **Targeting:** Own + optional Other via `get_wear_item_handle(slot,'player')` — confirmed
  supported by source. ✅
- **Picker tables:** `ItemEffectResource` hardcoded; `StringResourceFull` → `{{StringResource}}`
  token. ✅
- **Socket mode:** Socket Type {1,2} → Options, Amount {1,2} → Value. ✅
- **⚠️ Open:** picker text column `value_en` vs `value`. Kept as `value_en` per the original
  query; flag for confirmation (all other queries use `value`).
- Penetration flags (Part 2 bits 12/13) included by default.
- Order = 35, icon `fa-solid fa-dice`.

## 14. File-by-file change list

**App.Core**
- `Commands/LuaCommands.cs` — add `SetItemRandomOptionOwn/Player` (+ `FormatValue`).
- `RandomOptions/RandomOptionFlag.cs` — record struct.
- `RandomOptions/RandomOptionCatalog.cs` — Part1/Part2 flag lists + equippart list.
- `RandomOptions/RandomOptionCalculator.cs` — ComputeMask / BitsFromMask / DeriveStatType.
- `Models/Entities/ItemEffectRecord.cs` — new record.
- `Enums/QueryEntity.cs` — add `ItemEffects`.
- `Abstractions/IGameDataRepository.cs` — add `GetItemEffectsAsync`.

**App.Data**
- `Repositories/GameDataRepository.cs` — implement `GetItemEffectsAsync`.

**App.Desktop**
- `Config/queries.json` — add `ItemEffects` (MSSQL + MySQL).
- `Infrastructure/IClipboardService.cs` + `AvaloniaClipboardService.cs` — add `GetTextAsync`.
- `Services/ICommandDispatcher.cs` + `CommandDispatcher.cs` — add `Format`.
- `Infrastructure/IItemEffectPickerService.cs` + `ItemEffectPickerService.cs` — new.
- `Features/RandomOption/ItemEffectPickerWindow.axaml(.cs)` + `ItemEffectPickerViewModel.cs`.
- `Features/RandomOption/RandomOptionTabViewModel.cs` + `RandomOptionTabView.axaml(.cs)`.
- `Composition/ServiceCollectionExtensions.cs` — register `IItemEffectPickerService`.

**tests/App.Data.Tests**
- `RandomOptionCalculatorTests.cs`, `LuaRandomOptionCommandTests.cs`.
