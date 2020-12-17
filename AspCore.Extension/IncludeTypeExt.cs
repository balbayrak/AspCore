using AspCore.Dtos.Dto;
using AspCore.Entities.EntityFilter;
using AspCore.Entities.EntityType;
using AspCore.Utilities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AspCore.Utilities.ExpressionUtilities;

namespace AspCore.Extension
{
    public static class IncludeTypeExt
    {
        public static void Load<TEntity>(this List<IncludeType<TEntity>> list, Expression<Func<TEntity, object>> expression)
            where TEntity : class, IEntity, new()
        {
            list.Add(new IncludeType<TEntity>(ExpressionBuilder.GetExpressionFieldName<TEntity>(expression)));
        }

        public static List<IncludeExpression<TEntity>> ToIncludeExpressionList<TEntity>(this List<IncludeType<TEntity>> list)
           where TEntity : class, IEntity, new()
        {
            List<IncludeExpression<TEntity>> includes = new List<IncludeExpression<TEntity>>();
            foreach (var item in list)
            {
                includes.Add(new IncludeExpression<TEntity>(ExpressionBuilder.GetExpression<TEntity>(item.IncludeName)));
            }
            return includes;
        }

        public static List<SortingExpression<TEntity>> ToSortingExpressionList<TEntity>(this List<SortingType> list)
        
        {
            List<SortingExpression<TEntity>> sorters = new List<SortingExpression<TEntity>>();
            foreach (var item in list)
            {
                sorters.Add(new SortingExpression<TEntity>(ExpressionBuilder.GetExpression<TEntity>(item.PropertyName), item.SortDirection));
            }
            return sorters;
        }

        

    }
}
