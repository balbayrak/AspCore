using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace AspCore.Business.Specifications.Abstract
{
   public interface ISpecification<T>
    {
        bool IsSatisfiedBy(T obj);
        Expression<Func<T, bool>> ToExpression();
    }
}
