using AspCore.Business.Concrete;
using AspCore.Dependency.DependencyAttributes;
using AspCoreTest.Business.Abstract;
using AspCoreTest.DataAccess.Abstract;
using AspCoreTest.Entities.Models;
using System;

namespace testbusiness.Concrete
{
    [ExposedService(typeof(IPersonCVService))]
    public class PersonCVManager : DocumentEntityManager<PersonCv, IPersonCvDAL>, IPersonCVService
    {
        public PersonCVManager(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }
    }
  
    public class PersonCVManager2 : DocumentEntityManager<PersonCv, IPersonCvDAL>, IPersonCVService
    {
        public PersonCVManager2(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}
