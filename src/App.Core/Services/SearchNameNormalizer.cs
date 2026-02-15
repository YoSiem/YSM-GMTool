using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using App.Core.Interfaces;

namespace App.Core.Services;

public sealed partial class SearchNameNormalizer : INameNormalizer
{
    [GeneratedRegex("<[^>]*>", RegexOptions.Compiled)]
    private static partial Regex TagsRegex();

    [GeneratedRegex("\\s+", RegexOptions.Compiled)]
    private static partial Regex WhitespaceRegex();

    public string NormalizeForSearch(string? value, bool removeDiacritics = true)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        var withoutTags = TagsRegex().Replace(value, " ");
        var collapsedWhitespace = WhitespaceRegex().Replace(withoutTags, " ").Trim();

        if (!removeDiacritics)
        {
            return collapsedWhitespace.ToLowerInvariant();
        }

        var normalized = collapsedWhitespace.Normalize(NormalizationForm.FormD);
        var builder = new StringBuilder(normalized.Length);

        foreach (var c in normalized)
        {
            var category = CharUnicodeInfo.GetUnicodeCategory(c);
            if (category is not UnicodeCategory.NonSpacingMark)
            {
                builder.Append(c);
            }
        }

        return builder
            .ToString()
            .Normalize(NormalizationForm.FormC)
            .ToLowerInvariant();
    }
}
