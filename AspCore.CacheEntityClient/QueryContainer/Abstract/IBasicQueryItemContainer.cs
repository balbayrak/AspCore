using AspCore.CacheEntityClient.QueryBuilder.Abstract;

namespace AspCore.CacheEntityClient.QueryContiner.Abstract
{
    public interface IBasicQueryItemContainer : IQueryItemContainer
    {
        IQueryItem query { get; set; }
    }
}
