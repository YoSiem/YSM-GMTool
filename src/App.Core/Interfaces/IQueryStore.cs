using App.Core.Enums;

namespace App.Core.Interfaces;

public interface IQueryStore
{
    string GetQuery(DatabaseProvider provider, QueryEntity entity);
}
