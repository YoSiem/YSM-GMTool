namespace App.Core.Interfaces;

public interface ILuaCommandBuilder
{
    string Build(string templateKey, IReadOnlyDictionary<string, object?> values);
}
