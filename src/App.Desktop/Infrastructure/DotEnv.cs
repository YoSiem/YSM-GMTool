using Serilog;

namespace App.Desktop.Infrastructure;

/// <summary>
/// Discovers and loads a <c>.env</c> file into process environment variables:
/// search roots (base dir, current dir, app dir, plus parent walk), KEY=VALUE parse,
/// quote-stripping, and <see cref="Environment.SetEnvironmentVariable(string, string?)"/>.
/// </summary>
public static class DotEnv
{
    public const string DbProviderEnvKey = "YSM_DB_PROVIDER";
    public const string DbConnectionStringEnvKey = "YSM_DB_CONNECTION_STRING";

    public static void LoadIfPresent(string appDirectory)
    {
        var envPath = FindDotEnvPath(appDirectory);
        if (string.IsNullOrWhiteSpace(envPath) || !File.Exists(envPath))
        {
            return;
        }

        foreach (var rawLine in File.ReadLines(envPath))
        {
            var line = rawLine.Trim();
            if (line.Length == 0 || line.StartsWith('#'))
            {
                continue;
            }

            var separatorIndex = line.IndexOf('=');
            if (separatorIndex <= 0)
            {
                continue;
            }

            var key = line[..separatorIndex].Trim();
            var value = line[(separatorIndex + 1)..].Trim();
            if (value.StartsWith('\"') && value.EndsWith('\"') && value.Length >= 2)
            {
                value = value[1..^1];
            }

            if (string.IsNullOrWhiteSpace(key))
            {
                continue;
            }

            Environment.SetEnvironmentVariable(key, value);
        }

        Log.Information(
            ".env loaded. {ProviderKey} set: {ProviderSet}, {ConnectionKey} set: {ConnectionSet}",
            DbProviderEnvKey,
            !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(DbProviderEnvKey)),
            DbConnectionStringEnvKey,
            !string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable(DbConnectionStringEnvKey)));
    }

    private static string? FindDotEnvPath(string appDirectory)
    {
        var directCandidates = new[]
        {
            Path.Combine(AppContext.BaseDirectory, ".env"),
            Path.Combine(Directory.GetCurrentDirectory(), ".env"),
            Path.Combine(appDirectory, ".env")
        };

        foreach (var candidate in directCandidates)
        {
            if (File.Exists(candidate))
            {
                return candidate;
            }
        }

        var searchRoots = new[]
        {
            Directory.GetCurrentDirectory(),
            AppContext.BaseDirectory,
            appDirectory
        };

        foreach (var root in searchRoots.Distinct(StringComparer.OrdinalIgnoreCase))
        {
            var directory = new DirectoryInfo(root);
            while (directory is not null)
            {
                var candidate = Path.Combine(directory.FullName, ".env");
                if (File.Exists(candidate))
                {
                    return candidate;
                }

                directory = directory.Parent;
            }
        }

        return null;
    }
}
