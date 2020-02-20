using AspCore.DataAccess.Abstract;
using AspCoreTest.Entities.Models;

namespace AspCoreTest.DataAccess.Abstract
{
    public interface IAdminDAL : IEntityRepository<Admin>
    {
    }
}
