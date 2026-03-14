using App.Core.Enums;

namespace App.Core.Models;

public sealed class AppSettings
{
    public DatabaseProvider Provider { get; set; } = DatabaseProvider.MSSQL;

    // Legacy fallback for old settings.json versions.
    public string ConnectionString { get; set; } = string.Empty;

    public DatabaseConnectionSettings Connection { get; set; } = new();

    public TableNameSettings TableNames { get; set; } = new();

    public List<string> Players { get; set; } = [];

    public string? SelectedPlayer { get; set; }

    public bool AppendGeneratedCommands { get; set; } = true;

    public bool LimitSelectQueries { get; set; } = true;

    public bool UseLocalCache { get; set; } = false;

    public bool EnableEntityIcons { get; set; } = false;

    public string EntityIconsPath { get; set; } = string.Empty;

    public List<WarpLocationSettings> WarpLocations { get; set; } = [];

    public AppSettings Clone() => new()
    {
        Provider = Provider,
        ConnectionString = ConnectionString,
        Connection = (Connection ?? new DatabaseConnectionSettings()).Clone(),
        TableNames = (TableNames ?? new TableNameSettings()).Clone(),
        Players = Players is null ? [] : [.. Players],
        SelectedPlayer = SelectedPlayer,
        AppendGeneratedCommands = AppendGeneratedCommands,
        LimitSelectQueries = LimitSelectQueries,
        UseLocalCache = UseLocalCache,
        EnableEntityIcons = EnableEntityIcons,
        EntityIconsPath = EntityIconsPath,
        WarpLocations = WarpLocations is null ? [] : [.. WarpLocations.Select(static x => x.Clone())]
    };
}
