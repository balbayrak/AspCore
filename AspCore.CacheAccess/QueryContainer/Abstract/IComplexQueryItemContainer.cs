using AspCore.CacheAccess.QueryContiner.Concrete;

namespace AspCore.CacheAccess.QueryContiner.Abstract
{
    public interface IComplexQueryItemContainer : IQueryItemContainer 
    {
        ShouldQueryItemContainer shouldQueryContainer { get; set; }

        MustQueryItemContainer mustQueryContainer { get; set; }

        MustNotQueryItemContainer mustNotQueryContainer { get; set; }

        FilterQueryItemContainer filterQueryContainer { get; set; }
    }
}
