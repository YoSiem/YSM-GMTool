using App.Core.Enums;
using App.Core.Interfaces;
using App.Core.Models.Entities;
using App.Data.Infrastructure;
using Dapper;

namespace App.Data.Repositories;

public sealed class GameDataRepository(IQueryStore queryStore, DbConnectionFactory connectionFactory) : IGameDataRepository
{
    private readonly IQueryStore _queryStore = queryStore;
    private readonly DbConnectionFactory _connectionFactory = connectionFactory;

    static GameDataRepository()
    {
        DefaultTypeMap.MatchNamesWithUnderscores = true;
    }

    public Task<IReadOnlyList<PlayerRecord>> GetPlayersAsync(DatabaseProvider provider, string connectionString, CancellationToken cancellationToken = default)
        => QueryAsync<PlayerRecord>(provider, connectionString, QueryEntity.Playerchecker, cancellationToken);

    public Task<IReadOnlyList<MonsterRecord>> GetMonstersAsync(DatabaseProvider provider, string connectionString, CancellationToken cancellationToken = default)
        => QueryAsync<MonsterRecord>(provider, connectionString, QueryEntity.Monsters, cancellationToken);

    public Task<IReadOnlyList<ItemRecord>> GetItemsAsync(DatabaseProvider provider, string connectionString, CancellationToken cancellationToken = default)
        => QueryAsync<ItemRecord>(provider, connectionString, QueryEntity.Items, cancellationToken);

    public Task<IReadOnlyList<SkillRecord>> GetSkillsAsync(DatabaseProvider provider, string connectionString, CancellationToken cancellationToken = default)
        => QueryAsync<SkillRecord>(provider, connectionString, QueryEntity.Skills, cancellationToken);

    public Task<IReadOnlyList<StateRecord>> GetStatesAsync(DatabaseProvider provider, string connectionString, CancellationToken cancellationToken = default)
        => QueryAsync<StateRecord>(provider, connectionString, QueryEntity.States, cancellationToken);

    public Task<IReadOnlyList<NpcRecord>> GetNpcsAsync(DatabaseProvider provider, string connectionString, CancellationToken cancellationToken = default)
        => QueryAsync<NpcRecord>(provider, connectionString, QueryEntity.Npc, cancellationToken);

    public Task<IReadOnlyList<SummonRecord>> GetSummonsAsync(DatabaseProvider provider, string connectionString, CancellationToken cancellationToken = default)
        => QueryAsync<SummonRecord>(provider, connectionString, QueryEntity.Summons, cancellationToken);

    public async Task TestConnectionAsync(DatabaseProvider provider, string connectionString, CancellationToken cancellationToken = default)
    {
        await using var connection = _connectionFactory.Create(provider, connectionString);
        await connection.OpenAsync(cancellationToken);
        await connection.CloseAsync();
    }

    private async Task<IReadOnlyList<T>> QueryAsync<T>(DatabaseProvider provider, string connectionString, QueryEntity entity, CancellationToken cancellationToken)
    {
        var query = _queryStore.GetQuery(provider, entity);

        await using var connection = _connectionFactory.Create(provider, connectionString);
        await connection.OpenAsync(cancellationToken);

        var command = new CommandDefinition(query, cancellationToken: cancellationToken);
        var rows = await connection.QueryAsync<T>(command);
        return rows.AsList();
    }
}
