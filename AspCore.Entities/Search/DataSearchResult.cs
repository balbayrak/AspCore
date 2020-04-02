using AspCore.Entities.EntityType;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.Entities.Search
{
    public class DataSearchResult<T>
       where T : class, ISearchableEntity, new()
    {
        public List<T> items { get; set; }

        public int totalCount { get; set; }

        public int searchCount { get; set; }

        public List<AggregationResult> aggregations { get; set; }
    }
}
