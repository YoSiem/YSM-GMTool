namespace App.Desktop.Infrastructure;

/// <summary>Centralizes the app's modal message boxes.</summary>
public interface IDialogService
{
    Task ShowInfoAsync(string title, string message);
    Task ShowWarningAsync(string title, string message);
    Task ShowErrorAsync(string title, string message);
    Task<bool> ConfirmAsync(string title, string message);
}
