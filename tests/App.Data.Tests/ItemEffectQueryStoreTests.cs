using App.Core.Enums;
using App.Core.Services;
using Xunit;

namespace App.Data.Tests;

public class ItemEffectQueryStoreTests
{
    [Theory]
    [InlineData(DatabaseProvider.MSSQL, "SELECT mssql")]
    [InlineData(DatabaseProvider.MySQL, "SELECT mysql")]
    public void GetQuery_ItemEffects_ReturnsConfiguredQuery(DatabaseProvider provider, string expected)
    {
        var path = Path.GetTempFileName();
        try
        {
            File.WriteAllText(path, """
            {
              "MSSQL": { "ItemEffects": "SELECT mssql" },
              "MySQL": { "ItemEffects": "SELECT mysql" }
            }
            """);

            var store = new FileQueryStore(path);

            Assert.Equal(expected, store.GetQuery(provider, QueryEntity.ItemEffects));
        }
        finally
        {
            File.Delete(path);
        }
    }
}
