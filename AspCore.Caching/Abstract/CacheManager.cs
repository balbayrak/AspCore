using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.Caching.Abstract
{
    public class CacheManager
    {
        protected string uniqueCacheKey { get; private set; }

        public CacheManager()
        {
            uniqueCacheKey = Guid.NewGuid().ToString("N");
        }
    }
}
