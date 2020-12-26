using System;
using System.Linq.Expressions;
using AspCore.Entities.Expression;

namespace AspCore.Utilities.ExpressionUtilities
{
    public class FilterExpression<TSource>
    {
        public Operation OperationValue { get; set; }
        public Expression<Func<TSource, object>> Property { get; set; }
        public object PropertyValue { get; set; }
        public Expression<Func<TSource, bool>> Exp
        {
            get
            {
                return ExpressionBuilder.GetExpression<TSource>(this);
            }
        }

        public FilterExpression(Expression<Func<TSource, object>> property, object value, Operation operation)
        {
            this.Property = property;
            this.OperationValue = operation;
            this.PropertyValue = value;
        }
    }
}
