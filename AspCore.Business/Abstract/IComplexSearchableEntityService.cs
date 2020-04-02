using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.Business.Abstract
{
    public interface IComplexSearchableEntityService<TEntity, TSearchableEntity> : IEntityService<TEntity>
        where TSearchableEntity : class, ISearchableEntity, new()
        where TEntity : class, IEntity, new()
    {
        ServiceResult<TSearchableEntity[]> GetSearchableEntities();
    }
}
