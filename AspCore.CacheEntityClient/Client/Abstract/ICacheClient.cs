using AspCore.CacheEntityClient.QueryBuilder.Concrete;
using AspCore.Entities.Cache;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using System;

namespace AspCore.CacheEntityClient
{
    public interface ICacheClient<T>  
        where T : class, ICacheEntity,new()
    {
        string cacheKey { get;}
        ServiceResult<bool> Create(params T[] cacheItems);
        ServiceResult<CacheResult<T>> Read(Func<CacheSearchBuilder<T>, CacheSearchBuilder<T>> builder);
        ServiceResult<CacheResult<T>> Read(T cacheItem);
        ServiceResult<bool> Update(params T[] cacheItems);
        ServiceResult<bool> Delete(params T[] cacheItems);
        ServiceResult<MinMax> MinMax(T cacheItem);

    }
}
