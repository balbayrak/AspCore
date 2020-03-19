using AspCore.CacheAccess.QueryBuilder.Abstract;
using AspCore.CacheAccess.QueryBuilder.Concrete;
using AspCore.CacheAccess.QueryContiner.Abstract;

namespace AspCore.CacheAccess.QueryContiner.Concrete
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
