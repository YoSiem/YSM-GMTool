namespace App.WinForms.Models;

public sealed record BrowserColumnDefinition(
    string Name,
    string HeaderText,
    int Width = 120,
    bool Fill = false,
    bool IsImage = false,
    int ImageSize = 16);
