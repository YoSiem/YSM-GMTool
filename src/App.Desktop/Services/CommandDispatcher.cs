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
