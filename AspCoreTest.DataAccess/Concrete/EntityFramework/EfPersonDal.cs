using AspCoreTest.DataAccess.Abstract;
using AspCoreTest.Entities.Models;
using AspCore.DataAccess.EntityFramework;
using System;

namespace AspCoreTest.DataAccess.Concrete.EntityFramework
{
    public class EfPersonDal : EfEntityRepositoryBase<AspCoreTestDbContext, Person>, IPersonDal
    {
        public EfPersonDal(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }
    }
}
