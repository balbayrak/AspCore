using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using AspCore.Business.Specifications.Abstract;

namespace AspCore.Business.Specifications.Concrete
{
   public class NotSpecification<T>: Specification<T>
    {
        private readonly ISpecification<T> _specification;

        public NotSpecification(ISpecification<T> specification)
        {
            _specification = specification;
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            var expression = _specification.ToExpression();
            return Expression.Lambda<Func<T, bool>>(
                Expression.Not(expression.Body),
                expression.Parameters
            );
        }
    }
}
