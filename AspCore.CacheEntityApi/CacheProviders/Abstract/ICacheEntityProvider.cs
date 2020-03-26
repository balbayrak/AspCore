using AspCore.CacheEntityClient.QueryBuilder.Concrete;
using AspCore.Dependency.Abstract;
using AspCore.Entities.Cache;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using System.Collections.Generic;

namespace AspCore.CacheEntityApi.CacheProviders.Abstract
{
    public interface ICacheEntityProvider<T> : ITransientType where T : class, ICacheEntity, new()
    {
        ServiceResult<bool> CreateCacheItem(string cacheName, T cacheItem);

        ServiceResult<bool> UpdateCacheItem(string cacheName, T cacheItem);

        ServiceResult<bool> DeleteCacheItem(string cacheName, T cacheItem);

        ServiceResult<CacheResult<T>> ReadCacheItem(SearchRequestItem cacheRequestItem);

        ServiceResult<bool> UpdateCacheItemList(string cacheName, List<T> cacheItemList);

        ServiceResult<bool> DeleteCacheItemList(string cacheName, List<T> cacheItemList);

        ServiceResult<bool> CreateCacheItemList(string cacheName, List<T> cacheItemList);

        ServiceResult<MinMax> MinMaxCacheItem(SearchRequestItem cacheRequestItem);
    }
}
