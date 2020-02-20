using AspCoreTest.DataAccess.Abstract;
using AspCoreTest.Entities.Models;
using AspCore.DataAccess.EntityFramework;

namespace AspCoreTest.DataAccess.Concrete.EntityFramework
{
    public class EfRoleDAL : EfEntityRepositoryBase<AspCoreTestDbContext, Role>, IRoleDAL
    {
    }
}
