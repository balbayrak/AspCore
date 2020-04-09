using AspCore.Entities.EntityType;
using AspCore.Entities.General;

namespace AspCore.Business.Abstract
{
    public interface ISearchableEntityService<TEntity, TSearchableEntity> : IEntityService<TEntity>
        where TSearchableEntity : class, ISearchableEntity, new()
        where TEntity : class, IEntity, new()
    {
        ServiceResult<bool> ResetSearchableData(bool initWithData);
        ServiceResult<TSearchableEntity[]> GetSearchableEntities();
    }
}
