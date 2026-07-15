using System;
using App.Core.Abstractions;
using App.Core.Models;
using App.Core.Services;
using App.Desktop.Infrastructure;

namespace App.Desktop.Composition;

/// <summary>
/// Seeds an <see cref="AppSettings"/> instance after it is loaded from disk: fills in null
/// collections/objects with defaults and applies the <c>YSM_DB_*</c> environment overrides. Used by
/// <see cref="Program"/> to prepare settings synchronously before the shell and tab view models are
/// constructed (so the icon cache and row-height state are correct at construction time).
/// </summary>
internal static class SettingsBootstrap
{
    public static void EnsureDefaults(AppSettings settings)
    {
        settings.Connection ??= new();
        settings.TableNames ??= new();
        settings.Players ??= [];
        settings.WarpLocations ??= [];
        settings.EntityIconsPath ??= string.Empty;
    }

    /// <summary>
    /// Seeds the DB connection from the <c>YSM_DB_*</c> environment (.env) values — but only as a
    /// first-run fallback. A connection already saved in settings.json is authoritative and is never
    /// overridden here, so a stale/relocated .env can't revert the user's saved connection on startup.
    /// </summary>
    public static void ApplyEnvironmentDefaults(
        AppSettings settings,
        IConnectionStringBuilderService connectionStringBuilder)
        => AppSettingsEnvironmentSeeder.SeedFromEnvironment(
            settings,
            Environment.GetEnvironmentVariable(DotEnv.DbProviderEnvKey),
            Environment.GetEnvironmentVariable(DotEnv.DbConnectionStringEnvKey),
            connectionStringBuilder);
}
