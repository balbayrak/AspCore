using AspCore.ElasticSearchApiClient.QueryBuilder.Abstract;
using AspCore.ElasticSearchApiClient.QueryBuilder.Concrete;
using AspCore.ElasticSearchApiClient.QueryContiner.Abstract;

namespace AspCore.ElasticSearchApiClient.QueryContiner.Concrete
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
