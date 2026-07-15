using System.Globalization;
using App.Core.Abstractions;
using App.Core.Enums;
using App.Core.Interfaces;
using App.Data.Infrastructure;
using Microsoft.Data.Sqlite;

namespace App.Data.Services;

public sealed class SnapshotExportService : ISnapshotExportService
{
    private readonly IGameDataRepository _repo;

    public SnapshotExportService(IGameDataRepository repo) => _repo = repo;

    public async Task<SnapshotExportResult> ExportAsync(
        DatabaseProvider sourceProvider,
        string sourceConnectionString,
        IReadOnlyDictionary<string, string> queryTokens,
        string targetSnapshotPath,
        IProgress<SnapshotExportProgress>? progress,
        CancellationToken cancellationToken)
    {
        if (File.Exists(targetSnapshotPath)) File.Delete(targetSnapshotPath);
        SqliteSnapshotInitializer.Initialize(targetSnapshotPath);

        await using var target = new SqliteConnection($"Data Source={targetSnapshotPath}");
        await target.OpenAsync(cancellationToken);
        await using var tx = (SqliteTransaction)await target.BeginTransactionAsync(cancellationToken);

        var counts = new List<(string EntityName, int RowCount)>();

        var items    = await _repo.GetItemsAsync(sourceProvider, sourceConnectionString, queryTokens, cancellationToken);
        ReportAndInsert(progress, "Items", items, target, tx, "INSERT INTO Items VALUES ($a,$b,$c)",
            (cmd, r) => { cmd.Parameters.AddWithValue("$a", r.ItemId); cmd.Parameters.AddWithValue("$b", r.NameEn); cmd.Parameters.AddWithValue("$c", (object?)r.IconFileName ?? DBNull.Value); });
        counts.Add(("Items", items.Count));

        var monsters = await _repo.GetMonstersAsync(sourceProvider, sourceConnectionString, queryTokens, cancellationToken);
        ReportAndInsert(progress, "Monsters", monsters, target, tx, "INSERT INTO Monsters VALUES ($a,$b,$c,$d)",
            (cmd, r) => { cmd.Parameters.AddWithValue("$a", r.Id); cmd.Parameters.AddWithValue("$b", r.Name); cmd.Parameters.AddWithValue("$c", (object?)r.Level ?? DBNull.Value); cmd.Parameters.AddWithValue("$d", (object?)r.Location ?? DBNull.Value); });
        counts.Add(("Monsters", monsters.Count));

        var npcs     = await _repo.GetNpcsAsync(sourceProvider, sourceConnectionString, queryTokens, cancellationToken);
        ReportAndInsert(progress, "Npcs", npcs, target, tx, "INSERT INTO Npcs VALUES ($a,$b,$c,$d,$e)",
            (cmd, r) => { cmd.Parameters.AddWithValue("$a", r.NpcId); cmd.Parameters.AddWithValue("$b", r.NpcTitle); cmd.Parameters.AddWithValue("$c", (object?)r.X ?? DBNull.Value); cmd.Parameters.AddWithValue("$d", (object?)r.Y ?? DBNull.Value); cmd.Parameters.AddWithValue("$e", (object?)r.ContactScript ?? DBNull.Value); });
        counts.Add(("NPCs", npcs.Count));

        var skills   = await _repo.GetSkillsAsync(sourceProvider, sourceConnectionString, queryTokens, cancellationToken);
        ReportAndInsert(progress, "Skills", skills, target, tx, "INSERT INTO Skills VALUES ($a,$b,$c)",
            (cmd, r) => { cmd.Parameters.AddWithValue("$a", r.SkillId); cmd.Parameters.AddWithValue("$b", r.Skillname); cmd.Parameters.AddWithValue("$c", (object?)r.IconFileName ?? DBNull.Value); });
        counts.Add(("Skills", skills.Count));

        var states   = await _repo.GetStatesAsync(sourceProvider, sourceConnectionString, queryTokens, cancellationToken);
        ReportAndInsert(progress, "States", states, target, tx, "INSERT INTO States VALUES ($a,$b,$c)",
            (cmd, r) => { cmd.Parameters.AddWithValue("$a", r.StateId); cmd.Parameters.AddWithValue("$b", r.BuffName); cmd.Parameters.AddWithValue("$c", (object?)r.IconFileName ?? DBNull.Value); });
        counts.Add(("Buffs/States", states.Count));

        var summons  = await _repo.GetSummonsAsync(sourceProvider, sourceConnectionString, queryTokens, cancellationToken);
        ReportAndInsert(progress, "Summons", summons, target, tx, "INSERT INTO Summons VALUES ($a,$b,$c,$d)",
            (cmd, r) => { cmd.Parameters.AddWithValue("$a", r.SummonId); cmd.Parameters.AddWithValue("$b", r.SummonName); cmd.Parameters.AddWithValue("$c", (object?)r.CardName ?? DBNull.Value); cmd.Parameters.AddWithValue("$d", (object?)r.IconFileName ?? DBNull.Value); });
        counts.Add(("Summons", summons.Count));

        WriteMeta(target, tx, "schema_version", SqliteSnapshotInitializer.SchemaVersion.ToString(CultureInfo.InvariantCulture));
        WriteMeta(target, tx, "exported_at",    DateTime.UtcNow.ToString("O", CultureInfo.InvariantCulture));
        WriteMeta(target, tx, "source_provider", sourceProvider.ToString());

        await tx.CommitAsync(cancellationToken);

        return new SnapshotExportResult(counts);
    }

    private static void ReportAndInsert<T>(
        IProgress<SnapshotExportProgress>? progress,
        string entityName,
        IReadOnlyList<T> rows,
        SqliteConnection conn,
        SqliteTransaction tx,
        string sql,
        Action<SqliteCommand, T> bind)
    {
        var total = rows.Count;
        for (var i = 0; i < total; i++)
        {
            using var cmd = conn.CreateCommand();
            cmd.Transaction = tx;
            cmd.CommandText = sql;
            bind(cmd, rows[i]);
            cmd.ExecuteNonQuery();
            if ((i & 0x1F) == 0) progress?.Report(new SnapshotExportProgress(entityName, i + 1, total));
        }
        progress?.Report(new SnapshotExportProgress(entityName, total, total));
    }

    private static void WriteMeta(SqliteConnection c, SqliteTransaction tx, string key, string value)
    {
        using var cmd = c.CreateCommand();
        cmd.Transaction = tx;
        cmd.CommandText = "INSERT OR REPLACE INTO Meta(key,value) VALUES ($k,$v)";
        cmd.Parameters.AddWithValue("$k", key);
        cmd.Parameters.AddWithValue("$v", value);
        cmd.ExecuteNonQuery();
    }
}
