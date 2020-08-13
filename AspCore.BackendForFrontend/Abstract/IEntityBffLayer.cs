using AspCore.Dtos.Dto;
using AspCore.Entities.EntityFilter;
using AspCore.Entities.General;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspCore.BackendForFrontend.Abstract
{
    public interface IEntityBffLayer<TEntityDto,TCreatedDto,TUpdatedDto> : IBffLayer
         where TEntityDto : class, IEntityDto, new()
         where TCreatedDto : class, IEntityDto, new()
         where TUpdatedDto : class, IEntityDto, new()
    {
        Task<ServiceResult<bool>> Liveness();
        Task<ServiceResult<bool>> Readiness(Guid id);
        Task<ServiceResult<bool>> AddAsync(List<TCreatedDto> entities);
        Task<ServiceResult<bool>> UpdateAsync(List<TUpdatedDto> entities);
        Task<ServiceResult<bool>> DeleteAsync(List<TEntityDto> entities);
        Task<ServiceResult<bool>> DeleteWithIDsAsync(List<Guid> entityIds);
        Task<ServiceResult<List<TEntityDto>>> GetAll(EntityFilter entityFilter);
        Task<ServiceResult<List<TEntityDto>>> GetAllAsync(EntityFilter filterSetting);
        Task<ServiceResult<TEntityDto>> GetByIdAsync(EntityFilter filterSetting);
        Task<ServiceResult<List<TEntityDto>>> GetEntityHistoriesAsync(EntityFilter filterSetting);

    }

    public interface IEntityBffLayer<TEntityDto> : IEntityBffLayer<TEntityDto, TEntityDto, TEntityDto>
        where TEntityDto : class, IEntityDto, new()
    {

    }
}
