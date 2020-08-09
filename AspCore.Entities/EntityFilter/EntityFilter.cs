using AspCore.Entities.EntityType;
using System;
using System.Collections.Generic;

namespace AspCore.Entities.EntityFilter
{
    public class EntityFilter
    {
        public Guid id { get; set; }

        public List<SortingType> sorters { get; set; }

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
