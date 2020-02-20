using AspCore.Business.Manager;
using AspCoreTest.DataAccess.Abstract;
using AspCoreTest.Entities.Models;
using testbusiness.Abstract;

namespace testbusiness.Concrete
{
    public class PersonManager : BaseEntityManager<IPersonDal, Person>, IPersonService
    {
    }
}
