using System.Collections.Generic;

namespace AspCore.ElasticSearchApiClient.QueryContiner.Concrete
{
    public class BoolQueryContainer 
    {
        public List<BasicQueryItemContainer> queries;

        public ComplexQueryItemContainer complexQuery;
    }
}
