using AspCoreTest.DataAccess.Abstract;
using AspCoreTest.Entities.Models;
using AspCore.DataAccess.EntityFramework;
using System;

namespace AspCoreTest.DataAccess.Concrete.EntityFramework
{
    public class EfAdminDAL : EfEntityRepositoryBase<AspCoreTestDbContext, Admin>, IAdminDAL
    {
        public EfAdminDAL(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }
    }
}
