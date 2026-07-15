using static System.FormattableString;

namespace App.Core.Commands;

/// <summary>
/// Assembles the payload that goes inside <c>notice("...")</c> / <c>announce("...")</c> from the
/// builder's controls: multi-line text joined with <c>&lt;BR&gt;</c>, an optional bold wrap, and
/// optional color/size tag prefixes. The client auto-applies its own default style (yellow for
/// notice, green for announce, size 13, centered); a tag emitted here overrides that default.
/// </summary>
public static class NoticeTextBuilder
{
    /// <param name="rawText">User text; line breaks become <c>&lt;BR&gt;</c>.</param>
    /// <param name="bold">Wrap the whole body in <c>&lt;B&gt;…&lt;/B&gt;</c>.</param>
    /// <param name="colorHex">Normalized hex digits (no <c>#</c>; 6 = RGB or 8 = RGBA), or null for
    /// the client default color. Normalize raw input with <see cref="HexColor.Normalize"/> first.</param>
    /// <param name="size">Font size for a <c>&lt;size:n&gt;</c> tag, or null for the client default (13).</param>
    public static string Build(string rawText, bool bold, string? colorHex, int? size)
    {
        var normalized = (rawText ?? string.Empty)
            .Replace("\r\n", "\n", StringComparison.Ordinal)
            .Replace("\r", "\n", StringComparison.Ordinal);

        var body = string.Join("<BR>", normalized.Split('\n'));

        if (bold)
        {
            body = "<B>" + body + "</B>";
        }

        var prefix = string.Empty;
        if (!string.IsNullOrEmpty(colorHex))
        {
            prefix += "<#" + colorHex + ">";
        }

        if (size is int s)
        {
            prefix += Invariant($"<size:{s}>");
        }

        return prefix + body;
    }
}
