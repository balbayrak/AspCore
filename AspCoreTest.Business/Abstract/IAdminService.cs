using AspCore.Business.Abstract;
using AspCoreTest.Dtos.Dtos;
using AspCoreTest.Entities.Models;

namespace testbusiness.Abstract
{
    public interface IAdminService :  IEntityService<Admin,AdminDto>, IBusinessService
    {
    }
}
