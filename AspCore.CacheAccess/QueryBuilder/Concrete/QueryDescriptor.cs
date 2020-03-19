using AspCore.CacheAccess.QueryContiner.Concrete;
using System;

namespace AspCore.CacheAccess.QueryBuilder.Concrete
{
    public class QueryDescriptor<T> where T : class
    {
        internal BoolQueryBuilder<T> boolQueryBuilder;

        public QueryDescriptor()
        {
            this.boolQueryBuilder = new BoolQueryBuilder<T>();

        }
        public ComplexQueryItemContainer Bool(params Func<BoolQueryBuilder<T>, QueryItemContainer>[] queries)
        {
            BoolQueryBuilder<T> boolQueryBuilder = new BoolQueryBuilder<T>();
            ComplexQueryItemContainer complexContainer = new ComplexQueryItemContainer();

            foreach (var item in queries)
            {
                QueryItemContainer container = item(boolQueryBuilder);
                if (container is ShouldQueryItemContainer)
                {
                    complexContainer.shouldQueryContainer = (ShouldQueryItemContainer)container;
                }
                else if (container is MustQueryItemContainer)
                {
                    complexContainer.mustQueryContainer = (MustQueryItemContainer)container;
                }
                else if(container is MustNotQueryItemContainer)
                {
                    complexContainer.mustNotQueryContainer = (MustNotQueryItemContainer)container;
                }
                else
                {
                    complexContainer.filterQueryContainer = (FilterQueryItemContainer)container;
                }
            }

            return complexContainer;
        }
    }
}
