using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AspCore.Entities.EntityType;

namespace AspCore.Utilities
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
