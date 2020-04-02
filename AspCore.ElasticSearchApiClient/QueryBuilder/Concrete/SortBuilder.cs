using AspCore.ElasticSearchApiClient.General;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace AspCore.ElasticSearchApiClient.QueryBuilder.Concrete
{
    public class SortBuilder<T> where T : class
    {
        private SortItem sortItem;

        public SortBuilder()
        {
            this.sortItem = new SortItem();
        }
        public SortItem Ascending(params Expression<Func<T, object>>[] ascendingFields)
        {
            if (ascendingFields != null && ascendingFields.Length > 0)
            {
                List<string> properties = new List<string>();
                foreach (var fieldDescriptor in ascendingFields)
                {
                    properties.Add(fieldDescriptor.GetPropertyName());
                }

                sortItem.ascendingFields = properties.ToArray();
            }
            return sortItem;
        }

        public SortItem Descending(params Expression<Func<T, object>>[] descendingFields)
        {
            if (descendingFields != null && descendingFields.Length > 0)
            {
                List<string> properties = new List<string>();
                foreach (var fieldDescriptor in descendingFields)
                {
                    properties.Add(fieldDescriptor.GetSortPropertyName());
                }

                sortItem.descendingFields = properties.ToArray();
            }
            return sortItem;
        }

    }
}
