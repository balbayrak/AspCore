using AspCoreTest.DataAccess.Abstract;
using AspCoreTest.Entities.Models;
using AspCore.DataAccess.EntityFramework;
using System;

namespace AspCoreTest.DataAccess.Concrete.EntityFramework
{
    public class EfPersonCvDAL : EfEntityRepositoryBase<AspCoreTestDbContext, PersonCv>, IPersonCvDAL
    {
        public EfPersonCvDAL(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }
    }
}
