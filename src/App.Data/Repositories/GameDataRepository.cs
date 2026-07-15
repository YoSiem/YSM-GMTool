using App.Core.Enums;
using App.Core.Abstractions;
using App.Core.Models.Entities;
using App.Data.Infrastructure;
using Dapper;

namespace App.Data.Repositories;

public sealed class GameDataRepository(IQueryStore queryStore, DbConnectionFactory connectionFactory) : IGameDataRepository
{
    private readonly IQueryStore _queryStore = queryStore;
    private readonly DbConnectionFactory _connectionFactory = connectionFactory;

    public Task<IReadOnlyList<PlayerRecord>> GetPlayersAsync(
        DatabaseProvider provider,
        string connectionString,
        IReadOnlyDictionary<string, string>? queryTokens = null,
        CancellationToken cancellationToken = default)
        => QueryAsync<PlayerRecord>(provider, connectionString, QueryEntity.Playerchecker, queryTokens, null, cancellationToken);

    public Task<IReadOnlyList<PlayerRecord>> GetCharactersBySearchAsync(
        DatabaseProvider provider,
        string connectionString,
        string searchTerm,
        bool searchByAccount,
        IReadOnlyDictionary<string, string>? queryTokens = null,
        CancellationToken cancellationToken = default)
    {
        var entity = searchByAccount ? QueryEntity.PlayercheckerByAccount : QueryEntity.PlayercheckerByCharName;
        var parameters = new { SearchTerm = "%" + searchTerm.Trim() + "%" };
        return QueryAsync<PlayerRecord>(provider, connectionString, entity, queryTokens, parameters, cancellationToken);
    }

    public Task<IReadOnlyList<PlayerRecord>> GetAllCharactersAsync(
        DatabaseProvider provider,
        string connectionString,
        IReadOnlyDictionary<string, string>? queryTokens = null,
        CancellationToken cancellationToken = default)
        => QueryAsync<PlayerRecord>(provider, connectionString, QueryEntity.PlayercheckerAll, queryTokens, null, cancellationToken);

    public Task<IReadOnlyList<PlayerRecord>> GetOnlineCharactersAsync(
        DatabaseProvider provider,
        string connectionString,
        IReadOnlyDictionary<string, string>? queryTokens = null,
        CancellationToken cancellationToken = default)
        => QueryAsync<PlayerRecord>(provider, connectionString, QueryEntity.PlayercheckerOnline, queryTokens, null, cancellationToken);

    public Task<IReadOnlyList<InventoryItemRecord>> GetInventoryAsync(
        DatabaseProvider provider,
        string connectionString,
        int characterId,
        IReadOnlyDictionary<string, string>? queryTokens = null,
        CancellationToken cancellationToken = default)
        => QueryAsync<InventoryItemRecord>(provider, connectionString, QueryEntity.PlayerInventory, queryTokens,
            new { OwnerId = characterId }, cancellationToken);

    public Task<IReadOnlyList<InventoryItemRecord>> GetWarehouseAsync(
        DatabaseProvider provider,
        string connectionString,
        string accountName,
        IReadOnlyDictionary<string, string>? queryTokens = null,
        CancellationToken cancellationToken = default)
        => QueryAsync<InventoryItemRecord>(provider, connectionString, QueryEntity.PlayerWarehouse, queryTokens,
            new { AccountName = accountName }, cancellationToken);

    public Task<IReadOnlyList<MonsterRecord>> GetMonstersAsync(
        DatabaseProvider provider,
        string connectionString,
        IReadOnlyDictionary<string, string>? queryTokens = null,
        CancellationToken cancellationToken = default)
        => QueryAsync<MonsterRecord>(provider, connectionString, QueryEntity.Monsters, queryTokens, null, cancellationToken);

    public Task<IReadOnlyList<ItemRecord>> GetItemsAsync(
        DatabaseProvider provider,
        string connectionString,
        IReadOnlyDictionary<string, string>? queryTokens = null,
        CancellationToken cancellationToken = default)
        => QueryAsync<ItemRecord>(provider, connectionString, QueryEntity.Items, queryTokens, null, cancellationToken);

    public Task<IReadOnlyList<SkillRecord>> GetSkillsAsync(
        DatabaseProvider provider,
        string connectionString,
        IReadOnlyDictionary<string, string>? queryTokens = null,
        CancellationToken cancellationToken = default)
        => QueryAsync<SkillRecord>(provider, connectionString, QueryEntity.Skills, queryTokens, null, cancellationToken);

    public Task<IReadOnlyList<StateRecord>> GetStatesAsync(
        DatabaseProvider provider,
        string connectionString,
        IReadOnlyDictionary<string, string>? queryTokens = null,
        CancellationToken cancellationToken = default)
        => QueryAsync<StateRecord>(provider, connectionString, QueryEntity.States, queryTokens, null, cancellationToken);

    public Task<IReadOnlyList<NpcRecord>> GetNpcsAsync(
        DatabaseProvider provider,
        string connectionString,
        IReadOnlyDictionary<string, string>? queryTokens = null,
        CancellationToken cancellationToken = default)
        => QueryAsync<NpcRecord>(provider, connectionString, QueryEntity.Npc, queryTokens, null, cancellationToken);

    public Task<IReadOnlyList<SummonRecord>> GetSummonsAsync(
        DatabaseProvider provider,
        string connectionString,
        IReadOnlyDictionary<string, string>? queryTokens = null,
        CancellationToken cancellationToken = default)
        => QueryAsync<SummonRecord>(provider, connectionString, QueryEntity.Summons, queryTokens, null, cancellationToken);

    public Task<IReadOnlyList<ItemEffectRecord>> GetItemEffectsAsync(
        DatabaseProvider provider,
        string connectionString,
        IReadOnlyDictionary<string, string>? queryTokens = null,
        CancellationToken cancellationToken = default)
        => QueryAsync<ItemEffectRecord>(provider, connectionString, QueryEntity.ItemEffects, queryTokens, null, cancellationToken);

    public async Task TestConnectionAsync(DatabaseProvider provider, string connectionString, CancellationToken cancellationToken = default)
    {
        await using var connection = _connectionFactory.Create(provider, connectionString);
        await connection.OpenAsync(cancellationToken);
        await connection.CloseAsync();
    }

    private async Task<IReadOnlyList<T>> QueryAsync<T>(
        DatabaseProvider provider,
        string connectionString,
        QueryEntity entity,
        IReadOnlyDictionary<string, string>? queryTokens,
        object? parameters,
        CancellationToken cancellationToken)
    {
        var query = ApplyQueryTokens(_queryStore.GetQuery(provider, entity), queryTokens);

        await using var connection = _connectionFactory.Create(provider, connectionString);
        await connection.OpenAsync(cancellationToken);

        var command = new CommandDefinition(query, parameters, cancellationToken: cancellationToken);
        var rows = await connection.QueryAsync<T>(command);
        return rows.AsList();
    }

    private static string ApplyQueryTokens(string query, IReadOnlyDictionary<string, string>? queryTokens)
    {
        if (queryTokens is null || queryTokens.Count == 0)
        {
            return query;
        }

        var output = query;
        foreach (var (key, value) in queryTokens)
        {
            if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(value))
            {
                continue;
            }

            output = output.Replace($"{{{{{key}}}}}", value.Trim(), StringComparison.Ordinal);
        }

        return output;
    }
}
