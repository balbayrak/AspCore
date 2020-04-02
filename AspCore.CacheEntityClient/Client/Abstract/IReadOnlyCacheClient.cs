using AspCore.CacheEntityClient.QueryBuilder.Concrete;
using AspCore.Entities.Cache;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using System;

namespace AspCore.CacheEntityClient
{
    public interface IReadOnlyCacheClient<T>
        where T : class, ISearchableEntity, new()
    {
        string cacheKey { get; }
        ServiceResult<CacheResult<T>> Read(Func<CacheSearchBuilder<T>, CacheSearchBuilder<T>> builder);
    }
}
