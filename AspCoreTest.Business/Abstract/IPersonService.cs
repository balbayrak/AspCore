using AspCore.Business.Abstract;
using AspCoreTest.Entities.Models;
using AspCoreTest.Entities.SearchableEntities;

namespace testbusiness.Abstract
{
    public interface IPersonService :  IEntityService<Person>, IBusinessService
    {
    }
}
