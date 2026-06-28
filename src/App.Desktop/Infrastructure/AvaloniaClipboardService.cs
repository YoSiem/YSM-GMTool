using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;

namespace App.Desktop.Infrastructure;

public sealed class AvaloniaClipboardService : IClipboardService
{
    public async Task SetTextAsync(string text)
    {
        var top = (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow;
        if (top?.Clipboard is { } cb)
        {
            await cb.SetTextAsync(text);
        }
    }

    public async Task<string?> GetTextAsync()
    {
        var top = (Application.Current?.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.MainWindow;
        return top?.Clipboard is { } cb ? await cb.GetTextAsync() : null;
    }
}
