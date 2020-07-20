using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using AspCore.Business.Specifications.Abstract;

namespace AspCore.Business.Specifications.Concrete
{
    public class OrSpecification<T>:CompositeSpecification<T>
    {
        public OrSpecification(ISpecification<T> left, ISpecification<T> right) : base(left, right)
        {
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
           return Left.ToExpression().Or(Right.ToExpression());
        }
    }
}
