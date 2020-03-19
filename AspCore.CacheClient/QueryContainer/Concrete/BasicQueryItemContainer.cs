using AspCore.CacheClient.QueryBuilder.Abstract;
using AspCore.CacheClient.QueryBuilder.Concrete;
using AspCore.CacheClient.QueryContiner.Abstract;

namespace AspCore.CacheClient.QueryContiner.Concrete
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
