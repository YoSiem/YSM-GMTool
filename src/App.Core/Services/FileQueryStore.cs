using System.Text.Json;
using App.Core.Enums;
using App.Core.Interfaces;

namespace App.Core.Services;

public sealed class FileQueryStore : IQueryStore
{
    private readonly Dictionary<DatabaseProvider, Dictionary<string, string>> _queries;

    public FileQueryStore(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("Query configuration file was not found.", filePath);
        }

        var json = File.ReadAllText(filePath);
        var parsed = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(json,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        if (parsed is null || parsed.Count == 0)
        {
            throw new InvalidOperationException("Query configuration is empty.");
        }

        _queries = new Dictionary<DatabaseProvider, Dictionary<string, string>>();

        foreach (var (providerName, entityQueries) in parsed)
        {
            if (!Enum.TryParse<DatabaseProvider>(providerName, ignoreCase: true, out var provider))
            {
                continue;
            }

            _queries[provider] = new Dictionary<string, string>(entityQueries, StringComparer.OrdinalIgnoreCase);
        }

        if (_queries.Count == 0)
        {
            throw new InvalidOperationException("No supported providers were found in query configuration.");
        }
    }

    public string GetQuery(DatabaseProvider provider, QueryEntity entity)
    {
        if (!_queries.TryGetValue(provider, out var providerQueries))
        {
            throw new KeyNotFoundException($"Missing query provider section: {provider}.");
        }

        var entityKey = ResolveEntityKey(entity);
        if (!providerQueries.TryGetValue(entityKey, out var query) || string.IsNullOrWhiteSpace(query))
        {
            throw new KeyNotFoundException($"Missing query for provider '{provider}' and entity '{entityKey}'.");
        }

        return query;
    }

    private static string ResolveEntityKey(QueryEntity entity) => entity switch
    {
        QueryEntity.Playerchecker => "Playerchecker",
        QueryEntity.Monsters => "Monsters",
        QueryEntity.Items => "Items",
        QueryEntity.Skills => "Skills",
        QueryEntity.States => "States",
        QueryEntity.Npc => "NPC",
        QueryEntity.Summons => "Summons",
        _ => throw new ArgumentOutOfRangeException(nameof(entity), entity, "Unsupported query entity.")
    };
}
