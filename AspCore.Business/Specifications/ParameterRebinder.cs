using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace AspCore.Business.Specifications
{
    class ParameterRebinder:ExpressionVisitor
    {
    
        private readonly Dictionary<ParameterExpression, ParameterExpression> _map;
        internal ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
        {
            _map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }
        internal static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map,
            Expression exp)
        {
            return new ParameterRebinder(map).Visit(exp);
        }
        protected override Expression VisitParameter(ParameterExpression p)
        {
            if (_map.TryGetValue(p, out var replacement))
            {
                p = replacement;
            }

            return base.VisitParameter(p);
        }
    }
}
