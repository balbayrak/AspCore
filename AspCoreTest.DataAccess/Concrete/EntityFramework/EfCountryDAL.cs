using AspCore.DataAccess.EntityFramework;
using AspCoreTest.DataAccess.Abstract;
using AspCoreTest.Entities.Models;
using System;

namespace AspCoreTest.DataAccess.Concrete.EntityFramework
{
    public class EfCountryDAL : EfEntityRepositoryBase<AspCoreTestDbContext, Country>, ICountryDAL
    {
        public EfCountryDAL(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }
    }
}
