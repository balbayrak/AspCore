using AspCore.Entities.EntityType;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.Business.Abstract
{
    public interface ICacheEntityService<TEntity> : IEntityService<TEntity>
       where TEntity : class, ICacheEntity, new()
    {
    }
}
