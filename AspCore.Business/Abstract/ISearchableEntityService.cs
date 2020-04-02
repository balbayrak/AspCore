using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using System.Collections.Generic;

namespace AspCore.Business.Abstract
{
    public interface ISearchableEntityService<TSearchableEntity> : IEntityService<TSearchableEntity>
       where TSearchableEntity : class, ISearchableEntity, new()
    {
        ServiceResult<TSearchableEntity[]> GetSearchableEntities();
    }
}
