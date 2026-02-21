namespace App.WinForms.Forms;

public partial class AboutForm : Form
{
    public AboutForm()
    {
        InitializeComponent();
        ApplyDialogIcon();
    }

    private void ApplyDialogIcon()
    {
        try
        {
            using var exeIcon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            if (exeIcon is not null)
            {
                Icon = (Icon)exeIcon.Clone();
                picIcon.Image = exeIcon.ToBitmap();
                ShowIcon = true;
            }
        }
        catch
        {
            // Keep default if extraction fails.
        }
    }
}
