using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspCore.Business.Specifications.Abstract;
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

        Task<ServiceResult<bool>> AddAsync(params TEntity[] entities);

        Task<ServiceResult<bool>> UpdateAsync(params TEntity[] entities);

        Task<ServiceResult<bool>> DeleteAsync(params Guid[] entityIds);

        ServiceResult<TEntity> GetById(EntityFilter<TEntity> setting);

        Task<ServiceResult<TEntity>> GetByIdAsync(EntityFilter<TEntity> setting);

        ServiceResult<IList<TEntity>> GetAll(EntityFilter<TEntity> setting);

        Task<ServiceResult<IList<TEntity>>> GetAllAsync(EntityFilter<TEntity> setting);

        Task<ServiceResult<List<TEntity>>> GetHistoriesByIdAsync(EntityFilter<TEntity> setting);

        Task<ServiceResult<IList<TEntity>>> GetAllAsync(ISpecification<TEntity> specification);

    }
}
