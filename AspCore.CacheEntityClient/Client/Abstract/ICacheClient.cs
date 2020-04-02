using AspCore.CacheEntityClient.QueryBuilder.Concrete;
using AspCore.Entities.Cache;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using System;

namespace AspCore.CacheEntityClient
{
    public interface ICacheClient<T>  : IReadOnlyCacheClient<T>
        where T : class, ISearchableEntity,new()
    {
        ServiceResult<bool> Create(params T[] cacheItems);
        ServiceResult<bool> Update(params T[] cacheItems);
        ServiceResult<bool> Delete(params T[] cacheItems);
        ServiceResult<MinMax> MinMax(T cacheItem);

    }
}
