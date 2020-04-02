using AspCore.ElasticSearchApiClient.General;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace AspCore.ElasticSearchApiClient.QueryBuilder.Concrete
{
    public class FilterSourceBuilder<T> where T : class
    {
        public FilterSource filterSource;
        public FilterSourceBuilder()
        {
            filterSource = new FilterSource();
        }
        public FilterSourceBuilder<T> IncludeAll()
        {
            filterSource.includeAll = true;
            return this;
        }
        public FilterSourceBuilder<T> ExcludeAll()
        {
            filterSource.excludeAll = true;
            return this;
        }

        public FilterSourceBuilder<T> IncludeFields(params Expression<Func<T, object>>[] includeFields)
        {
            if (includeFields != null && includeFields.Length > 0)
            {
                List<string> properties = new List<string>();
                foreach (var fieldDescriptor in includeFields)
                {
                    properties.Add(fieldDescriptor.GetPropertyName());
                }

                filterSource.includeFields = properties.ToArray();
            }
            return this;
        }
        public FilterSourceBuilder<T> IncludeFields(params string[] includeFields)
        {
            if (includeFields != null && includeFields.Length > 0)
            {
                filterSource.includeFields = includeFields;
            }
            return this;
        }
        public FilterSourceBuilder<T> ExcludeFields(params Expression<Func<T, object>>[] excludeFields)
        {
            if (excludeFields != null && excludeFields.Length > 0)
            {
                List<string> properties = new List<string>();
                foreach (var fieldDescriptor in excludeFields)
                {
                    properties.Add(fieldDescriptor.GetPropertyName());
                }

                filterSource.excludeFields = properties.ToArray();
            }
            return this;
        }
        public FilterSourceBuilder<T> ExcludeFields(params string[] excludeFields)
        {
            if (excludeFields != null && excludeFields.Length > 0)
            {
                filterSource.excludeFields = excludeFields;
            }
            return this;
        }
    }
}
