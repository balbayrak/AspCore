using AspCore.Entities.EntityType;

namespace AspCore.Business.Abstract
{
    public interface ICacheEntityService<TEntity> : IEntityService<TEntity>
       where TEntity : class, ICacheEntity, new()
    {
    }
}
