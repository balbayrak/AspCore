using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AspCore.Entities.DataTable;
using AspCore.Entities.Expression;

namespace AspCore.Utilities
{
    public static class CustomSorter
    {
        public static IOrderedEnumerable<TSource> CustomSort<TSource, TKey>(this IEnumerable<TSource> items, EnumSortingDirection direction, Func<TSource, TKey> keySelector)
        {
            if (direction == EnumSortingDirection.Ascending)
            {
                return items.OrderBy(keySelector);
            }

            return items.OrderByDescending(keySelector);
        }

        public static IOrderedEnumerable<TSource> CustomSort<TSource, TKey>(this IOrderedEnumerable<TSource> items, EnumSortingDirection direction, Func<TSource, TKey> keySelector)
        {
            if (direction == EnumSortingDirection.Ascending)
            {
                return items.ThenBy(keySelector);
            }

            return items.ThenByDescending(keySelector);
        }

        public static List<TSource> CustomSort<TSource>(this List<TSource> items, Comparison<TSource> comparison, EnumSortingDirection direction)
        {
            items.Sort(comparison);
            if (!direction.GetHashCode().Equals(EnumSortingDirection.Descending.GetHashCode()))
            {
                items.Reverse();
            }
            return items;
        }

        public static List<TSource> CustomSort<TSource>(this List<TSource> items, List<SortingColumn> sortingColumns)
        {
            foreach (SortingColumn item in sortingColumns)
            {
                items.CustomSort(CustomComparison.GetCustomComparison<TSource>(item.propertyName), item.sortDirection);
            }
            return items;
        }

        public static List<TSource> CustomSort<TSource>(this List<TSource> items, Expression<Func<TSource, object>> prop, EnumSortingDirection direction)
        {
            return items.CustomSort(CustomComparison.GetCustomComparison<TSource>(ExpressionBuilder.GetExpressionFieldName(prop)), direction);
        }

        public static List<TSource> CustomSort<TSource>(this List<TSource> items, SortingExpression<TSource> prop)
        {
            return items.CustomSort(CustomComparison.GetCustomComparison<TSource>(ExpressionBuilder.GetExpressionFieldName(prop.Property)), prop.SortDirection);
        }

        public static List<TSource> CustomSort<TSource>(this List<TSource> items, List<SortingExpression<TSource>> sortingProperties)
        {
            foreach (SortingExpression<TSource> item in sortingProperties)
            {
                items.CustomSort(item);
            }
            return items;
        }

        public static IQueryable<TSource> CustomSort<TSource>(this IQueryable<TSource> items, List<SortingExpression<TSource>> sortingProperties)
        {
            foreach (SortingExpression<TSource> item in sortingProperties)
            {
                items = items.CustomSort(item);
            }
            return items;
        }

        public static IQueryable<TSource> CustomSort<TSource>(this IQueryable<TSource> items, SortingExpression<TSource> prop)
        {
            if (prop.SortDirection.Equals(EnumSortingDirection.Descending))
            {
                items = items.OrderByDescending(prop.Property);
            }
            else
            {
                items = items.OrderBy(prop.Property);
            }
            return items;
        }


    }
}
