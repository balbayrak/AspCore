using Microsoft.Extensions.Caching.Distributed;
using System;

namespace AspCore.Storage.Concrete
{
    public class RedisCacheManager : CacheManager
    {
        public RedisCacheManager(IDistributedCache cache, Guid uniqueKey) : base(cache, uniqueKey)
        {
        }
    }
}
