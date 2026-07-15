namespace App.Core.Commands;

/// <summary>
/// Normalizes a user-entered hex color into the form used by the game's rich-text color tag
/// <c>&lt;#rrggbb[aa]&gt;</c>. Accepts an optional leading <c>#</c>; 3-digit shorthand expands to
/// 6; 6 (RGB) and 8 (RGBA, with alpha) digits pass through lower-cased; anything else is rejected
/// (returns null, so the caller falls back to the client default color).
/// </summary>
public static class HexColor
{
    public static string? Normalize(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        var hex = new string(value.Trim().TrimStart('#').Where(Uri.IsHexDigit).ToArray());

        return hex.Length switch
        {
            3 => string.Concat(hex.Select(c => $"{c}{c}")).ToLowerInvariant(),
            6 or 8 => hex.ToLowerInvariant(),
            _ => null,
        };
    }
}
