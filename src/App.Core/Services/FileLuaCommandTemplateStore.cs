using System.Text.Json;
using App.Core.Interfaces;

namespace App.Core.Services;

public sealed class FileLuaCommandTemplateStore : ILuaCommandTemplateStore
{
    private readonly Dictionary<string, string> _templates;

    public FileLuaCommandTemplateStore(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("Lua command template file was not found.", filePath);
        }

        var json = File.ReadAllText(filePath);
        var parsed = JsonSerializer.Deserialize<Dictionary<string, string>>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        _templates = parsed is null
            ? throw new InvalidOperationException("Lua command template configuration is empty.")
            : new Dictionary<string, string>(parsed, StringComparer.OrdinalIgnoreCase);
    }

    public string GetTemplate(string templateKey)
    {
        if (!_templates.TryGetValue(templateKey, out var template) || string.IsNullOrWhiteSpace(template))
        {
            throw new KeyNotFoundException($"Lua template '{templateKey}' was not found.");
        }

        return template;
    }
}
