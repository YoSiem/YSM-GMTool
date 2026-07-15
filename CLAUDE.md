# GM Tool — project notes for Claude

## Publishing — DO THIS AFTER ANY CHANGE
After **any** change to the app, publish a fresh build so the user can run it from the desktop.
Do not consider a change "done" until it has been published.

```
pwsh scripts/publish-release.ps1 -PublishDir "C:\Users\patry\Desktop\GM Tool"
```

- Target: `C:\Users\patry\Desktop\GM Tool` — note the **space**, not a hyphen (NOT `GM-Tool`).
  Always pass this `-PublishDir` explicitly; the script's default points elsewhere
  (`Documents\YSMReleasedTools\GM-Tool`).
- Reuse the existing folder; the script wipes + recreates it in place.
- The script auto-kills a `GM Tool.exe` running from the target folder, wipes the dir, then runs
  `dotnet publish -c Release -r win-x64 --self-contained false` (framework-dependent win-x64).
- If publish fails on a locked file, an instance launched from another folder is running — ask the
  user to close "GM Tool" first.

## Build & test
- No `.sln`; build per-project. Desktop: `dotnet build src/App.Desktop/App.Desktop.csproj`.
- Tests: `dotnet test tests/App.Data.Tests/App.Data.Tests.csproj` (xUnit; references App.Core).
  Pure logic that needs testing lives in **App.Core** so this project can reach it.

## Architecture (brief)
- Avalonia 11 + ReactiveUI, .NET 10, MVVM. Solution layout: `App.Core` (pure logic, models,
  Lua command builders), `App.Data` (Dapper repositories), `App.Desktop` (Avalonia UI).
- Tabs are `TabModuleViewModel` subclasses, auto-discovered by reflection
  (`Composition/ServiceCollectionExtensions.AddTabModules`) and ordered by `Order`. Views resolve
  by name via `ReactiveViewLocator` (`*ViewModel` → `*View`).
- Commands are built as Lua strings in `App.Core/Commands/LuaCommands.cs` and dispatched through
  `ICommandDispatcher` (copies to clipboard, optionally prefixed with `/run`). The app has no live
  game connection — the model is build-command → copy → paste into the in-game GM console.
- Lua semantics are verified against the C++ server source at
  `C:\Users\patry\Desktop\RZ-HeavenSource\Game-Server` (source of truth for GM command behavior).
```
