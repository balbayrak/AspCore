using AspCore.ElasticSearchApiClient.QueryBuilder.Abstract;
using AspCore.ElasticSearchApiClient.QueryBuilder.Concrete;

namespace AspCore.ElasticSearchApiClient.QueryContiner.Abstract
{
    public interface IBasicQueryItemContainer : IQueryItemContainer
    {
        IQueryItem query { get; set; }
    }
}
