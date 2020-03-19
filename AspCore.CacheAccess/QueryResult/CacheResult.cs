using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using System.Collections.Generic;

namespace AspCore.CacheAccess.QueryResult
{
    public class CacheResult<T>
    {
        public IEnumerable<T> cacheItems { get; set; }
        public decimal MinValue { get; set; }
        public decimal MaxValue { get; set; }
    }
}
