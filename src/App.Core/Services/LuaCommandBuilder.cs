using System.Globalization;
using System.Text.RegularExpressions;
using App.Core.Interfaces;

namespace App.Core.Services;

public sealed partial class LuaCommandBuilder(ILuaCommandTemplateStore templateStore) : ILuaCommandBuilder
{
    private readonly ILuaCommandTemplateStore _templateStore = templateStore;

    [GeneratedRegex("\\{(?<key>[a-zA-Z0-9_]+)\\}", RegexOptions.Compiled)]
    private static partial Regex PlaceholderRegex();

    public string Build(string templateKey, IReadOnlyDictionary<string, object?> values)
    {
        var template = _templateStore.GetTemplate(templateKey);

        var rendered = PlaceholderRegex().Replace(template, match =>
        {
            var key = match.Groups["key"].Value;
            if (!values.TryGetValue(key, out var value))
            {
                throw new InvalidOperationException($"Missing value for placeholder '{key}' in template '{templateKey}'.");
            }

            return Convert.ToString(value, CultureInfo.InvariantCulture) ?? string.Empty;
        });

        return rendered;
    }
}
