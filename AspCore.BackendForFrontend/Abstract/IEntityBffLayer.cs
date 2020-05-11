using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspCore.Entities.DataTable;
using AspCore.Entities.EntityFilter;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;

namespace AspCore.BackendForFrontend.Abstract
{
    public interface IEntityBffLayer<TViewModel, TEntity> : IBffLayer
         where TViewModel : BaseViewModel<TEntity> 
         where TEntity : class, IEntity, new()
    {
        Task<ServiceResult<bool>> Liveness();
        Task<ServiceResult<bool>> Readiness(Guid id);
        Task<ServiceResult<bool>> Add(List<TViewModel> entities);
        Task<ServiceResult<bool>> Update(List<TViewModel> entities);
        Task<ServiceResult<bool>> Delete(List<TViewModel> entities);
        Task<ServiceResult<bool>> DeleteWithIDs(List<Guid> entityIds);
        Task<ServiceResult<List<TViewModel>>> GetAll(EntityFilter<TEntity> entityFilter);
        Task<ServiceResult<List<TViewModel>>> GetAllAsync(EntityFilter<TEntity> filterSetting);
        Task<ServiceResult<TViewModel>> GetById(EntityFilter<TEntity> filterSetting);



    }
}
