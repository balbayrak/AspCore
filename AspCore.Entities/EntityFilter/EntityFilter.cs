using System;
using System.Collections.Generic;
using AspCore.Entities.EntityType;

namespace AspCore.Entities.EntityFilter
{
    public class EntityFilter<TEntity>
    where TEntity : class, IEntity, new()
    {
        public Guid id { get; set; }

        public List<SortingType<TEntity>> sorters { get; set; }

        public SearchType search { get; set; }

        public int? page { get; set; }

        public int? pageSize { get; set; }

        public EntityFilter()
        {
            sorters = null;
            page = null;
            pageSize = null;
            search = null;
        }
    }
}
