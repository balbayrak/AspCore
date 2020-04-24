using AspCore.Business.Concrete;
using AspCoreTest.Business.Abstract;
using AspCoreTest.DataAccess.Abstract;
using AspCoreTest.Entities.Models;
using System;

namespace testbusiness.Concrete
{
    public class PersonCVManager : DocumentEntityManager<PersonCv, IPersonCvDAL>, IPersonCVService
    {
        public PersonCVManager(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }
    }
}
