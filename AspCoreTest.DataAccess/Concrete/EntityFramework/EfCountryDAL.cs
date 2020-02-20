using AspCore.DataAccess.EntityFramework;
using AspCoreTest.DataAccess.Abstract;
using AspCoreTest.Entities.Models;

namespace AspCoreTest.DataAccess.Concrete.EntityFramework
{
    public class EfCountryDAL : EfEntityRepositoryBase<AspCoreTestDbContext, Country>, ICountryDAL
    {
    }
}
