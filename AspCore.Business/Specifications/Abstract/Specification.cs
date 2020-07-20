using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace AspCore.Business.Specifications.Abstract
{
    public abstract class Specification<T>:ISpecification<T>
    {
        public bool IsSatisfiedBy(T obj)
        {
            return ToExpression().Compile()(obj);
        }
        public abstract Expression<Func<T, bool>> ToExpression();

        public static implicit operator Expression<Func<T, bool>>(Specification<T> specification)
        {
            return specification.ToExpression();
        }
    }
}
