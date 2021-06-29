using AspCore.Business.Specifications.Abstract;
using AspCore.Dependency.Abstract;
using AspCore.Dtos.Dto;
using AspCore.Entities.EntityFilter;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspCore.Business.Abstract
{
    public interface IEntityService<TEntity, TEntityDto, in TCreatedDto, in TUpdatedDto> : ITransientType
       where TEntity : class, IEntity, new()
       where TUpdatedDto : class, IEntityDto, new()
       where TCreatedDto : class, IEntityDto, new()
       where TEntityDto : class, IEntityDto, new()
    {
        ServiceResult<bool> Add(params TCreatedDto[] entities);

        ServiceResult<bool> Update(params TUpdatedDto[] entities);

        ServiceResult<bool> Delete(params Guid[] entityIds);

        Task<ServiceResult<bool>> AddAsync(params TCreatedDto[] entities);

        Task<ServiceResult<bool>> UpdateAsync(params TUpdatedDto[] entities);
        Task<ServiceResult<bool>> UpdateAsync(TUpdatedDto entity);
  

        Task<ServiceResult<bool>> DeleteAsync(params Guid[] entityIds);

        ServiceResult<TEntityDto> GetById(EntityFilter setting);

        Task<ServiceResult<TEntityDto>> GetByIdAsync(EntityFilter setting);

        ServiceResult<IList<TEntityDto>> GetAll(EntityFilter setting);

        Task<ServiceResult<IList<TEntityDto>>> GetAllAsync(EntityFilter setting);

        Task<ServiceResult<List<TEntityDto>>> GetHistoriesByIdAsync(EntityFilter setting);
        Task<ServiceResult<IList<TEntityDto>>> GetAllAsync(ISpecification<TEntity> specification);
        Task<ServiceResult<IList<TEntityDto>>> GetAllAsync();
    }

    public interface IEntityService<TEntity, TEntityDto> : IEntityService<TEntity, TEntityDto, TEntityDto, TEntityDto>
        where TEntity : class, IEntity, new()
        where TEntityDto : class, IEntityDto, new()
    {

    }
}
