using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspCore.Dependency.Abstract;
using AspCore.Entities.EntityFilter;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;

namespace AspCore.Business.Abstract
{
    public interface IEntityService<TEntity> : ITransientType
       where TEntity : class, IEntity, new()
    {
        ServiceResult<bool> Add(params TEntity[] entities);

        ServiceResult<bool> Update(params TEntity[] entities);

        ServiceResult<bool> Delete(params Guid[] entityIds);

        ServiceResult<TEntity> GetById(EntityFilter<TEntity> setting);

        ServiceResult<IList<TEntity>> GetAll(EntityFilter<TEntity> setting);

        Task<ServiceResult<IList<TEntity>>> GetAllAsync(EntityFilter<TEntity> setting);

        ServiceResult<bool> AddList(List<TEntity> entityList);

        ServiceResult<bool> UpdateList(List<TEntity> entityList);

        ServiceResult<bool> DeleteList(List<TEntity> entityList);
    }
}
