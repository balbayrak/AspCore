using AspCore.DataSearch.Concrete.ElasticSearch;
using AspCoreTest.Entities.SearchableEntities;
using System;

namespace AspCoreTest.WebUI.DataSearch
{
    public class PersonDataSearchClient : ESDataSearchClient<PersonSearchEntity>, IPersonDataSearchClient
    {
        public PersonDataSearchClient(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }
    }
}
