using AspCore.Entities.EntityType;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.Entities.Cache
{
    public class CacheResult<T>
       where T : class, ICacheEntity, new()
    {
        public List<T> cacheItems { get; set; }

        public MinMax minMax { get; set; }

        public int totalCount { get; set; }

        public int searchCount { get; set; }
    }
}
