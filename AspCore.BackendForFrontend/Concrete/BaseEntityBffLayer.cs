using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspCore.BackendForFrontend.Abstract;
using AspCore.Entities.Constants;
using AspCore.Entities.EntityFilter;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Extension;

namespace AspCore.BackendForFrontend.Concrete
{
    public abstract class BaseEntityBffLayer<TViewModel, TEntity> : BaseBffLayer, IEntityBffLayer<TViewModel, TEntity>
        where TViewModel : BaseViewModel<TEntity>, new()
        where TEntity : class, IEntity, new()
    {
        protected BaseEntityBffLayer(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            apiControllerRoute = "api/" + typeof(TEntity).Name;
        }

        public async Task<ServiceResult<bool>> Liveness()
        {
            ApiClient.apiUrl = apiControllerRoute + "/" + ApiConstants.Urls.LIVENESS;

            var result = await ApiClient.PostRequest<ServiceResult<bool>>(string.Empty);
            return result;
        }
        public async Task<ServiceResult<bool>> Readiness(Guid id)
        {
            ApiClient.apiUrl = apiControllerRoute + "/" + ApiConstants.Urls.READINESS;
            var result = await ApiClient.PostRequest<ServiceResult<bool>>(id);
            return result;
        }
        public async Task<ServiceResult<bool>> Add(List<TViewModel> entities)
        {
            ApiClient.apiUrl = apiControllerRoute + "/" + ApiConstants.Urls.ADDAsync;
            var listEntities = entities.Select(t => t.dataEntity);
            var result = await ApiClient.PostRequest<ServiceResult<bool>>(listEntities);
            return result;
        }
        public async Task<ServiceResult<bool>> Update(List<TViewModel> entities)
        {
            ApiClient.apiUrl = apiControllerRoute + "/" + ApiConstants.Urls.UPDATEAsync;
            var listEntities = entities.Select(t => t.dataEntity);
            var result = await ApiClient.PostRequest<ServiceResult<bool>>(listEntities);
            return result;
        }
        public async Task<ServiceResult<bool>> Delete(List<TViewModel> entities)
        {
            ApiClient.apiUrl = apiControllerRoute + "/" + ApiConstants.Urls.DELETE;
            var listEntities = entities.Select(t => t.dataEntity);
            var result = await ApiClient.PostRequest<ServiceResult<bool>>(listEntities);
            return result;
        }
        public async Task<ServiceResult<bool>> DeleteWithIDs(List<Guid> entityIds)
        {
            ApiClient.apiUrl = apiControllerRoute + "/" + ApiConstants.Urls.DELETEAsync;
            var result = await ApiClient.PostRequest<ServiceResult<bool>>(entityIds.ToArray());
            return result;
        }
        public async Task<ServiceResult<List<TViewModel>>> GetAll(EntityFilter<TEntity> entityFilter)
        {
            ApiClient.apiUrl = apiControllerRoute + "/" + ApiConstants.Urls.GET_ALL;
            var viewResult = new ServiceResult<List<TViewModel>>();
            var result = await ApiClient.PostRequest<ServiceResult<List<TEntity>>>(entityFilter);
            viewResult.ToViewModelResult(result);
            return viewResult;
        }
        public async Task<ServiceResult<List<TViewModel>>> GetAllAsync(EntityFilter<TEntity> filterSetting)
        {
            ApiClient.apiUrl = apiControllerRoute + "/" + ApiConstants.Urls.GET_ALL_ASYNC;
            var viewResult = new ServiceResult<List<TViewModel>>();
            var result = await ApiClient.PostRequest<ServiceResult<List<TEntity>>>(filterSetting);
            viewResult.ToViewModelResult(result);

            return viewResult;
        }
        public async Task<ServiceResult<TViewModel>> GetById(EntityFilter<TEntity> filterSetting)
        {
            ApiClient.apiUrl = apiControllerRoute + "/" + ApiConstants.Urls.GET_BY_ID;
            var viewResult = new ServiceResult<TViewModel>();
            var result = await ApiClient.PostRequest<ServiceResult<TEntity>>(filterSetting);
            viewResult.ToViewModelResult(result);
            return viewResult;
        }

        public async Task<ServiceResult<List<TViewModel>>> GetEntityHistoriesAsync(EntityFilter<TEntity> filterSetting)
        {
            ApiClient.apiUrl = apiControllerRoute + "/" + ApiConstants.Urls.GET_ENTITY_HISTORIES_ASYNC;
            var viewResult = new ServiceResult<List<TViewModel>>();
            var result = await ApiClient.PostRequest<ServiceResult<List<TEntity>>>(filterSetting);
            viewResult.ToViewModelResult(result);

            return viewResult;
        }
    }
}
