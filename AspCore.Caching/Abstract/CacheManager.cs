using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.Caching.Abstract
{
    public abstract class CacheManager
    {
        protected string uniqueCacheKey { get; private set; }

        protected CacheManager()
        {
            uniqueCacheKey = Guid.NewGuid().ToString("N");
        }
    }
}
