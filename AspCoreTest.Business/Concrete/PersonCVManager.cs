using AspCore.Business.Concrete;
using AspCore.Dependency.DependencyAttributes;
using AspCoreTest.Business.Abstract;
using AspCoreTest.DataAccess.Abstract;
using AspCoreTest.Entities.Models;

namespace testbusiness.Concrete
{
    [ExposedService(typeof(IPersonCVService))]
    public class PersonCVManager : DocumentEntityManager<PersonCv, IPersonCvDAL>, IPersonCVService
    {
    }
  
    public class PersonCVManager2 : DocumentEntityManager<PersonCv, IPersonCvDAL>, IPersonCVService
    {
    }
}
