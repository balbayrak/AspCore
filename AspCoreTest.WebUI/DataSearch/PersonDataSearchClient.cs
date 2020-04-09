using AspCore.DataSearch.Abstract;
using AspCore.DataSearch.Concrete.ElasticSearch;
using AspCoreTest.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspCoreTest.WebUI.DataSearch
{
    public class PersonDataSearchClient : ESDataSearchClient<Person>, IPersonDataSearchClient
    {
    }
}
