# Character & Notice Tabs — Design

**Date:** 2026-06-28
**Status:** Approved (UI mockup confirmed) — pending spec review
**Branch:** `feat/character-notice-tabs`

## 1. Goal

Add two new shell tabs to the Avalonia GM-Tool:

- **Character** — a universal *Set value* / *Add value* builder that emits the server's
  `sv('key',value[,'player'])` / `av('key',value[,'player'])` Lua functions, plus one-click
  presets for the most common edits (Set Level, +EXP, +JP, +Gold).
- **Notice** — a notice/announce builder with a **live preview** that emits
  `notice(...)` / `notice_left(...)` / `notice_right(...)` / `announce(...)`, with color, size,
  bold, alignment and multi-line (`<BR>`) support.

Both follow the app's universal "dispatch = copy to clipboard, optionally prefixed with `/run`"
model. The UI was prototyped and approved as an interactive localhost mockup
(`scratchpad/mockup/index.html`); this spec is the source of truth for behaviour.

## 2. Source-of-truth: the Lua functions

Verified against the Rappelz server source under
`C:\Users\patry\Desktop\RZ-HeavenSource\Game-Server` (the server that matches the `SFrame`
client in the same tree).

### 2.1 `sv` / `av` / `gv` (value editing)

Registered in `CaptainHerlockServer.cpp:863-872`:

```
gv = get_value   av = add_value   sv = set_value
```

Signatures (from `Game/Script/ScriptPlayer.cpp`):

```
sv('key', value [, 'player'])    -- SCRIPT_SetValue  (:766)  arg1=key arg2=value arg3=player(getPlayer L,3)
av('key', amount [, 'player'])   -- SCRIPT_AddValue  (:786)  same arg layout; reads current, adds, writes
gv('key' [, 'player'])           -- SCRIPT_GetValue  (:672)  (not used by this tool)
```

- The 3rd `player` arg is **optional**; omitted → the script owner (self).
- `exp`, `gold`, `jp` are handled as **`__int64`** (`:691`, `:812`); other numeric keys are
  doubles/ints. → The tool emits values as **integers (long)**; all curated keys are integer-valued.
- **Level is special** (`StructPlayer::onChangeProperty`, `StructPlayer.cpp:735`): writing
  `lv`/`level` calls `SetEXP(GameContent::GetNeedExp(N))`, i.e. *setting level sets EXP to the
  threshold for level N*. So `sv('level', N)` is the correct way to set a character's level;
  `av('level', +k)` likewise bumps it by `k` levels.
- Valid keys are whatever `StructPlayer::BindProperty` binds (`StructPlayer.cpp:514-590`).
  Curated subset (§6) plus a free-text "custom key" escape hatch.

### 2.2 `notice` / `notice_left` / `notice_right` / `announce` (broadcasts)

Registered in `CaptainHerlockServer.cpp:1034-1038`; implemented in `Game/Script/ScriptMisc.cpp`:

```
notice("text")        -- SCRIPT_Notice        (:273) -> SendNotice(@NOTICE,        CHAT_NOTICE)
notice_left("text")   -- SCRIPT_Notice_Left   (:279) -> SendNotice(@NOTICE_LEFT,   CHAT_LEFT_NOTICE)
notice_right("text")  -- SCRIPT_Notice_Right  (:285) -> SendNotice(@NOTICE_RIGHT,  CHAT_RIGHT_NOTICE)
announce("text")      -- SCRIPT_Announce      (:292) -> SendGlobalChatMessage(CHAT_ANNOUNCE, @ANNOUNCE)
```

- All four are **global broadcasts** (every online player sees them) and take a **single text
  argument**. They are not player-targeted → the Notice tab has **no Own/Other selector**.
- Client rendering (`SFrame/game/Interface/TopConsole/SUINoticeWnd.cpp`,
  `sNoticeLine::getCaptionTag`): the client auto-prefixes a style tag before the text:
  - notice / notice_left / notice_right → `<size:13><hcenter><top><#ffea00>` (**yellow**), routed
    by sender `@NOTICE` / `@NOTICE_LEFT` / `@NOTICE_RIGHT` to center / left / right screen
    positions, ~10s lifetime.
  - announce → `<size:13><hcenter><top><#00ff00>` (**green**), centered, ~5s lifetime.
- The text may contain rich-text tags that **override** the defaults. Tag set
  (`SFrame/engine/Ui/KTextParser.h`): `<#rrggbb[aa]>`, `<size:n>`, `<font:name>`, `<B></B>`,
  `<U></U>`, `<BR>`, `<P>`, `<HCENTER>/<LEFT>/<RIGHT>/<VCENTER>/<TOP>/<BOTTOM>`, `<SHD>`,
  `<OUT>`, `<GLOW>`, `<INV>`, `<STRIKE>`, emoticons `<%%id>` / `<%id>`. The builder produces the
  common subset (color/size/bold + `<BR>`); the user can type any other tags manually into the
  text box and they are passed through verbatim.

## 3. Placement & architecture

Follow the established per-tab pattern verbatim (reflection discovery via `AddTabModules`;
ViewModel→View resolved by `ReactiveViewLocator`):

- **Character** — `src/App.Desktop/Features/Character/`
  - `CharacterTabViewModel.cs` (`TabModuleViewModel`), `Title => "Character"`,
    `IconKey => "fa-solid fa-user"`, `Order => 15` (between Playerchecker=10 and Monster=20).
  - `CharacterTabView.axaml` (+ `.axaml.cs`). Standalone builder form (no `EntityBrowserView`).
  - `CharacterOp.cs` — `enum { Set, Add }`.
- **Notice** — `src/App.Desktop/Features/Notice/`
  - `NoticeTabViewModel.cs` (`TabModuleViewModel`), `Title => "Notice"`,
    `IconKey => "fa-solid fa-bullhorn"`, `Order => 100` (last tab).
  - `NoticeTabView.axaml` (+ `.axaml.cs`).
  - `NoticeKind.cs` — `enum { Center, Left, Right, Announce }`.

Pure, testable logic lives in **App.Core** (so `App.Data.Tests` can unit-test it):
- `Commands/LuaCommands.cs` — the `sv`/`av`/notice/announce string builders (§5).
- `Commands/NoticeTextBuilder.cs` — assembles the notice payload from controls (§7).
- `Characters/PlayerValueCatalog.cs` + `Characters/PlayerValueAttribute.cs` — curated list (§6).

Reuse existing infra: `ICommandDispatcher.Format` (live preview + `/run` prefix, already added
in the Random Option work), `IPlayerContext` (Own/Other), `IDialogService` (warnings).

## 4. Character tab — UI / field model

Dark dense styling using existing theme brushes / style classes (`sectionHeader`,
`sectionDivider`, `fieldLabel`, `field`, `primary`). Reference layout = approved mockup.

- **Target** radio: `Own` (default) | `Other`. `Other` uses the right-sidebar selected player
  (`IPlayerContext`); none selected → warning dialog, no copy (mirrors Items/Warp/RandomOption).
- **Operation** segmented: `Set (sv)` (default) | `Add (av)`. Value label switches
  `Value` ↔ `Amount`.
- **Attribute** dropdown — curated list (§6), default `Level`.
- **Custom key** checkbox: when ticked, replaces the dropdown with a free-text key field
  (e.g. `jlv`, `immoral`, `pk_count`). Empty custom key → Copy blocked with a warning.
- **Value / Amount** numeric field — backed by `long` (holds int64 exp/gold), default `0`,
  integer formatting (no decimals).
- **Quick actions** (configure the builder, single source of truth — they do **not** add
  separate inputs): `Set Level` → (Set, level) · `+EXP` → (Add, exp) · `+JP` → (Add, jp) ·
  `+Gold` → (Add, gold). Clicking sets operation+attribute; the user enters the value and copies.
- **Command** preview — read-only, live, shows exactly what Copy puts on the clipboard (incl.
  the `/run ` prefix when the global setting is on, via `ICommandDispatcher.Format`).
- **Copy command to clipboard** — primary button; dispatches via `ICommandDispatcher`.

Examples: `sv('level',150)` · `av('exp',1000000)` · `av('gold',5000000,'Foo')` ·
`sv('free_statpoints',999)`.

## 5. Notice tab — UI / field model + preview

### Controls
- **Alignment / function** segmented: `Center` (`notice`, default) | `Left` (`notice_left`) |
  `Right` (`notice_right`) | `Announce` (`announce`). Announce forces centered preview and a
  green default color.
- **Text** — multi-line text box. Line breaks join with `<BR>` into a single function call.
  May contain manual tags (passed through verbatim).
- **Custom color** checkbox + color picker → `<#rrggbb>`. Unchecked → no color tag emitted
  (client default applies: yellow for notice variants, green for announce).
- **Custom size** checkbox + numeric (6–60) → `<size:n>`. Unchecked → no size tag (client
  default 13).
- **Bold** toggle → wraps the whole body in `<B>…</B>`.
- **Command** preview (read-only, live) + **Copy** primary button. Empty text → Copy blocked
  with a warning (avoid emitting `notice("")`).

### Live preview (the headline feature)
A dark `Border` (simulated game screen) containing a `TextBlock` bound to:
- `Foreground` = custom color, else mode default (`#FFEA00` notice / `#00FF00` announce).
- `FontSize` = custom size (display-scaled), else a fixed default (~17px baseline for the
  in-game size-13 look).
- `FontWeight` = Bold when toggled.
- `TextAlignment` = Center / Left / Right (Announce → Center).
- Text with `<BR>` shown as real line breaks.

**Fidelity limitation (documented):** the preview reflects only the builder's own controls
(color / size / bold / alignment / `<BR>`). Arbitrary tags typed manually into the text are sent
to the game correctly but are **not** fully parsed/rendered in the preview.

Examples: `notice("Welcome!")` ·
`notice_left("<#ff0000><size:20><B>Event<BR>Now</B>")` · `announce("Server restart in 5m")`.

## 6. Player value catalog (`App.Core/Characters/`)

`readonly record struct PlayerValueAttribute(string Key, string Label)` and a static
`PlayerValueCatalog.Attributes` list (label → emitted key):

| Label | Key | Label | Key |
|-------|-----|-------|-----|
| Level | `level` | STR | `str` |
| EXP | `exp` | AGI | `agi` |
| JP (Job Points) | `jp` | DEX | `dex` |
| TP (Talent Points) | `tp` | INT | `int` |
| Gold | `gold` | LUCK | `luck` |
| HP | `hp` | VITAL | `vital` |
| MP | `mp` | MENTAL | `mental` |
| Free stat points | `free_statpoints` | Charisma | `charisma` |

Plus the **custom key** field for any other bound key. All keys/labels are lower-case stable
strings; the dropdown binds `Key` as the value so no parsing is needed.

## 7. Command construction

### 7.1 `App.Core/Commands/LuaCommands.cs` — add

```csharp
// Player value editing (sv/av). value emitted as integer (exp/gold/jp are int64 server-side).
public static string SetValueOwn(string key, long value)
    => Invariant($"sv('{LuaEscape.Single(key)}',{value})");
public static string SetValuePlayer(string key, long value, string playerName)
    => Invariant($"sv('{LuaEscape.Single(key)}',{value},'{LuaEscape.Single(playerName)}')");
public static string AddValueOwn(string key, long value)
    => Invariant($"av('{LuaEscape.Single(key)}',{value})");
public static string AddValuePlayer(string key, long value, string playerName)
    => Invariant($"av('{LuaEscape.Single(key)}',{value},'{LuaEscape.Single(playerName)}')");

// Notice / announce broadcasts. Freeform text -> Lua double-quoted (text often has apostrophes).
public static string Notice(string text)      => Invariant($"notice(\"{LuaEscape.Double(text)}\")");
public static string NoticeLeft(string text)  => Invariant($"notice_left(\"{LuaEscape.Double(text)}\")");
public static string NoticeRight(string text) => Invariant($"notice_right(\"{LuaEscape.Double(text)}\")");
public static string Announce(string text)    => Invariant($"announce(\"{LuaEscape.Double(text)}\")");
```

Keys and player names use `LuaEscape.Single` (single-quoted literals, matching existing
methods). Notice text uses `LuaEscape.Double` (double-quoted) so apostrophes in words don't
need escaping.

### 7.2 `App.Core/Commands/NoticeTextBuilder.cs` — new (pure)

```csharp
// Builds the notice payload (the string that goes inside notice("...")) from the builder controls.
public static string Build(string rawText, bool bold, string? colorHex6, int? size)
```

Logic:
1. `body = string.Join("<BR>", rawText.Replace("\r\n","\n").Replace("\r","\n").Split('\n'))`.
2. If `bold` → `body = $"<B>{body}</B>"`.
3. `prefix = (colorHex6 is not null ? $"<#{colorHex6}>" : "") + (size is not null ? $"<size:{size}>" : "")`.
4. return `prefix + body`.

`colorHex6` is 6 hex chars without `#` (normalized from the picker). The VM passes `null` when
the "custom color/size" toggles are off. The assembled payload is then wrapped by the matching
`LuaCommands.Notice*/Announce` method per the selected `NoticeKind`.

## 8. Error handling

- Character `Other` with no selected player → `IDialogService.ShowWarningAsync`, no copy.
- Character custom-key enabled but blank → warning, no copy.
- Notice empty text → warning, no copy.
- All dialogs go through the existing `IDialogService`. ReactiveCommand `ThrownExceptions`
  logged via Serilog (matching `RandomOptionTabViewModel`).

## 9. Testing strategy (`tests/App.Data.Tests`, references App.Core)

Follow `LuaRandomOptionCommandTests.cs` style:

1. `LuaCharacterCommandTests` — exact strings for `SetValueOwn/Player` and `AddValueOwn/Player`,
   including a large int64 value (`av('exp',9000000000)`), an "other player" case, and a key
   with an apostrophe (escaping).
2. `LuaNoticeCommandTests` — `Notice/NoticeLeft/NoticeRight/Announce` strings, plus escaping of
   `"` and `\` inside the text.
3. `NoticeTextBuilderTests` — multi-line → `<BR>`; bold wrap; color-only prefix; size-only
   prefix; combined color+size+bold; defaults (no toggles → no tags); `\r\n` normalization.
4. `PlayerValueCatalogTests` — keys non-empty, lower-case, unique (cheap guard).

VM/UI is not unit-tested (no desktop test project); dispatcher/player-context are exercised
through their interfaces, consistent with the existing tabs.

## 10. Out of scope (YAGNI)

- No live reading of current values (no `gv` round-trip) — the app's model is build-and-copy,
  with no live game connection.
- No full rich-text tag parser in the preview (only the builder's own controls are rendered).
- No `black_announce`, underline/glow/outline/strike buttons, emoticon picker, or font picker —
  manual tags cover the long tail; these can be added later if requested.
- No batch multi-attribute editing; no persistence of last-used form state.

## 11. Decisions & assumptions (confirmed with user)

- Character layout = unified builder **+ quick-action presets** that configure the builder. ✅
- Attribute dropdown = **curated list + custom-key** field. ✅
- Notice controls = **color + size + bold + alignment**, multi-line via **`<BR>` in one call**. ✅
- **Announce** variant added (green, centered, global). ✅
- `sv('level',N)` sets EXP to the level-N threshold — correct/intended behaviour (server source). ✅
- Values emitted as integer (`long`); notice text double-quoted; keys/players single-quoted. ✅
- Orders: Character = 15 (`fa-solid fa-user`), Notice = 100 (`fa-solid fa-bullhorn`).

## 12. File-by-file change list

**App.Core**
- `Commands/LuaCommands.cs` — add `SetValueOwn/Player`, `AddValueOwn/Player`,
  `Notice/NoticeLeft/NoticeRight/Announce`.
- `Commands/NoticeTextBuilder.cs` — new pure builder.
- `Characters/PlayerValueAttribute.cs` — record struct.
- `Characters/PlayerValueCatalog.cs` — curated attribute list.

**App.Desktop**
- `Features/Character/CharacterTabViewModel.cs` + `CharacterTabView.axaml(.cs)` + `CharacterOp.cs`.
- `Features/Notice/NoticeTabViewModel.cs` + `NoticeTabView.axaml(.cs)` + `NoticeKind.cs`.
- (No DI changes needed — tabs are reflection-discovered; all dependencies already registered.)

**tests/App.Data.Tests**
- `LuaCharacterCommandTests.cs`, `LuaNoticeCommandTests.cs`, `NoticeTextBuilderTests.cs`,
  `PlayerValueCatalogTests.cs`.
