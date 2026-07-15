using App.Core.Abstractions;
using App.Core.Enums;
using App.Core.Interfaces;
using App.Data.Infrastructure;
using App.Data.Repositories;
using App.Data.Services;
using Microsoft.Data.Sqlite;

namespace App.Data.Tests;

public class SnapshotExportServiceTests
{
    [Fact]
    public async Task Export_CopiesAllEntitiesAndWritesMeta()
    {
        var sourcePath = Path.GetTempFileName();
        var targetPath = Path.GetTempFileName();
        File.Delete(sourcePath);
        File.Delete(targetPath);

        try
        {
            SqliteSnapshotInitializer.Initialize(sourcePath);
            using (var seed = new SqliteConnection($"Data Source={sourcePath}"))
            {
                seed.Open();
                using var cmd = seed.CreateCommand();
                cmd.CommandText = """
                    INSERT INTO Items VALUES (1,'Sword','sword.png');
                    INSERT INTO Monsters VALUES (10,'Goblin',5,'Forest');
                    INSERT INTO Npcs VALUES (20,'Blacksmith',1.0,2.0,'shop.lua');
                    INSERT INTO Skills VALUES (30,'Slash','slash.png');
                    INSERT INTO States VALUES (40,'Poison','poison.png');
                    INSERT INTO Summons VALUES (50,'Wolf','Wolf Card','wolf.png');
                    """;
                cmd.ExecuteNonQuery();
            }

            var queryStore = new TestQueryStore();
            var factory = new DbConnectionFactory();
            var repo = new GameDataRepository(queryStore, factory);
            var service = new SnapshotExportService(repo);

            await service.ExportAsync(
                DatabaseProvider.Sqlite,
                $"Data Source={sourcePath}",
                new Dictionary<string, string>(),
                targetPath,
                progress: null,
                CancellationToken.None);

            using var conn = new SqliteConnection($"Data Source={targetPath}");
            conn.Open();
            Assert.Equal(1L, ScalarLong(conn, "SELECT COUNT(*) FROM Items"));
            Assert.Equal(1L, ScalarLong(conn, "SELECT COUNT(*) FROM Monsters"));
            Assert.Equal(1L, ScalarLong(conn, "SELECT COUNT(*) FROM Npcs"));
            Assert.Equal(1L, ScalarLong(conn, "SELECT COUNT(*) FROM Skills"));
            Assert.Equal(1L, ScalarLong(conn, "SELECT COUNT(*) FROM States"));
            Assert.Equal(1L, ScalarLong(conn, "SELECT COUNT(*) FROM Summons"));
            Assert.Equal("1", ScalarString(conn, "SELECT value FROM Meta WHERE key='schema_version'"));
            Assert.Equal("Sqlite", ScalarString(conn, "SELECT value FROM Meta WHERE key='source_provider'"));
        }
        finally
        {
            SqliteConnection.ClearAllPools();
            if (File.Exists(sourcePath)) File.Delete(sourcePath);
            if (File.Exists(targetPath)) File.Delete(targetPath);
        }
    }

    private static long ScalarLong(SqliteConnection c, string sql)
    {
        using var cmd = c.CreateCommand(); cmd.CommandText = sql;
        return (long)cmd.ExecuteScalar()!;
    }

    private static string ScalarString(SqliteConnection c, string sql)
    {
        using var cmd = c.CreateCommand(); cmd.CommandText = sql;
        return (string)cmd.ExecuteScalar()!;
    }
}

internal sealed class TestQueryStore : IQueryStore
{
    public string GetQuery(DatabaseProvider provider, QueryEntity entity)
        => entity switch
        {
            QueryEntity.Items    => "SELECT item_id, name_en, icon_file_name FROM Items",
            QueryEntity.Monsters => "SELECT id, name, level, location FROM Monsters",
            QueryEntity.Npc      => "SELECT npc_id, npc_title, x, y, contact_script FROM Npcs",
            QueryEntity.Skills   => "SELECT skill_id, skillname, icon_file_name FROM Skills",
            QueryEntity.States   => "SELECT state_id, buff_name, icon_file_name FROM States",
            QueryEntity.Summons  => "SELECT summon_id, summon_name, card_name, icon_file_name FROM Summons",
            _ => throw new NotSupportedException(entity.ToString())
        };
}
