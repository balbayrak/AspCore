using System;
using System.Collections.Generic;
using System.Text;
using AspCore.Business.Specifications.Abstract;
using AspCore.Business.Specifications.Concrete;

namespace AspCore.Business.Specifications
{
    public static class SpecificationExtensions
    {
        public static ISpecification<T> And<T>(this ISpecification<T> specification,
            ISpecification<T> other)
        {
            if (specification == null)
            {
                throw new ArgumentNullException(nameof(specification));
            }

            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }
            return new AndSpecification<T>(specification, other);
        }
        public static ISpecification<T> Or<T>(this ISpecification<T> specification,
            ISpecification<T> other)
        {
            if (specification == null)
            {
                throw new ArgumentNullException(nameof(specification));
            }

            if (other == null)
            {
                throw new ArgumentNullException(nameof(other));
            }

            return new OrSpecification<T>(specification, other);
        }

        public static ISpecification<T> Not<T>(this ISpecification<T> specification)
        {
            if (specification==null)
            {
                throw new ArgumentNullException(nameof(specification));
            }
            return new NotSpecification<T>(specification);
        }
    }
}
