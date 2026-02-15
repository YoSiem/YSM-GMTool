namespace App.WinForms.Models;

public sealed class BrowserRow(object tag, params object?[] values)
{
    public object Tag { get; } = tag;

    public object?[] Values { get; } = values;
}
