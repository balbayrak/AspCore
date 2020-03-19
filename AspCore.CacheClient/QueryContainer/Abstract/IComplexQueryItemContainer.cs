using AspCore.CacheClient.QueryContiner.Concrete;

namespace AspCore.CacheClient.QueryContiner.Abstract
{
    public interface IComplexQueryItemContainer : IQueryItemContainer 
    {
        ShouldQueryItemContainer shouldQueryContainer { get; set; }

        MustQueryItemContainer mustQueryContainer { get; set; }

        MustNotQueryItemContainer mustNotQueryContainer { get; set; }

        FilterQueryItemContainer filterQueryContainer { get; set; }
    }
}
