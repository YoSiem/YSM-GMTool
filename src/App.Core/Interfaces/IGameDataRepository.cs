using App.Core.Enums;
using App.Core.Models.Entities;

namespace App.Core.Interfaces;

public interface IGameDataRepository
{
    Task<IReadOnlyList<PlayerRecord>> GetPlayersAsync(DatabaseProvider provider, string connectionString, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<MonsterRecord>> GetMonstersAsync(DatabaseProvider provider, string connectionString, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ItemRecord>> GetItemsAsync(DatabaseProvider provider, string connectionString, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SkillRecord>> GetSkillsAsync(DatabaseProvider provider, string connectionString, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<StateRecord>> GetStatesAsync(DatabaseProvider provider, string connectionString, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<NpcRecord>> GetNpcsAsync(DatabaseProvider provider, string connectionString, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SummonRecord>> GetSummonsAsync(DatabaseProvider provider, string connectionString, CancellationToken cancellationToken = default);

    Task TestConnectionAsync(DatabaseProvider provider, string connectionString, CancellationToken cancellationToken = default);
}
