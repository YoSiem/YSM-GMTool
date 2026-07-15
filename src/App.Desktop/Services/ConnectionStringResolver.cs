using System.Collections.Generic;
using App.Core.Abstractions;
using App.Core.Enums;

namespace App.Desktop.Services;

/// <summary>
/// Resolves the effective connection string + query tokens from the live settings into a
/// shared injectable used by every data-backed tab.
/// </summary>
public sealed class ConnectionStringResolver(IConnectionStringBuilderService builder, IAppSettingsHolder settings)
{
    public DatabaseProvider Provider => settings.Current.Provider;

    /// <summary>
    /// Builds the connection string from the structured connection settings when a server+database are
    /// present (caching it back into <c>ConnectionString</c>); otherwise falls back to the stored
    /// <c>ConnectionString</c>. Throws when neither is configured.
    /// </summary>
    public string Resolve()
    {
        var current = settings.Current;

        if (!string.IsNullOrWhiteSpace(current.Connection.Server)
            && !string.IsNullOrWhiteSpace(current.Connection.Database))
        {
            var built = builder.Build(current.Provider, current.Connection);
            current.ConnectionString = built;
            return built;
        }

        if (string.IsNullOrWhiteSpace(current.ConnectionString))
        {
            throw new InvalidOperationException(
                "Connection string is empty. Open Settings and configure the database provider/connection.");
        }

        return current.ConnectionString;
    }

    public IReadOnlyDictionary<string, string> Tokens() => settings.Current.TableNames.ToTokenMap();
}
