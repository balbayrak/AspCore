using System;
using System.Linq.Expressions;
using AspCore.Entities.EntityType;

namespace AspCore.Utilities.ExpressionUtilities
{
    public class IncludeExpression<TSource>
    {
        public Expression<Func<TSource, object>> Include { get; set; }

        public Expression<Func<TSource, bool>> isDeletedExpression { get; set; }

        public string IncludeName
        {
            get
            {
                return ExpressionBuilder.GetExpressionFieldName(Include);
            }
        }

        public IncludeExpression(Expression<Func<TSource, object>> include)
        {
            this.Include = include;
            isDeletedExpression = ExpressionBuilder.GetEqualsExpression<TSource>(IncludeName.Substring(IncludeName.IndexOf('.')).Trim('.') + "." + nameof(BaseEntity.IsDeleted), false);
        }

    }
}
