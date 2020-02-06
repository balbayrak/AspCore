using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AspCore.Entities.Expression;

namespace AspCore.Utilities
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
