using AspCore.CacheEntityClient.QueryBuilder.Abstract;
using AspCore.CacheEntityClient.QueryBuilder.Concrete;
using AspCore.CacheEntityClient.QueryContiner.Abstract;

namespace AspCore.CacheEntityClient.QueryContiner.Concrete
{
    public class BasicQueryItemContainer : IBasicQueryItemContainer
    {
        public IQueryItem query { get; set; }
        public BasicQueryItemContainer()
        {

        }
        public BasicQueryItemContainer(QueryItem query)
        {
            this.query = query;
        }
    }
}
