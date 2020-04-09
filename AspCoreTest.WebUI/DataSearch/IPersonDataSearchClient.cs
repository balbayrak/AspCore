using AspCore.DataSearch.Abstract;
using AspCoreTest.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspCoreTest.WebUI.DataSearch
{
    public interface IPersonDataSearchClient : IDataSearchClient<Person>
    {
    }
}
