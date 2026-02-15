using System.Collections.ObjectModel;
using App.Core.Interfaces;

namespace App.Core.Services;

public sealed class CommandHistoryService : ICommandHistoryService
{
    private readonly List<string> _commands = [];

    public IReadOnlyList<string> Commands => new ReadOnlyCollection<string>(_commands);

    public event EventHandler? CommandsChanged;

    public void Add(string command)
    {
        if (string.IsNullOrWhiteSpace(command))
        {
            return;
        }

        _commands.Add(command.Trim());
        CommandsChanged?.Invoke(this, EventArgs.Empty);
    }

    public void Clear()
    {
        if (_commands.Count == 0)
        {
            return;
        }

        _commands.Clear();
        CommandsChanged?.Invoke(this, EventArgs.Empty);
    }
}
