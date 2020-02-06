using System;
using System.Linq.Expressions;
using AspCore.Entities.Expression;

namespace AspCore.Utilities
{
    public class SortingExpression<TSource>
    {
        public Expression<Func<TSource, object>> Property { get; set; }

        public EnumSortingDirection SortDirection { get; set; }


        public SortingExpression(Expression<Func<TSource, object>> property, EnumSortingDirection sortDirection)
        {
            this.Property = property;
            this.SortDirection = sortDirection;
        }

        public SortingExpression(string property, EnumSortingDirection sortDirection)
        {
            this.Property = ExpressionBuilder.GetExpression<TSource>(property);
            this.SortDirection = sortDirection;
        }
    }
}
