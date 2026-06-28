using App.Core.Abstractions;
using App.Core.Enums;
using App.Core.Models;

namespace App.Core.Services;

/// <summary>
/// Applies DB connection values from the environment (<c>.env</c> / <c>YSM_DB_*</c>) as a FIRST-RUN
/// SEED only. When the loaded settings already have a connection configured (server+database, or a
/// legacy raw connection string), that is authoritative and the environment is ignored — otherwise a
/// stale or relocated <c>.env</c> would revert the user's saved connection on every startup.
/// </summary>
public static class AppSettingsEnvironmentSeeder
{
    /// <summary>True when the settings already carry a usable connection (so env must not override).</summary>
    public static bool HasConfiguredConnection(AppSettings settings)
        => (!string.IsNullOrWhiteSpace(settings.Connection?.Server)
                && !string.IsNullOrWhiteSpace(settings.Connection?.Database))
            || !string.IsNullOrWhiteSpace(settings.ConnectionString);

    /// <summary>
    /// Seeds <paramref name="settings"/> from the environment-provided provider/connection string,
    /// but only when no connection is already configured.
    /// </summary>
    public static void SeedFromEnvironment(
        AppSettings settings,
        string? envProvider,
        string? envConnectionString,
        IConnectionStringBuilderService connectionStringBuilder)
    {
        if (HasConfiguredConnection(settings))
        {
            return;
        }

        if (!string.IsNullOrWhiteSpace(envProvider)
            && Enum.TryParse<DatabaseProvider>(envProvider, ignoreCase: true, out var provider))
        {
            settings.Provider = provider;
        }

        if (!string.IsNullOrWhiteSpace(envConnectionString))
        {
            settings.ConnectionString = envConnectionString.Trim();
            if (connectionStringBuilder.TryParse(settings.Provider, settings.ConnectionString, out var parsed))
            {
                settings.Connection = parsed;
            }
        }
    }
}
