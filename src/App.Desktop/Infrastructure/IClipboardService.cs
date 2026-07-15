namespace App.Desktop.Infrastructure;

public interface IClipboardService
{
    Task SetTextAsync(string text);

    Task<string?> GetTextAsync();
}
