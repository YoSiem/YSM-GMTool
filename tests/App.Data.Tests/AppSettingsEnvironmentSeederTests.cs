using App.Core.Enums;
using App.Core.Models;
using App.Core.Services;
using Xunit;

namespace App.Data.Tests;

public class AppSettingsEnvironmentSeederTests
{
    private static readonly DefaultConnectionStringBuilderService Builder = new();

    [Fact]
    public void SeedFromEnvironment_WhenConnectionConfigured_DoesNotOverride()
    {
        // Regression: a stale/relocated .env must NOT clobber a user-saved connection on startup.
        var settings = new AppSettings
        {
            Provider = DatabaseProvider.MSSQL,
            ConnectionString = "Server=10.243.190.78,55233;Database=Arcadia_Season;User Id=arc;Password=p;",
            Connection = new DatabaseConnectionSettings { Server = "10.243.190.78", Port = 55233, Database = "Arcadia_Season" },
        };

        AppSettingsEnvironmentSeeder.SeedFromEnvironment(
            settings,
            envProvider: "MySQL",
            envConnectionString: "Server=127.0.0.1;Database=HeavenDB55;User Id=h;Password=x;",
            Builder);

        Assert.Equal(DatabaseProvider.MSSQL, settings.Provider);
        Assert.Equal("10.243.190.78", settings.Connection.Server);
        Assert.Equal("Arcadia_Season", settings.Connection.Database);
        Assert.Contains("Arcadia_Season", settings.ConnectionString);
        Assert.DoesNotContain("HeavenDB55", settings.ConnectionString);
    }

    [Fact]
    public void SeedFromEnvironment_WhenOnlyLegacyConnectionStringConfigured_DoesNotOverride()
    {
        // Connection has no Database, but a legacy ConnectionString is present => still "configured".
        var settings = new AppSettings
        {
            ConnectionString = "Server=10.243.190.78;Database=Arcadia_Season;",
            Connection = new DatabaseConnectionSettings { Server = "127.0.0.1", Database = string.Empty },
        };

        AppSettingsEnvironmentSeeder.SeedFromEnvironment(
            settings, "MSSQL", "Server=127.0.0.1;Database=HeavenDB55;", Builder);

        Assert.Equal("Server=10.243.190.78;Database=Arcadia_Season;", settings.ConnectionString);
    }

    [Fact]
    public void SeedFromEnvironment_WhenNoConnection_AppliesEnv()
    {
        var settings = new AppSettings(); // fresh: Connection.Database empty, ConnectionString empty

        AppSettingsEnvironmentSeeder.SeedFromEnvironment(
            settings,
            envProvider: "MySQL",
            envConnectionString: "Server=db.example;Port=3307;Database=Arc;User Id=u;Password=p;",
            Builder);

        Assert.Equal(DatabaseProvider.MySQL, settings.Provider);
        Assert.Equal("Server=db.example;Port=3307;Database=Arc;User Id=u;Password=p;", settings.ConnectionString);
        Assert.Equal("db.example", settings.Connection.Server);
        Assert.Equal("Arc", settings.Connection.Database);
    }

    [Fact]
    public void SeedFromEnvironment_WhenNoEnvAndNoConnection_LeavesDefaults()
    {
        var settings = new AppSettings();

        AppSettingsEnvironmentSeeder.SeedFromEnvironment(settings, null, null, Builder);

        Assert.Equal(DatabaseProvider.MSSQL, settings.Provider);
        Assert.Equal(string.Empty, settings.ConnectionString);
    }

    [Theory]
    [InlineData("svr", "db", "", true)]
    [InlineData("", "", "Server=x;Database=y;", true)]
    [InlineData("svr", "", "", false)]   // server but no database, and no legacy connection string
    [InlineData("", "", "", false)]
    public void HasConfiguredConnection_Cases(string server, string database, string connStr, bool expected)
    {
        var settings = new AppSettings
        {
            ConnectionString = connStr,
            Connection = new DatabaseConnectionSettings { Server = server, Database = database },
        };

        Assert.Equal(expected, AppSettingsEnvironmentSeeder.HasConfiguredConnection(settings));
    }
}
