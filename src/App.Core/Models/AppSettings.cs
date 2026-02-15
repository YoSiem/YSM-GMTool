using App.Core.Enums;

namespace App.Core.Models;

public sealed class AppSettings
{
    public DatabaseProvider Provider { get; set; } = DatabaseProvider.MSSQL;

    public string ConnectionString { get; set; } = string.Empty;

    public List<string> Players { get; set; } = [];

    public string? SelectedPlayer { get; set; }

    public bool AppendGeneratedCommands { get; set; } = true;
}
