using AspCore.ElasticSearchApiClient.QueryContiner.Concrete;

namespace AspCore.ElasticSearchApiClient.QueryContiner.Abstract
{
    public interface IComplexQueryItemContainer : IQueryItemContainer 
    {
        ShouldQueryItemContainer shouldQueryContainer { get; set; }

        MustQueryItemContainer mustQueryContainer { get; set; }

        MustNotQueryItemContainer mustNotQueryContainer { get; set; }

        FilterQueryItemContainer filterQueryContainer { get; set; }
    }
}
