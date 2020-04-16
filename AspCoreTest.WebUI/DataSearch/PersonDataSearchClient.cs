using AspCore.DataSearch.Concrete.ElasticSearch;
using AspCoreTest.Entities.SearchableEntities;

namespace AspCoreTest.WebUI.DataSearch
{
    public class PersonDataSearchClient : ESDataSearchClient<PersonSearchEntity>, IPersonDataSearchClient
    {
    }
}
