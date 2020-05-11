using AspCore.DataSearch.Concrete.ElasticSearch;
using AspCoreTest.DataSearch.Abstract;
using AspCoreTest.Entities.SearchableEntities;
using System;

namespace AspCoreTest.WebUI.DataSearch
{
    public class PersonDataSearchEngine : ESDataSearchEngine<PersonSearchEntity>, IPersonDataSearchEngine
    {
        public PersonDataSearchEngine(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }
    }
}
