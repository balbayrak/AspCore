using AspCore.Entities.EntityFilter;
using AspCore.Entities.EntityType;
using AspCore.Extension;
using AspCore.Utilities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace AspCore.DataAccess.General
{
    public static class DataAccessFilterExt
    {
        public static DataAccessFilter<TEntity> ToDataFilter<TEntity>(this EntityFilter filter) where TEntity : class, IEntity
        {
            DataAccessFilter<TEntity> dataAccessFilter = new DataAccessFilter<TEntity>();
            dataAccessFilter.page = filter.page;
            dataAccessFilter.pageSize = filter.pageSize;

            if (filter.search != null && !string.IsNullOrEmpty(filter.search.searchValue))
            {
                dataAccessFilter.searchQuery = filter.GetSearchExpression<TEntity>();
            }
            if (filter.sorters != null)
            {
                dataAccessFilter.sorter = filter.sorters.ToSortingExpressionList<TEntity>();
            }

            return dataAccessFilter;
        }

        public static DataAccessFilter<TEntity> Load<TEntity>(this DataAccessFilter<TEntity> dataAccessFilter, Expression<Func<TEntity, object>> include) where TEntity : class, IEntity
        {
            dataAccessFilter.includes = dataAccessFilter.includes ?? new List<Expression<Func<TEntity, object>>>();
            dataAccessFilter.includes.Add(include);
            return dataAccessFilter;
        }
    }
}
