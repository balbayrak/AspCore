using AspCoreTest.DataAccess.Abstract;
using AspCoreTest.Entities.Models;
using AspCore.DataAccess.EntityFramework;
using System;

namespace AspCoreTest.DataAccess.Concrete.EntityFramework
{
    public class EfRoleDAL : EfEntityRepositoryBase<AspCoreTestDbContext, Role>, IRoleDAL
    {
        public EfRoleDAL(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }
    }
}
