namespace App.Desktop.Features.Notice;

/// <summary>Which broadcast function the Notice builder emits.</summary>
public enum NoticeKind
{
    /// <summary><c>notice</c> — yellow, centered.</summary>
    Center,

    /// <summary><c>notice_left</c> — yellow, left side.</summary>
    Left,

    /// <summary><c>notice_right</c> — yellow, right side.</summary>
    Right,

    /// <summary><c>announce</c> — green, centered.</summary>
    Announce,
}
