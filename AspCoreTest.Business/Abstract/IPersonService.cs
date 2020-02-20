using AspCore.Business.Abstract;
using AspCoreTest.Entities.Models;

namespace testbusiness.Abstract
{
    public interface IPersonService: IEntityService<Person>,IBusinessService
    {
    }
}
