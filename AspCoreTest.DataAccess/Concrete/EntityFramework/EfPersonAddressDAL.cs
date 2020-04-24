using AspCoreTest.DataAccess.Abstract;
using AspCoreTest.Entities.Models;
using AspCore.DataAccess.EntityFramework;
using System;

namespace AspCoreTest.DataAccess.Concrete.EntityFramework
{
    public class EfPersonAddressDAL : EfEntityRepositoryBase<AspCoreTestDbContext, PersonAddress>, IPersonAddressDAL
    {
        public EfPersonAddressDAL(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }
    }
}
