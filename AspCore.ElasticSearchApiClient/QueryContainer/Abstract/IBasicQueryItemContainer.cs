using AspCore.ElasticSearchApiClient.QueryBuilder.Abstract;

namespace AspCore.ElasticSearchApiClient.QueryContiner.Abstract
{
    public interface IBasicQueryItemContainer : IQueryItemContainer
    {
        IQueryItem query { get; set; }
    }
}
