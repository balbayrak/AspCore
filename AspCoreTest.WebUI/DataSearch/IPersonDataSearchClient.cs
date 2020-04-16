using AspCore.DataSearch.Abstract;
using AspCoreTest.Entities.SearchableEntities;

namespace AspCoreTest.WebUI.DataSearch
{
    public interface IPersonDataSearchClient : IDataSearchClient<PersonSearchEntity>
    {
    }
}
