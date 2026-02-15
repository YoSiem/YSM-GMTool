namespace App.Core.Interfaces;

public interface ICommandHistoryService
{
    IReadOnlyList<string> Commands { get; }

    event EventHandler? CommandsChanged;

    void Add(string command);

    void Clear();
}
