using AspCore.Business.Abstract;
using AspCoreTest.Entities.Models;
using AspCoreTest.Entities.SearchableEntities;

namespace testbusiness.Abstract
{
    public interface IPersonService : ISearchableEntityService<PersonSearchEntity>, IEntityService<Person>, IBusinessService
    {
    }
}
