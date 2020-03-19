using AspCore.CacheAccess.QueryBuilder.Concrete;
using AspCore.CacheAccess.QueryResult;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using System;
using System.Collections.Generic;

namespace AspCore.CacheAccess.Helper.Abstract
{
    public interface ICacheClient<T> : IDisposable 
        where T : class, IEntity,new()
    {
        ServiceResult<bool> Create(params T[] cacheItems);
        ServiceResult<CacheResult<T>> Read(Func<CacheSearchBuilder<T>, CacheSearchBuilder<T>> builder);
        ServiceResult<CacheResult<T>> Read(T cacheItem);
        ServiceResult<bool> Update(params T[] cacheItems);
        ServiceResult<bool> Delete(params T[] cacheItems);
        ServiceResult<CacheResult<T>> MinMax(T cacheItem);

    }
}
