using App.Core.Enums;
using App.Core.Models.Entities;

namespace App.Core.Abstractions;

public interface IGameDataRepository
{
    Task<IReadOnlyList<PlayerRecord>> GetPlayersAsync(
        DatabaseProvider provider,
        string connectionString,
        IReadOnlyDictionary<string, string>? queryTokens = null,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<PlayerRecord>> GetCharactersBySearchAsync(
        DatabaseProvider provider,
        string connectionString,
        string searchTerm,
        bool searchByAccount,
        IReadOnlyDictionary<string, string>? queryTokens = null,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<PlayerRecord>> GetAllCharactersAsync(
        DatabaseProvider provider,
        string connectionString,
        IReadOnlyDictionary<string, string>? queryTokens = null,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<PlayerRecord>> GetOnlineCharactersAsync(
        DatabaseProvider provider,
        string connectionString,
        IReadOnlyDictionary<string, string>? queryTokens = null,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<InventoryItemRecord>> GetInventoryAsync(
        DatabaseProvider provider,
        string connectionString,
        int characterId,
        IReadOnlyDictionary<string, string>? queryTokens = null,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<InventoryItemRecord>> GetWarehouseAsync(
        DatabaseProvider provider,
        string connectionString,
        string accountName,
        IReadOnlyDictionary<string, string>? queryTokens = null,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<MonsterRecord>> GetMonstersAsync(
        DatabaseProvider provider,
        string connectionString,
        IReadOnlyDictionary<string, string>? queryTokens = null,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ItemRecord>> GetItemsAsync(
        DatabaseProvider provider,
        string connectionString,
        IReadOnlyDictionary<string, string>? queryTokens = null,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SkillRecord>> GetSkillsAsync(
        DatabaseProvider provider,
        string connectionString,
        IReadOnlyDictionary<string, string>? queryTokens = null,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<StateRecord>> GetStatesAsync(
        DatabaseProvider provider,
        string connectionString,
        IReadOnlyDictionary<string, string>? queryTokens = null,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<NpcRecord>> GetNpcsAsync(
        DatabaseProvider provider,
        string connectionString,
        IReadOnlyDictionary<string, string>? queryTokens = null,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<SummonRecord>> GetSummonsAsync(
        DatabaseProvider provider,
        string connectionString,
        IReadOnlyDictionary<string, string>? queryTokens = null,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<ItemEffectRecord>> GetItemEffectsAsync(
        DatabaseProvider provider,
        string connectionString,
        IReadOnlyDictionary<string, string>? queryTokens = null,
        CancellationToken cancellationToken = default);

    Task TestConnectionAsync(DatabaseProvider provider, string connectionString, CancellationToken cancellationToken = default);
}
