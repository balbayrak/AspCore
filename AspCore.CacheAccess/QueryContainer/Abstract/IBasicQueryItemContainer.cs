using AspCore.CacheAccess.QueryBuilder.Abstract;

namespace AspCore.CacheAccess.QueryContiner.Abstract
{
    public interface IBasicQueryItemContainer : IQueryItemContainer
    {
        IQueryItem query { get; set; }
    }
}
