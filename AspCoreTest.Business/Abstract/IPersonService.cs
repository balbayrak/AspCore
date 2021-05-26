using AspCore.Business.Abstract;
using AspCore.Entities.General;
using AspCoreTest.Dtos.Dtos;
using AspCoreTest.Entities.Models;

namespace testbusiness.Abstract
{
    public interface IPersonService :  IEntityService<Person,PersonDto>, IBusinessService
    {
        ServiceResult<bool> Add(PersonDto entities);
    }
}
