using AspCore.Business.Concrete;
using AspCoreTest.DataAccess.Abstract;
using AspCoreTest.Entities.Models;
using testbusiness.Abstract;

namespace testbusiness.Concrete
{
    public class PersonManager : BaseSearchableEntityManager<IPersonDal, Person>, IPersonService
    {
    }
}
