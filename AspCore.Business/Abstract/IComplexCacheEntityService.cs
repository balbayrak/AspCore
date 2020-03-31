using AspCore.Entities.EntityType;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.Business.Abstract
{
    public interface IComplexCacheEntityService<TEntity,TCacheEntity> : IEntityService<TEntity>
        where TCacheEntity : class, ICacheEntity, new()
        where TEntity : class, IEntity, new()
    {
    }
}
