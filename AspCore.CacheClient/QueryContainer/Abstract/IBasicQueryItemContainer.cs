using AspCore.CacheClient.QueryBuilder.Abstract;

namespace AspCore.CacheClient.QueryContiner.Abstract
{
    public interface IBasicQueryItemContainer : IQueryItemContainer
    {
        IQueryItem query { get; set; }
    }
}
