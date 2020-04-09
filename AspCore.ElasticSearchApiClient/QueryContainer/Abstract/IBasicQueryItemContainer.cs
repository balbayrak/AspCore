using AspCore.ElasticSearchApiClient.QueryBuilder.Abstract;
using AspCore.ElasticSearchApiClient.QueryBuilder.Concrete;

namespace AspCore.ElasticSearchApiClient.QueryContiner.Abstract
{
    public interface IBasicQueryItemContainer : IQueryItemContainer
    {
        QueryItem query { get; set; }
    }
}
