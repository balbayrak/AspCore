using AspCore.Business.Abstract;
using AspCoreTest.Dtos.Dtos;
using AspCoreTest.Entities.Models;

namespace testbusiness.Abstract
{
    public interface IPersonService :  IEntityService<Person,PersonDto>, IBusinessService
    {
    }
}
