# Random Option Tab Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Add a "Random Option" shell tab that builds `set_item_random_option(...)` Lua commands (Stat / Socket / Item effect) with a live stat bitflag grid, clipboard import, and a DB-backed item-effect picker.

**Architecture:** Pure logic (bitflag catalog, mask math, type derivation, command strings) lives in **App.Core** so `App.Data.Tests` can unit-test it. The data layer gains one query + repo method. The desktop layer gains small infra extensions (clipboard read, dispatcher format), a modal item-effect picker (service + window + VM), and the tab itself (`RandomOptionTabViewModel` + `RandomOptionTabView`), discovered by the existing reflection-based tab scan.

**Tech Stack:** .NET 10, C# 13, Avalonia 11 + ReactiveUI, Dapper, xUnit. Spec: `docs/superpowers/specs/2026-06-28-random-option-tab-design.md`.

## Global Constraints

- Target framework `net10.0` (desktop) / `net10.0` test project; `Nullable` enabled.
- Follow the existing per-tab MVVM pattern (`TabModuleViewModel` + `*View.axaml`); tabs are auto-discovered by reflection (`ServiceCollectionExtensions.AddTabModules`) — **no manual tab registration**.
- Views are resolved by `ReactiveViewLocator` via the `*ViewModel`→`*View` name swap and `Activator.CreateInstance` (parameterless View ctor) — **no DI registration for views**.
- All Lua command strings are built in `App.Core/Commands/LuaCommands.cs` using `Invariant($"...")` and `LuaEscape.Single(...)` for player names.
- All user dialogs go through `IDialogService`.
- The stat bitmask is carried as a **`long`** end-to-end (bit 31 = `FLAG_FINAL_DMG_REDUCTION` overflows `int`).
- Command emitted exactly as: `set_item_random_option(get_wear_item_handle(SLOT[,'PLAYER']),LINE,TYPE,OPTIONS,VALUE)`.
- Commit after every task. Build: `dotnet build` (from repo root `C:\Users\patry\Documents\Repos\YSM-GMTool`). Test: `dotnet test tests/App.Data.Tests/App.Data.Tests.csproj`.

---

### Task 1: Stat bitflag catalog + calculator (App.Core)

**Files:**
- Create: `src/App.Core/RandomOptions/RandomOptionFlag.cs`
- Create: `src/App.Core/RandomOptions/RandomOptionCatalog.cs`
- Create: `src/App.Core/RandomOptions/RandomOptionCalculator.cs`
- Test: `tests/App.Data.Tests/RandomOptionCalculatorTests.cs`

**Interfaces:**
- Produces:
  - `readonly record struct RandomOptionFlag(int Bit, string Label)`
  - `readonly record struct RandomOptionEquippart(int Index, string Label)`
  - `RandomOptionCatalog.Part1 : IReadOnlyList<RandomOptionFlag>` (bits 0..31)
  - `RandomOptionCatalog.Part2 : IReadOnlyList<RandomOptionFlag>` (bits 0..6, 8..30)
  - `RandomOptionCatalog.Equipparts : IReadOnlyList<RandomOptionEquippart>` (indices 0..30)
  - `long RandomOptionCalculator.ComputeMask(IEnumerable<int> checkedBits)`
  - `IReadOnlyList<int> RandomOptionCalculator.BitsFromMask(long mask, IReadOnlyList<RandomOptionFlag> catalog)`
  - `int RandomOptionCalculator.DeriveStatType(bool isPart2, bool isPercentage)`

- [ ] **Step 1: Write the failing test**

Create `tests/App.Data.Tests/RandomOptionCalculatorTests.cs`:

```csharp
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
```

- [ ] **Step 2: Run test to verify it fails**

Run: `dotnet test tests/App.Data.Tests/App.Data.Tests.csproj --filter "FullyQualifiedName~RandomOptionCalculatorTests"`
Expected: FAIL — does not compile (`RandomOptionCalculator` / `RandomOptionCatalog` not defined).

- [ ] **Step 3: Create `RandomOptionFlag.cs`**

```csharp
namespace App.Core.RandomOptions;

/// <summary>One selectable stat bitflag: its bit position and display label.</summary>
public readonly record struct RandomOptionFlag(int Bit, string Label);

/// <summary>One equippable wear slot: its slot index (emitted in the command) and display label.</summary>
public readonly record struct RandomOptionEquippart(int Index, string Label);
```

- [ ] **Step 4: Create `RandomOptionCatalog.cs`**

```csharp
namespace App.Core.RandomOptions;

/// <summary>
/// Static catalogs for the Random Option tab: the Part 1 / Part 2 stat bitflags (ported from the
/// Rappelz FLAG_* / FLAG_ET_* enums) and the equippable wear-slot list. Labels are readable, not
/// the verbatim enum names.
/// </summary>
public static class RandomOptionCatalog
{
    /// <summary>Stat option flags, bits 0..31 (FLAG_* enum).</summary>
    public static readonly IReadOnlyList<RandomOptionFlag> Part1 =
    [
        new(0, "Strength"), new(1, "Vitality"), new(2, "Agility"), new(3, "Dexterity"),
        new(4, "Intelligence"), new(5, "Wisdom"), new(6, "Luck"), new(7, "p.Atk"),
        new(8, "m.Atk"), new(9, "P.Def"), new(10, "M.Def"), new(11, "Atk Speed"),
        new(12, "Cast Speed"), new(13, "Move Speed"), new(14, "Accuracy"), new(15, "m.Acc"),
        new(16, "Crit Chance"), new(17, "Block Chance"), new(18, "Block Def"), new(19, "Evasion"),
        new(20, "m.Res"), new(21, "Max HP"), new(22, "Max MP"), new(23, "Max SP"),
        new(24, "HP Regen"), new(25, "MP Regen"), new(26, "SP Regen"), new(27, "HP Regen Ratio"),
        new(28, "MP Regen Ratio"), new(29, "Final Dmg Increase"), new(30, "Max Weight"),
        new(31, "Final Dmg Reduction"),
    ];

    /// <summary>Elemental / additional option flags (FLAG_ET_* enum). Bit 7 has no flag.</summary>
    public static readonly IReadOnlyList<RandomOptionFlag> Part2 =
    [
        new(0, "None Resist"), new(1, "Fire Resist"), new(2, "Water Resist"), new(3, "Wind Resist"),
        new(4, "Earth Resist"), new(5, "Light Resist"), new(6, "Dark Resist"),
        new(8, "Attack Range"), new(9, "Perfect Block"), new(10, "Ignore P.Def"), new(11, "Ignore M.Def"),
        new(12, "Physical Penetration"), new(13, "Magical Penetration"), new(14, "None Damage"),
        new(15, "Fire Damage"), new(16, "Water Damage"), new(17, "Wind Damage"), new(18, "Earth Damage"),
        new(19, "Light Damage"), new(20, "Dark Damage"), new(21, "None Add. Damage"), new(22, "Fire Add. Damage"),
        new(23, "Water Add. Damage"), new(24, "Wind Add. Damage"), new(25, "Earth Add. Damage"),
        new(26, "Light Add. Damage"), new(27, "Dark Add. Damage"), new(28, "Crit Damage"),
        new(29, "HP Regen Stop"), new(30, "MP Regen Stop"),
    ];

    /// <summary>Equippable wear slots (readable name -> slot index emitted in the command).</summary>
    public static readonly IReadOnlyList<RandomOptionEquippart> Equipparts =
    [
        new(0, "Weapon"), new(1, "Shield"), new(2, "Armor"), new(3, "Helmet"), new(4, "Gloves"),
        new(5, "Boots"), new(6, "Belt"), new(7, "Mantle"), new(8, "Amulet"), new(9, "Ring"),
        new(10, "Second Ring"), new(11, "Earring"), new(12, "Face"), new(13, "Backpack"),
        new(14, "Deco Weapon"), new(15, "Deco Shield"), new(16, "Deco Armor"), new(17, "Deco Helmet"),
        new(18, "Deco Gloves"), new(19, "Deco Boots"), new(20, "Deco Mantle"), new(21, "Deco Shoulder"),
        new(22, "Ride Item"), new(23, "Bag Slot"), new(24, "Deco Booster"), new(25, "Deco Emblem"),
        new(26, "Second Earring"), new(27, "Chaos Stone"), new(28, "Medal"), new(29, "Mask"), new(30, "Wings"),
    ];
}
```

- [ ] **Step 5: Create `RandomOptionCalculator.cs`**

```csharp
namespace App.Core.RandomOptions;

/// <summary>Pure bitmask + option-type math for the Random Option tab (no UI dependencies).</summary>
public static class RandomOptionCalculator
{
    /// <summary>OR of (1L &lt;&lt; bit) over the given bits. Uses long so bit 31 doesn't overflow.</summary>
    public static long ComputeMask(IEnumerable<int> checkedBits)
    {
        long mask = 0;
        foreach (var bit in checkedBits)
        {
            if (bit is >= 0 and < 32)
            {
                mask |= 1L << bit;
            }
        }

        return mask;
    }

    /// <summary>The bits from <paramref name="catalog"/> that are set in <paramref name="mask"/>.</summary>
    public static IReadOnlyList<int> BitsFromMask(long mask, IReadOnlyList<RandomOptionFlag> catalog)
    {
        var bits = new List<int>();
        foreach (var flag in catalog)
        {
            if ((mask & (1L << flag.Bit)) != 0)
            {
                bits.Add(flag.Bit);
            }
        }

        return bits;
    }

    /// <summary>Stat-effect option type: Part1 fix=96/pct=98, Part2 fix=97/pct=99.</summary>
    public static int DeriveStatType(bool isPart2, bool isPercentage) => (isPart2, isPercentage) switch
    {
        (false, false) => 96,
        (false, true) => 98,
        (true, false) => 97,
        (true, true) => 99,
    };
}
```

- [ ] **Step 6: Run test to verify it passes**

Run: `dotnet test tests/App.Data.Tests/App.Data.Tests.csproj --filter "FullyQualifiedName~RandomOptionCalculatorTests"`
Expected: PASS (7 tests).

- [ ] **Step 7: Commit**

```bash
git add src/App.Core/RandomOptions/ tests/App.Data.Tests/RandomOptionCalculatorTests.cs
git commit -m "feat(random-option): add stat bitflag catalog and calculator"
```

---

### Task 2: Lua command builders (App.Core)

**Files:**
- Modify: `src/App.Core/Commands/LuaCommands.cs`
- Test: `tests/App.Data.Tests/LuaRandomOptionCommandTests.cs`

**Interfaces:**
- Consumes: `LuaEscape.Single` (existing).
- Produces:
  - `string LuaCommands.SetItemRandomOptionOwn(int wearSlot, int line, int type, long options, double value)`
  - `string LuaCommands.SetItemRandomOptionPlayer(int wearSlot, string playerName, int line, int type, long options, double value)`

- [ ] **Step 1: Write the failing test**

Create `tests/App.Data.Tests/LuaRandomOptionCommandTests.cs`:

```csharp
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
```

> Note: the expected escaping in the last test must match `LuaEscape.Single`. If that helper escapes a single quote differently, update the expected string to match the helper's actual output (the helper is the source of truth).

- [ ] **Step 2: Run test to verify it fails**

Run: `dotnet test tests/App.Data.Tests/App.Data.Tests.csproj --filter "FullyQualifiedName~LuaRandomOptionCommandTests"`
Expected: FAIL — does not compile (`SetItemRandomOptionOwn` not defined).

- [ ] **Step 3: Add the builders to `LuaCommands.cs`**

Add these members inside the `LuaCommands` class (e.g. after the `// Items` block, before `// Skills`):

```csharp
    // Random options (set_item_random_option). options is a long to hold the 32-bit stat
    // bitmask (bit 31 = FLAG_FINAL_DMG_REDUCTION) without int overflow.
    public static string SetItemRandomOptionOwn(int wearSlot, int line, int type, long options, double value)
        => Invariant($"set_item_random_option(get_wear_item_handle({wearSlot}),{line},{type},{options},{FormatValue(value)})");
    public static string SetItemRandomOptionPlayer(int wearSlot, string playerName, int line, int type, long options, double value)
        => Invariant($"set_item_random_option(get_wear_item_handle({wearSlot},'{LuaEscape.Single(playerName)}'),{line},{type},{options},{FormatValue(value)})");

    // Value (fValue2) is a double; emit whole numbers without a decimal point and trim trailing zeros.
    private static string FormatValue(double value)
        => value.ToString("0.######", System.Globalization.CultureInfo.InvariantCulture);
```

- [ ] **Step 4: Run test to verify it passes**

Run: `dotnet test tests/App.Data.Tests/App.Data.Tests.csproj --filter "FullyQualifiedName~LuaRandomOptionCommandTests"`
Expected: PASS (4 tests). If the escape test fails, correct the expected string to match `LuaEscape.Single`.

- [ ] **Step 5: Commit**

```bash
git add src/App.Core/Commands/LuaCommands.cs tests/App.Data.Tests/LuaRandomOptionCommandTests.cs
git commit -m "feat(random-option): add set_item_random_option command builders"
```

---

### Task 3: Item-effect data layer (App.Core + App.Data + queries)

**Files:**
- Create: `src/App.Core/Models/Entities/ItemEffectRecord.cs`
- Modify: `src/App.Core/Enums/QueryEntity.cs`
- Modify: `src/App.Core/Abstractions/IGameDataRepository.cs`
- Modify: `src/App.Core/Services/FileQueryStore.cs:61-77` (add `ItemEffects` case)
- Modify: `src/App.Data/Repositories/GameDataRepository.cs`
- Modify: `src/App.Desktop/Config/queries.json`
- Test: `tests/App.Data.Tests/ItemEffectQueryStoreTests.cs`

**Interfaces:**
- Produces:
  - `sealed class ItemEffectRecord { int EffectId; string EffectText; }`
  - `QueryEntity.ItemEffects`
  - `Task<IReadOnlyList<ItemEffectRecord>> IGameDataRepository.GetItemEffectsAsync(DatabaseProvider, string, IReadOnlyDictionary<string,string>?, CancellationToken)`

- [ ] **Step 1: Write the failing test**

Create `tests/App.Data.Tests/ItemEffectQueryStoreTests.cs`:

```csharp
using App.Core.Enums;
using App.Core.Services;
using Xunit;

namespace App.Data.Tests;

public class ItemEffectQueryStoreTests
{
    [Theory]
    [InlineData(DatabaseProvider.MSSQL, "SELECT mssql")]
    [InlineData(DatabaseProvider.MySQL, "SELECT mysql")]
    public void GetQuery_ItemEffects_ReturnsConfiguredQuery(DatabaseProvider provider, string expected)
    {
        var path = Path.GetTempFileName();
        try
        {
            File.WriteAllText(path, """
            {
              "MSSQL": { "ItemEffects": "SELECT mssql" },
              "MySQL": { "ItemEffects": "SELECT mysql" }
            }
            """);

            var store = new FileQueryStore(path);

            Assert.Equal(expected, store.GetQuery(provider, QueryEntity.ItemEffects));
        }
        finally
        {
            File.Delete(path);
        }
    }
}
```

- [ ] **Step 2: Run test to verify it fails**

Run: `dotnet test tests/App.Data.Tests/App.Data.Tests.csproj --filter "FullyQualifiedName~ItemEffectQueryStoreTests"`
Expected: FAIL — does not compile (`QueryEntity.ItemEffects` not defined).

- [ ] **Step 3: Add `QueryEntity.ItemEffects`**

In `src/App.Core/Enums/QueryEntity.cs`, add `ItemEffects` to the enum (after `Summons`):

```csharp
    Npc,
    Summons,
    ItemEffects
```

- [ ] **Step 4: Add the `FileQueryStore` key mapping**

In `src/App.Core/Services/FileQueryStore.cs`, add a case to `ResolveEntityKey` (before the `_ => throw ...` default):

```csharp
        QueryEntity.Summons => "Summons",
        QueryEntity.ItemEffects => "ItemEffects",
        _ => throw new ArgumentOutOfRangeException(nameof(entity), entity, "Unsupported query entity.")
```

- [ ] **Step 5: Run test to verify it passes**

Run: `dotnet test tests/App.Data.Tests/App.Data.Tests.csproj --filter "FullyQualifiedName~ItemEffectQueryStoreTests"`
Expected: PASS (2 tests).

- [ ] **Step 6: Create `ItemEffectRecord.cs`**

```csharp
namespace App.Core.Models.Entities;

/// <summary>One row of the item-effect picker (ItemEffectResource joined to the string table).</summary>
public sealed class ItemEffectRecord
{
    public int EffectId { get; init; }

    public string EffectText { get; init; } = string.Empty;
}
```

- [ ] **Step 7: Add the repository method to the interface**

In `src/App.Core/Abstractions/IGameDataRepository.cs`, add (after `GetSummonsAsync`):

```csharp
    Task<IReadOnlyList<ItemEffectRecord>> GetItemEffectsAsync(
        DatabaseProvider provider,
        string connectionString,
        IReadOnlyDictionary<string, string>? queryTokens = null,
        CancellationToken cancellationToken = default);
```

- [ ] **Step 8: Implement the repository method**

In `src/App.Data/Repositories/GameDataRepository.cs`, add (after `GetSummonsAsync`):

```csharp
    public Task<IReadOnlyList<ItemEffectRecord>> GetItemEffectsAsync(
        DatabaseProvider provider,
        string connectionString,
        IReadOnlyDictionary<string, string>? queryTokens = null,
        CancellationToken cancellationToken = default)
        => QueryAsync<ItemEffectRecord>(provider, connectionString, QueryEntity.ItemEffects, queryTokens, null, cancellationToken);
```

- [ ] **Step 9: Add the queries to `queries.json`**

In `src/App.Desktop/Config/queries.json`, inside the **`"MSSQL"`** object add this entry (e.g. right after the `"Items": ...` line):

```json
        "ItemEffects": "SELECT i.[id] AS EffectId, n.[value_en] AS EffectText FROM [{{ArcadiaName}}].dbo.ItemEffectResource i LEFT JOIN [{{ArcadiaName}}].dbo.{{StringResource}} n ON i.[tooltip_id] = n.[code] ORDER BY i.[id];",
```

Inside the **`"MySQL"`** object add (after its `"Items": ...` line):

```json
        "ItemEffects": "SELECT i.id AS EffectId, n.value_en AS EffectText FROM `{{ArcadiaName}}`.`ItemEffectResource` i LEFT JOIN `{{ArcadiaName}}`.`{{StringResource}}` n ON i.tooltip_id = n.code ORDER BY i.id;",
```

> ⚠️ Assumption from the spec (§13): `ItemEffectResource` is hardcoded; `StringResourceFull` maps to the `{{StringResource}}` token; the text column is `value_en`. Verify against the live DB when available — if the string table uses `value` instead, change `value_en` → `value` in both entries.

- [ ] **Step 10: Build to verify the data layer compiles**

Run: `dotnet build`
Expected: Build succeeded, 0 errors.

- [ ] **Step 11: Run the full test project (no regressions)**

Run: `dotnet test tests/App.Data.Tests/App.Data.Tests.csproj`
Expected: PASS (all tests, including the new ones).

- [ ] **Step 12: Commit**

```bash
git add src/App.Core/Models/Entities/ItemEffectRecord.cs src/App.Core/Enums/QueryEntity.cs src/App.Core/Abstractions/IGameDataRepository.cs src/App.Core/Services/FileQueryStore.cs src/App.Data/Repositories/GameDataRepository.cs src/App.Desktop/Config/queries.json tests/App.Data.Tests/ItemEffectQueryStoreTests.cs
git commit -m "feat(random-option): add item-effect query, record, and repository method"
```

---

### Task 4: Clipboard read (App.Desktop infra)

**Files:**
- Modify: `src/App.Desktop/Infrastructure/IClipboardService.cs`
- Modify: `src/App.Desktop/Infrastructure/AvaloniaClipboardService.cs`

**Interfaces:**
- Produces: `Task<string?> IClipboardService.GetTextAsync()`

> No unit test: this touches the Avalonia top-level clipboard (no desktop test project). Verified by build + later manual use of "Import options".

- [ ] **Step 1: Add `GetTextAsync` to the interface**

Replace the body of `src/App.Desktop/Infrastructure/IClipboardService.cs`:

```csharp
namespace App.Desktop.Infrastructure;

public interface IClipboardService
{
    Task SetTextAsync(string text);

    Task<string?> GetTextAsync();
}
```

- [ ] **Step 2: Implement `GetTextAsync`**

In `src/App.Desktop/Infrastructure/AvaloniaClipboardService.cs`, add the method after `SetTextAsync`:

```csharp
    public async Task<string?> GetTextAsync()
    {
        var top = (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow;
        return top?.Clipboard is { } cb ? await cb.GetTextAsync() : null;
    }
```

- [ ] **Step 3: Build**

Run: `dotnet build`
Expected: Build succeeded, 0 errors.

- [ ] **Step 4: Commit**

```bash
git add src/App.Desktop/Infrastructure/IClipboardService.cs src/App.Desktop/Infrastructure/AvaloniaClipboardService.cs
git commit -m "feat(random-option): add clipboard read for options import"
```

---

### Task 5: Dispatcher command formatter (App.Desktop)

**Files:**
- Modify: `src/App.Desktop/Services/ICommandDispatcher.cs`
- Modify: `src/App.Desktop/Services/CommandDispatcher.cs`

**Interfaces:**
- Produces: `string ICommandDispatcher.Format(string luaCommand)` — applies the same `/run` prefix logic `DispatchAsync` uses, so the tab's live preview matches what is copied.

> No unit test (Desktop project, no test host). Verified by build + the live preview at Task 7.

- [ ] **Step 1: Add `Format` to the interface**

Replace `src/App.Desktop/Services/ICommandDispatcher.cs`:

```csharp
namespace App.Desktop.Services;

/// <summary>The single funnel every generated Lua command flows through.</summary>
public interface ICommandDispatcher
{
    Task DispatchAsync(string luaCommand);

    /// <summary>The exact text <see cref="DispatchAsync"/> would copy (with the optional /run prefix).</summary>
    string Format(string luaCommand);
}
```

- [ ] **Step 2: Expose the prefix logic in `CommandDispatcher`**

Replace `src/App.Desktop/Services/CommandDispatcher.cs`:

```csharp
using App.Desktop.Infrastructure;

namespace App.Desktop.Services;

public sealed class CommandDispatcher(IClipboardService clipboard, IAppSettingsHolder settings) : ICommandDispatcher
{
    public Task DispatchAsync(string luaCommand) => clipboard.SetTextAsync(Format(luaCommand));

    public string Format(string command)
    {
        if (!settings.Current.AppendGeneratedCommands)
        {
            return command;
        }

        var t = command.TrimStart();
        if (t.StartsWith("//", StringComparison.Ordinal) || t.StartsWith("/run ", StringComparison.OrdinalIgnoreCase))
        {
            return command;
        }

        return "/run " + command;
    }
}
```

- [ ] **Step 3: Build**

Run: `dotnet build`
Expected: Build succeeded, 0 errors.

- [ ] **Step 4: Commit**

```bash
git add src/App.Desktop/Services/ICommandDispatcher.cs src/App.Desktop/Services/CommandDispatcher.cs
git commit -m "feat(random-option): expose dispatcher command formatter for live preview"
```

---

### Task 6: Item-effect picker (service + window + VM)

**Files:**
- Create: `src/App.Desktop/Infrastructure/IItemEffectPickerService.cs`
- Create: `src/App.Desktop/Infrastructure/ItemEffectPickerService.cs`
- Create: `src/App.Desktop/Features/RandomOption/ItemEffectPickerViewModel.cs`
- Create: `src/App.Desktop/Features/RandomOption/ItemEffectPickerWindow.axaml`
- Create: `src/App.Desktop/Features/RandomOption/ItemEffectPickerWindow.axaml.cs`
- Modify: `src/App.Desktop/Composition/ServiceCollectionExtensions.cs:36` (register the service)

**Interfaces:**
- Consumes: `IGameDataRepository.GetItemEffectsAsync` (Task 3), `ConnectionStringResolver`, `IDialogService`, `ItemEffectRecord`.
- Produces: `Task<ItemEffectRecord?> IItemEffectPickerService.PickAsync(CancellationToken)`.

> No unit test (Avalonia window). Verified by build + the manual run at Task 7.

- [ ] **Step 1: Create the service interface**

`src/App.Desktop/Infrastructure/IItemEffectPickerService.cs`:

```csharp
using App.Core.Models.Entities;

namespace App.Desktop.Infrastructure;

/// <summary>Opens a modal item-effect picker and returns the chosen effect (or null if cancelled).</summary>
public interface IItemEffectPickerService
{
    Task<ItemEffectRecord?> PickAsync(CancellationToken cancellationToken = default);
}
```

- [ ] **Step 2: Create the picker view model**

`src/App.Desktop/Features/RandomOption/ItemEffectPickerViewModel.cs`:

```csharp
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reactive.Linq;
using App.Core.Models.Entities;
using ReactiveUI;

namespace App.Desktop.Features.RandomOption;

/// <summary>Backs the modal item-effect picker: a filterable list of (Effect_id, Effecttext) rows.</summary>
public sealed class ItemEffectPickerViewModel : ReactiveObject
{
    private readonly IReadOnlyList<ItemEffectRecord> _all;
    private string _searchText = string.Empty;
    private ItemEffectRecord? _selected;

    public ItemEffectPickerViewModel(IReadOnlyList<ItemEffectRecord> effects)
    {
        _all = effects;
        Rows = new ObservableCollection<ItemEffectRecord>(effects);

        this.WhenAnyValue(x => x.SearchText)
            .Throttle(TimeSpan.FromMilliseconds(150), RxApp.MainThreadScheduler)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(_ => ApplyFilter());
    }

    public ObservableCollection<ItemEffectRecord> Rows { get; }

    public string SearchText
    {
        get => _searchText;
        set => this.RaiseAndSetIfChanged(ref _searchText, value);
    }

    public ItemEffectRecord? Selected
    {
        get => _selected;
        set => this.RaiseAndSetIfChanged(ref _selected, value);
    }

    private void ApplyFilter()
    {
        var q = SearchText.Trim();
        IEnumerable<ItemEffectRecord> filtered = _all;

        if (!string.IsNullOrWhiteSpace(q))
        {
            filtered = _all.Where(e =>
                e.EffectId.ToString(CultureInfo.InvariantCulture).Contains(q, StringComparison.OrdinalIgnoreCase)
                || (e.EffectText?.Contains(q, StringComparison.OrdinalIgnoreCase) ?? false));
        }

        Rows.Clear();
        foreach (var e in filtered)
        {
            Rows.Add(e);
        }
    }
}
```

- [ ] **Step 3: Create the picker window XAML**

`src/App.Desktop/Features/RandomOption/ItemEffectPickerWindow.axaml`:

```xml
<Window xmlns="https://github.com/avaloniaui"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:ro="clr-namespace:App.Desktop.Features.RandomOption"
        x:Class="App.Desktop.Features.RandomOption.ItemEffectPickerWindow"
        x:DataType="ro:ItemEffectPickerViewModel"
        x:CompileBindings="False"
        Title="Select Item Effect"
        Width="560" Height="520"
        WindowStartupLocation="CenterOwner"
        Background="{DynamicResource WindowBackground}">
    <Grid RowDefinitions="Auto,*,Auto" Margin="10">
        <TextBox Grid.Row="0"
                 Watermark="Search by id or text..."
                 Text="{Binding SearchText, Mode=TwoWay, UpdateSourceTrigger=PropertyChanged}" />

        <DataGrid x:Name="EffectsGrid"
                  Grid.Row="1"
                  Margin="0,8"
                  IsReadOnly="True"
                  SelectionMode="Single"
                  CanUserSortColumns="True"
                  CanUserReorderColumns="False"
                  CanUserResizeColumns="True"
                  AutoGenerateColumns="False"
                  HeadersVisibility="Column"
                  FontSize="12"
                  ItemsSource="{Binding Rows}"
                  SelectedItem="{Binding Selected, Mode=TwoWay}">
            <DataGrid.Columns>
                <DataGridTextColumn Header="Effect_id" Binding="{Binding EffectId}" Width="110" />
                <DataGridTextColumn Header="Effecttext" Binding="{Binding EffectText}" Width="*" />
            </DataGrid.Columns>
        </DataGrid>

        <StackPanel Grid.Row="2" Orientation="Horizontal" HorizontalAlignment="Right" Spacing="8">
            <Button Content="Cancel" Click="OnCancelClick" />
            <Button Classes="primary" Content="Select" Click="OnSelectClick" />
        </StackPanel>
    </Grid>
</Window>
```

- [ ] **Step 4: Create the picker window code-behind**

`src/App.Desktop/Features/RandomOption/ItemEffectPickerWindow.axaml.cs`:

```csharp
using App.Core.Models.Entities;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace App.Desktop.Features.RandomOption;

public partial class ItemEffectPickerWindow : Window
{
    public ItemEffectPickerWindow()
    {
        InitializeComponent();
        EffectsGrid.DoubleTapped += (_, _) => Confirm();
    }

    private void OnSelectClick(object? sender, RoutedEventArgs e) => Confirm();

    private void OnCancelClick(object? sender, RoutedEventArgs e) => Close(null);

    private void Confirm()
    {
        if (DataContext is ItemEffectPickerViewModel { Selected: { } record })
        {
            Close(record);
        }
    }
}
```

- [ ] **Step 5: Create the picker service implementation**

`src/App.Desktop/Infrastructure/ItemEffectPickerService.cs`:

```csharp
using App.Core.Abstractions;
using App.Core.Models.Entities;
using App.Desktop.Features.RandomOption;
using App.Desktop.Services;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;

namespace App.Desktop.Infrastructure;

/// <summary>Loads item effects from the live DB and shows them in a modal picker window.</summary>
public sealed class ItemEffectPickerService(
    IGameDataRepository repository,
    ConnectionStringResolver connection,
    IDialogService dialog) : IItemEffectPickerService
{
    public async Task<ItemEffectRecord?> PickAsync(CancellationToken cancellationToken = default)
    {
        IReadOnlyList<ItemEffectRecord> effects;
        try
        {
            effects = await repository.GetItemEffectsAsync(
                connection.Provider, connection.Resolve(), connection.Tokens(), cancellationToken);
        }
        catch (Exception ex)
        {
            await dialog.ShowErrorAsync("Item Effect", ex.Message);
            return null;
        }

        var window = new ItemEffectPickerWindow
        {
            DataContext = new ItemEffectPickerViewModel(effects),
        };

        var owner = (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow;
        return owner is not null ? await window.ShowDialog<ItemEffectRecord?>(owner) : null;
    }
}
```

- [ ] **Step 6: Register the service**

In `src/App.Desktop/Composition/ServiceCollectionExtensions.cs`, in the `// Desktop infra` block (after the `IInventoryWindowService` line), add:

```csharp
        s.AddSingleton<IItemEffectPickerService, ItemEffectPickerService>();
```

- [ ] **Step 7: Build**

Run: `dotnet build`
Expected: Build succeeded, 0 errors.

- [ ] **Step 8: Commit**

```bash
git add src/App.Desktop/Infrastructure/IItemEffectPickerService.cs src/App.Desktop/Infrastructure/ItemEffectPickerService.cs src/App.Desktop/Features/RandomOption/ItemEffectPickerViewModel.cs src/App.Desktop/Features/RandomOption/ItemEffectPickerWindow.axaml src/App.Desktop/Features/RandomOption/ItemEffectPickerWindow.axaml.cs src/App.Desktop/Composition/ServiceCollectionExtensions.cs
git commit -m "feat(random-option): add modal item-effect picker"
```

---

### Task 7: Random Option tab (VM + flag VM + View)

**Files:**
- Create: `src/App.Desktop/Features/RandomOption/RandomOptionKind.cs`
- Create: `src/App.Desktop/Features/RandomOption/RandomOptionFlagViewModel.cs`
- Create: `src/App.Desktop/Features/RandomOption/RandomOptionTabViewModel.cs`
- Create: `src/App.Desktop/Features/RandomOption/RandomOptionTabView.axaml`
- Create: `src/App.Desktop/Features/RandomOption/RandomOptionTabView.axaml.cs`

**Interfaces:**
- Consumes: `ICommandDispatcher` (`DispatchAsync`, `Format`), `IPlayerContext` (`TryResolveRequired`), `IDialogService`, `IClipboardService` (`GetTextAsync`), `IItemEffectPickerService` (`PickAsync`), `LuaCommands.SetItemRandomOption*`, `RandomOptionCatalog`, `RandomOptionCalculator`.
- Produces: a reflection-discovered `ITabModule` (`Title="Random Option"`, `Order=35`).

> No unit test (Desktop VM/UI). Verified by build + a manual app run (the tab appears; the command preview updates live; the picker opens).

- [ ] **Step 1: Create the mode enum**

`src/App.Desktop/Features/RandomOption/RandomOptionKind.cs`:

```csharp
namespace App.Desktop.Features.RandomOption;

public enum RandomOptionKind
{
    Stat,
    Socket,
    Item,
}
```

- [ ] **Step 2: Create the per-checkbox flag view model**

`src/App.Desktop/Features/RandomOption/RandomOptionFlagViewModel.cs`:

```csharp
using System;
using ReactiveUI;

namespace App.Desktop.Features.RandomOption;

/// <summary>One checkbox in the stat grid. Toggling notifies the owner so the mask recomputes;
/// <see cref="SetCheckedSilently"/> updates the box during import without firing per-box rebuilds.</summary>
public sealed class RandomOptionFlagViewModel : ReactiveObject
{
    private readonly Action _onToggled;
    private bool _isChecked;
    private bool _suppress;

    public RandomOptionFlagViewModel(int bit, string label, Action onToggled)
    {
        Bit = bit;
        Label = label;
        _onToggled = onToggled;
    }

    public int Bit { get; }

    public string Label { get; }

    public bool IsChecked
    {
        get => _isChecked;
        set
        {
            this.RaiseAndSetIfChanged(ref _isChecked, value);
            if (!_suppress)
            {
                _onToggled();
            }
        }
    }

    public void SetCheckedSilently(bool value)
    {
        _suppress = true;
        IsChecked = value;
        _suppress = false;
    }
}
```

- [ ] **Step 3: Create the tab view model**

`src/App.Desktop/Features/RandomOption/RandomOptionTabViewModel.cs`:

```csharp
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using App.Core.Commands;
using App.Core.RandomOptions;
using App.Desktop.Infrastructure;
using App.Desktop.Modules;
using App.Desktop.Services;
using ReactiveUI;
using Serilog;

namespace App.Desktop.Features.RandomOption;

/// <summary>
/// Builds a <c>set_item_random_option(...)</c> command for Stat / Socket / Item effects. The stat
/// mode drives the option bitmask from a checkbox grid; the item mode fills the options from a
/// DB-backed picker. The live <see cref="Command"/> preview mirrors what Copy puts on the clipboard.
/// </summary>
public sealed class RandomOptionTabViewModel : TabModuleViewModel
{
    private const string Title_ = "Random Option";

    private readonly ICommandDispatcher _cmd;
    private readonly IPlayerContext _player;
    private readonly IDialogService _dlg;
    private readonly IClipboardService _clipboard;
    private readonly IItemEffectPickerService _picker;

    private long _mask;

    public override string Title => Title_;

    public override string IconKey => "fa-solid fa-dice";

    public override int Order => 35;

    public RandomOptionTabViewModel(
        ICommandDispatcher cmd,
        IPlayerContext player,
        IDialogService dlg,
        IClipboardService clipboard,
        IItemEffectPickerService picker)
    {
        _cmd = cmd;
        _player = player;
        _dlg = dlg;
        _clipboard = clipboard;
        _picker = picker;

        Equipparts = RandomOptionCatalog.Equipparts;
        _selectedEquippart = Equipparts[0];

        CopyCommand = ReactiveCommand.CreateFromTask(CopyAsync);
        ImportOptions = ReactiveCommand.CreateFromTask(ImportAsync);
        PickEffect = ReactiveCommand.CreateFromTask(PickEffectAsync);

        CopyCommand.ThrownExceptions.Subscribe(ex => Log.Warning(ex, "Random Option: copy failed."));
        ImportOptions.ThrownExceptions.Subscribe(ex => Log.Warning(ex, "Random Option: import failed."));
        PickEffect.ThrownExceptions.Subscribe(ex => Log.Warning(ex, "Random Option: pick failed."));

        BuildFlags();
        Rebuild();
    }

    // --- Static lists for the view. ---

    public IReadOnlyList<RandomOptionEquippart> Equipparts { get; }

    public int[] Lines { get; } = Enumerable.Range(1, 10).ToArray();

    public int[] SocketTypes { get; } = [1, 2];

    public int[] SocketAmounts { get; } = [1, 2];

    public ObservableCollection<RandomOptionFlagViewModel> Flags { get; } = [];

    public ReactiveCommand<Unit, Unit> CopyCommand { get; }

    public ReactiveCommand<Unit, Unit> ImportOptions { get; }

    public ReactiveCommand<Unit, Unit> PickEffect { get; }

    // --- Mode. ---

    private RandomOptionKind _kind = RandomOptionKind.Stat;

    public RandomOptionKind Kind
    {
        get => _kind;
        set
        {
            if (_kind == value)
            {
                return;
            }

            _kind = value;
            this.RaisePropertyChanged(nameof(IsStatMode));
            this.RaisePropertyChanged(nameof(IsSocketMode));
            this.RaisePropertyChanged(nameof(IsItemMode));
            Rebuild();
        }
    }

    public bool IsStatMode { get => Kind == RandomOptionKind.Stat; set { if (value) { Kind = RandomOptionKind.Stat; } } }

    public bool IsSocketMode { get => Kind == RandomOptionKind.Socket; set { if (value) { Kind = RandomOptionKind.Socket; } } }

    public bool IsItemMode { get => Kind == RandomOptionKind.Item; set { if (value) { Kind = RandomOptionKind.Item; } } }

    // --- Universal inputs. ---

    private RandomOptionEquippart _selectedEquippart;

    public RandomOptionEquippart SelectedEquippart
    {
        get => _selectedEquippart;
        set { this.RaiseAndSetIfChanged(ref _selectedEquippart, value); Rebuild(); }
    }

    private int _selectedLine = 1;

    public int SelectedLine
    {
        get => _selectedLine;
        set { this.RaiseAndSetIfChanged(ref _selectedLine, value); Rebuild(); }
    }

    private bool _applyToOther;

    public bool ApplyToOther
    {
        get => _applyToOther;
        set { this.RaiseAndSetIfChanged(ref _applyToOther, value); Rebuild(); }
    }

    // --- Stat inputs. ---

    private bool _isPart2;

    public bool IsPart2
    {
        get => _isPart2;
        set { this.RaiseAndSetIfChanged(ref _isPart2, value); BuildFlags(); RecomputeMask(); Rebuild(); }
    }

    private bool _isPercentage;

    public bool IsPercentage
    {
        get => _isPercentage;
        set { this.RaiseAndSetIfChanged(ref _isPercentage, value); Rebuild(); }
    }

    private double _statValue;

    public double StatValue
    {
        get => _statValue;
        set { this.RaiseAndSetIfChanged(ref _statValue, value); Rebuild(); }
    }

    // --- Socket inputs. ---

    private int _socketType = 1;

    public int SocketType
    {
        get => _socketType;
        set { this.RaiseAndSetIfChanged(ref _socketType, value); Rebuild(); }
    }

    private int _socketAmount = 1;

    public int SocketAmount
    {
        get => _socketAmount;
        set { this.RaiseAndSetIfChanged(ref _socketAmount, value); Rebuild(); }
    }

    // --- Item inputs. ---

    private int? _itemEffectId;

    public int? ItemEffectId
    {
        get => _itemEffectId;
        private set { this.RaiseAndSetIfChanged(ref _itemEffectId, value); Rebuild(); }
    }

    private string _effectText = string.Empty;

    public string EffectText
    {
        get => _effectText;
        private set => this.RaiseAndSetIfChanged(ref _effectText, value);
    }

    private double _itemValue = 1;

    public double ItemValue
    {
        get => _itemValue;
        set { this.RaiseAndSetIfChanged(ref _itemValue, value); Rebuild(); }
    }

    // --- Derived (read-only display). ---

    private int _type;

    public int Type
    {
        get => _type;
        private set => this.RaiseAndSetIfChanged(ref _type, value);
    }

    private long _optionsDisplay;

    public long OptionsDisplay
    {
        get => _optionsDisplay;
        private set => this.RaiseAndSetIfChanged(ref _optionsDisplay, value);
    }

    private string _command = string.Empty;

    public string Command
    {
        get => _command;
        private set => this.RaiseAndSetIfChanged(ref _command, value);
    }

    // --- Logic. ---

    private void BuildFlags()
    {
        Flags.Clear();
        var catalog = IsPart2 ? RandomOptionCatalog.Part2 : RandomOptionCatalog.Part1;
        foreach (var flag in catalog)
        {
            Flags.Add(new RandomOptionFlagViewModel(flag.Bit, flag.Label, OnFlagToggled));
        }
    }

    private void OnFlagToggled()
    {
        RecomputeMask();
        Rebuild();
    }

    private void RecomputeMask()
        => _mask = RandomOptionCalculator.ComputeMask(Flags.Where(f => f.IsChecked).Select(f => f.Bit));

    private int CurrentType() => Kind switch
    {
        RandomOptionKind.Stat => RandomOptionCalculator.DeriveStatType(IsPart2, IsPercentage),
        RandomOptionKind.Socket => 130,
        RandomOptionKind.Item => 133,
        _ => 0,
    };

    private long CurrentOptions() => Kind switch
    {
        RandomOptionKind.Stat => _mask,
        RandomOptionKind.Socket => SocketType,
        RandomOptionKind.Item => ItemEffectId ?? 0,
        _ => 0,
    };

    private double CurrentValue() => Kind switch
    {
        RandomOptionKind.Stat => StatValue,
        RandomOptionKind.Socket => SocketAmount,
        RandomOptionKind.Item => ItemValue,
        _ => 0,
    };

    private string BuildRaw()
    {
        var type = CurrentType();
        var options = CurrentOptions();
        var value = CurrentValue();
        var slot = SelectedEquippart.Index;

        return ApplyToOther && _player.TryResolveRequired(out var p)
            ? LuaCommands.SetItemRandomOptionPlayer(slot, p, SelectedLine, type, options, value)
            : LuaCommands.SetItemRandomOptionOwn(slot, SelectedLine, type, options, value);
    }

    private void Rebuild()
    {
        Type = CurrentType();
        OptionsDisplay = CurrentOptions();
        Command = _cmd.Format(BuildRaw());
    }

    private async Task CopyAsync()
    {
        if (ApplyToOther && !_player.TryResolveRequired(out _))
        {
            await _dlg.ShowWarningAsync(Title_, "Select player in the right sidebar for 'Other' target.");
            return;
        }

        if (Kind == RandomOptionKind.Item && (ItemEffectId is null || ItemEffectId <= 0))
        {
            await _dlg.ShowWarningAsync(Title_, "Pick an item effect first.");
            return;
        }

        await _cmd.DispatchAsync(BuildRaw());
    }

    private async Task ImportAsync()
    {
        var text = await _clipboard.GetTextAsync();
        if (string.IsNullOrWhiteSpace(text)
            || !long.TryParse(text.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var mask)
            || mask < 0)
        {
            await _dlg.ShowWarningAsync(Title_, "Clipboard does not contain a valid options number.");
            return;
        }

        var catalog = IsPart2 ? RandomOptionCatalog.Part2 : RandomOptionCatalog.Part1;
        var bits = RandomOptionCalculator.BitsFromMask(mask, catalog).ToHashSet();
        foreach (var f in Flags)
        {
            f.SetCheckedSilently(bits.Contains(f.Bit));
        }

        RecomputeMask();
        Rebuild();
    }

    private async Task PickEffectAsync()
    {
        var picked = await _picker.PickAsync();
        if (picked is null)
        {
            return;
        }

        EffectText = picked.EffectText;
        ItemEffectId = picked.EffectId; // setter triggers Rebuild
    }
}
```

- [ ] **Step 4: Create the tab view XAML**

`src/App.Desktop/Features/RandomOption/RandomOptionTabView.axaml`:

```xml
<UserControl xmlns="https://github.com/avaloniaui"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:ro="clr-namespace:App.Desktop.Features.RandomOption"
             x:Class="App.Desktop.Features.RandomOption.RandomOptionTabView"
             x:DataType="ro:RandomOptionTabViewModel"
             x:CompileBindings="False"
             Background="{DynamicResource WindowBackground}">
    <ScrollViewer>
        <Grid ColumnDefinitions="*,Auto,Auto" Margin="12" >
            <!-- Left: builder form -->
            <StackPanel Grid.Column="0" Spacing="8" MinWidth="430">

                <TextBlock Classes="sectionHeader" Text="Random Type" />
                <Border Classes="sectionDivider" />
                <StackPanel Orientation="Horizontal" Spacing="14">
                    <RadioButton GroupName="roKind" Content="Stat-Effect" IsChecked="{Binding IsStatMode}" />
                    <RadioButton GroupName="roKind" Content="Socket-Effect" IsChecked="{Binding IsSocketMode}" />
                    <RadioButton GroupName="roKind" Content="Item-Effect" IsChecked="{Binding IsItemMode}" />
                </StackPanel>

                <WrapPanel>
                    <StackPanel Classes="field" Width="220">
                        <TextBlock Classes="fieldLabel" Text="Equippart (equipped)" />
                        <ComboBox HorizontalAlignment="Stretch"
                                  ItemsSource="{Binding Equipparts}"
                                  SelectedItem="{Binding SelectedEquippart}">
                            <ComboBox.ItemTemplate>
                                <DataTemplate>
                                    <TextBlock Text="{Binding Label}" />
                                </DataTemplate>
                            </ComboBox.ItemTemplate>
                        </ComboBox>
                    </StackPanel>
                    <StackPanel Classes="field" Width="90">
                        <TextBlock Classes="fieldLabel" Text="Line" />
                        <ComboBox HorizontalAlignment="Stretch"
                                  ItemsSource="{Binding Lines}"
                                  SelectedItem="{Binding SelectedLine}" />
                    </StackPanel>
                </WrapPanel>

                <StackPanel Orientation="Horizontal" Spacing="12">
                    <RadioButton GroupName="roTarget" Content="Own" IsChecked="{Binding !ApplyToOther}" />
                    <RadioButton GroupName="roTarget" Content="Other" IsChecked="{Binding ApplyToOther}" />
                </StackPanel>

                <!-- Stat-only options -->
                <StackPanel Spacing="8" IsVisible="{Binding IsStatMode}">
                    <WrapPanel>
                        <StackPanel Classes="field" Margin="0,0,16,0">
                            <TextBlock Classes="fieldLabel" Text="Options" />
                            <StackPanel Orientation="Horizontal" Spacing="12">
                                <RadioButton GroupName="roPart" Content="Part 1" IsChecked="{Binding !IsPart2}" />
                                <RadioButton GroupName="roPart" Content="Part 2" IsChecked="{Binding IsPart2}" />
                            </StackPanel>
                        </StackPanel>
                        <StackPanel Classes="field">
                            <TextBlock Classes="fieldLabel" Text="OptTypes" />
                            <StackPanel Orientation="Horizontal" Spacing="12">
                                <RadioButton GroupName="roOpt" Content="Fix Value" IsChecked="{Binding !IsPercentage}" />
                                <RadioButton GroupName="roOpt" Content="Percentage" IsChecked="{Binding IsPercentage}" />
                            </StackPanel>
                        </StackPanel>
                    </WrapPanel>

                    <StackPanel Classes="field" Width="160">
                        <TextBlock Classes="fieldLabel" Text="Value" />
                        <NumericUpDown Minimum="-1000000000" Maximum="1000000000"
                                       Increment="1" FormatString="0.######"
                                       Value="{Binding StatValue}" />
                    </StackPanel>

                    <Button Content="Import options" Command="{Binding ImportOptions}" HorizontalAlignment="Left" />
                </StackPanel>

                <!-- Socket-only -->
                <WrapPanel IsVisible="{Binding IsSocketMode}">
                    <StackPanel Classes="field" Width="120">
                        <TextBlock Classes="fieldLabel" Text="Socket Type" />
                        <ComboBox HorizontalAlignment="Stretch"
                                  ItemsSource="{Binding SocketTypes}"
                                  SelectedItem="{Binding SocketType}" />
                    </StackPanel>
                    <StackPanel Classes="field" Width="120">
                        <TextBlock Classes="fieldLabel" Text="Amount" />
                        <ComboBox HorizontalAlignment="Stretch"
                                  ItemsSource="{Binding SocketAmounts}"
                                  SelectedItem="{Binding SocketAmount}" />
                    </StackPanel>
                </WrapPanel>

                <!-- Item-only -->
                <StackPanel Spacing="8" IsVisible="{Binding IsItemMode}">
                    <StackPanel Orientation="Horizontal" Spacing="10">
                        <Button Content="Pick effect..." Command="{Binding PickEffect}" />
                        <TextBlock VerticalAlignment="Center"
                                   Foreground="{DynamicResource MutedForeground}"
                                   Text="{Binding EffectText}" />
                    </StackPanel>
                    <StackPanel Classes="field" Width="160">
                        <TextBlock Classes="fieldLabel" Text="Value" />
                        <NumericUpDown Minimum="-1000000000" Maximum="1000000000"
                                       Increment="1" FormatString="0.######"
                                       Value="{Binding ItemValue}" />
                    </StackPanel>
                </StackPanel>

                <!-- Derived row -->
                <WrapPanel>
                    <StackPanel Classes="field" Width="90">
                        <TextBlock Classes="fieldLabel" Text="Type" />
                        <TextBox IsReadOnly="True" Text="{Binding Type}" />
                    </StackPanel>
                    <StackPanel Classes="field" Width="180">
                        <TextBlock Classes="fieldLabel" Text="Options" />
                        <TextBox IsReadOnly="True" Text="{Binding OptionsDisplay}" />
                    </StackPanel>
                </WrapPanel>

                <TextBlock Classes="sectionHeader" Text="Command" Margin="0,8,0,0" />
                <Border Classes="sectionDivider" />
                <TextBox IsReadOnly="True" AcceptsReturn="True" TextWrapping="Wrap"
                         FontFamily="Consolas, monospace" MinHeight="46"
                         Text="{Binding Command}" />
                <TextBlock Foreground="{DynamicResource MutedForeground}" FontSize="11"
                           Text="set_item_random_option(get_wear_item_handle(SLOT[,'player']),LINE,TYPE,OPTIONS,VALUE)" />

                <Button Classes="primary" Content="Copy Command to Clipboard"
                        Command="{Binding CopyCommand}" HorizontalAlignment="Left" Margin="0,4,0,0" />
            </StackPanel>

            <Border Grid.Column="1" Classes="sectionDivider" Width="1" Margin="14,0"
                    IsVisible="{Binding IsStatMode}" />

            <!-- Right: stat checkbox grid -->
            <StackPanel Grid.Column="2" MinWidth="320" IsVisible="{Binding IsStatMode}">
                <TextBlock Classes="sectionHeader" Text="Stat flags" />
                <Border Classes="sectionDivider" />
                <ItemsControl ItemsSource="{Binding Flags}" Margin="0,6,0,0">
                    <ItemsControl.ItemsPanel>
                        <ItemsPanelTemplate>
                            <UniformGrid Columns="2" />
                        </ItemsPanelTemplate>
                    </ItemsControl.ItemsPanel>
                    <ItemsControl.ItemTemplate>
                        <DataTemplate>
                            <CheckBox Margin="0,1" Content="{Binding Label}" IsChecked="{Binding IsChecked}" />
                        </DataTemplate>
                    </ItemsControl.ItemTemplate>
                </ItemsControl>
            </StackPanel>
        </Grid>
    </ScrollViewer>
</UserControl>
```

- [ ] **Step 5: Create the tab view code-behind**

`src/App.Desktop/Features/RandomOption/RandomOptionTabView.axaml.cs`:

```csharp
using Avalonia.Controls;

namespace App.Desktop.Features.RandomOption;

public partial class RandomOptionTabView : UserControl
{
    public RandomOptionTabView()
    {
        InitializeComponent();
    }
}
```

- [ ] **Step 6: Build**

Run: `dotnet build`
Expected: Build succeeded, 0 errors.

- [ ] **Step 7: Run the app and verify the tab**

Run: `dotnet run --project src/App.Desktop`
Expected (manual check):
- A "Random Option" tab appears between "Items" and "Skills".
- Default mode Stat-Effect shows the checkbox grid; Type reads 96.
- Checking "Strength" + "Vitality" sets Options to 3 and the Command preview updates live to
  `/run set_item_random_option(get_wear_item_handle(0),1,96,3,0)` (prefix present only if the
  "append /run" setting is on).
- Switching Part 2 / Percentage changes Type to 97/99 and relabels the grid (no bit-7 entry).
- Socket-Effect shows the two dropdowns; Type reads 130.
- Item-Effect shows "Pick effect..."; Type reads 133. (The picker needs a configured DB; with
  none, it shows an error dialog — that is expected.)
Close the app when done.

- [ ] **Step 8: Commit**

```bash
git add src/App.Desktop/Features/RandomOption/RandomOptionKind.cs src/App.Desktop/Features/RandomOption/RandomOptionFlagViewModel.cs src/App.Desktop/Features/RandomOption/RandomOptionTabViewModel.cs src/App.Desktop/Features/RandomOption/RandomOptionTabView.axaml src/App.Desktop/Features/RandomOption/RandomOptionTabView.axaml.cs
git commit -m "feat(random-option): add Random Option tab (view model + view)"
```

---

## Self-Review

**Spec coverage:**
- §3 placement (Order 35, dice icon, reflection discovery) → Task 7. ✅
- §4 field model per mode (Stat/Socket/Item, target Own/Other, live preview, copy) → Task 7. ✅
- §5 command builders + dispatcher Format → Tasks 2, 5. ✅
- §6 bitflag catalog + calculator → Task 1. ✅
- §7 equippart catalog → Task 1 (`RandomOptionCatalog.Equipparts`). ✅
- §8 item-effect picker (record, query, repo, window, service) → Tasks 3, 6. ✅
- §9 infra (`GetTextAsync`, `Format`, picker service) → Tasks 4, 5, 6. ✅
- §10 error handling (other-without-player, item-without-effect, bad import, picker DB error) → Tasks 6, 7. ✅
- §11 testing (mask, type, command, query store) → Tasks 1, 2, 3. ✅

**Type consistency:** `long` options carried through `RandomOptionCalculator.ComputeMask` → `LuaCommands.SetItemRandomOption*` → VM `CurrentOptions()`/`OptionsDisplay`. `RandomOptionEquippart.Index`, `RandomOptionFlag.Bit`, `IItemEffectPickerService.PickAsync`, `IClipboardService.GetTextAsync`, `ICommandDispatcher.Format` names match between producing and consuming tasks. ✅

**Placeholder scan:** No TBD/TODO; every code step shows complete code. The one flagged item (`value_en` column) is an explicit, documented assumption with a concrete fallback, not a placeholder. ✅

**Open assumption to verify during/after implementation:** the item-effect text column (`value_en` vs `value`) — Task 3, Step 9.
