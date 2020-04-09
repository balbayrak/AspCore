using AspCore.DataSearch.Abstract;
using AspCore.ElasticSearchApiClient;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;

namespace AspCore.DataSearch.Concrete.ElasticSearch
{
    public class ESDataSearchEngine<T> : ElasticClient<T>, IDataSearchEngine<T>
         where T : class, ISearchableEntity, new()
    {
        public ESDataSearchEngine(string apiClientKey, string elasticApiRoute) : base(apiClientKey, elasticApiRoute)
        {
        }
    }
}
