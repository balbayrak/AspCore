using AspCore.DataSearch.Abstract;
using AspCoreTest.Entities.SearchableEntities;

namespace AspCoreTest.DataSearch.Abstract
{
    public interface IPersonDataSearchEngine : IDataSearchEngine<PersonSearchEntity>
    {
    }
}
