namespace App.Desktop.Services;

/// <summary>The single funnel every generated Lua command flows through.</summary>
public interface ICommandDispatcher
{
    Task DispatchAsync(string luaCommand);

    /// <summary>The exact text <see cref="DispatchAsync"/> would copy (with the optional /run prefix).</summary>
    string Format(string luaCommand);
}
