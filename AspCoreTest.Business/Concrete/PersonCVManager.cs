using AspCore.Business.Manager;
using AspCoreTest.Business.Abstract;
using AspCoreTest.DataAccess.Abstract;
using AspCoreTest.Entities.Models;

namespace testbusiness.Concrete
{
    public class PersonCVManager : DocumentEntityManager<PersonCv, IPersonCvDAL>, IPersonCVService
    {
    }
}
