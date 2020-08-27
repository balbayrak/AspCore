using AspCore.DataSearch.Abstract;
using AspCore.Entities.General;
using AspCore.Entities.Search;
using AspCoreTest.Entities.ModelFilters;
using AspCoreTest.Entities.SearchableEntities;
using System.Threading.Tasks;

namespace AspCoreTest.DataSearch.Abstract
{
    public interface IPersonDataSearchEngine : IDataSearchEngine<PersonSearchEntity>
    {
        Task<ServiceResult<DataSearchResult<PersonSearchEntity>>> FindByYetkiliKisi(PersonFilter filter);
    }
}
