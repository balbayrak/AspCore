using AspCore.Dependency.Abstract;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;

namespace AspCore.Business.Abstract
{
    public interface ISearchableEntityService<TSearchableEntity>: ITransientType
        where TSearchableEntity : class, ISearchableEntity, new()
    {
        ServiceResult<TSearchableEntity[]> GetSearchableEntities();
    }
}
