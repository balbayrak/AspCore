using AspCore.BackendForFrontend.Abstract;
using AspCore.Dtos.Dto;
using AspCore.Entities.Constants;
using AspCore.Entities.EntityFilter;
using AspCore.Entities.General;
using AspCore.Extension;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspCore.BackendForFrontend.Concrete
{
    public abstract class BaseEntityBffLayer<TEntityDto,TCreatedDto,TUpdatedDto> : BaseBffLayer, IEntityBffLayer<TEntityDto,TCreatedDto,TUpdatedDto>
        where TEntityDto : class, IEntityDto, new()
        where TCreatedDto : class, IEntityDto, new()
        where TUpdatedDto : class, IEntityDto, new()
    {
        protected BaseEntityBffLayer(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            apiControllerRoute = "api/" + typeof(TEntityDto).Name.Replace("Dto","");
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
        public async Task<ServiceResult<bool>> AddAsync(List<TCreatedDto> entities)
        {
            ApiClient.apiUrl = apiControllerRoute + "/" + ApiConstants.Urls.ADDAsync;
            var result = await ApiClient.PostRequest<ServiceResult<bool>>(entities);
            return result;
        }
        public async Task<ServiceResult<bool>> UpdateAsync(List<TUpdatedDto> entities)
        {
            ApiClient.apiUrl = apiControllerRoute + "/" + ApiConstants.Urls.UPDATEAsync;
            var result = await ApiClient.PostRequest<ServiceResult<bool>>(entities);
            return result;
        }
        public async Task<ServiceResult<bool>> DeleteAsync(List<TEntityDto> entities)
        {
            ApiClient.apiUrl = apiControllerRoute + "/" + ApiConstants.Urls.DELETE;
            var result = await ApiClient.PostRequest<ServiceResult<bool>>(entities);
            return result;
        }
        public async Task<ServiceResult<bool>> DeleteWithIDsAsync(List<Guid> entityIds)
        {
            ApiClient.apiUrl = apiControllerRoute + "/" + ApiConstants.Urls.DELETEAsync;
            var result = await ApiClient.PostRequest<ServiceResult<bool>>(entityIds.ToArray());
            return result;
        }
        public async Task<ServiceResult<List<TEntityDto>>> GetAll(EntityFilter entityFilter)
        {
            ApiClient.apiUrl = apiControllerRoute + "/" + ApiConstants.Urls.GET_ALL;
            var result = await ApiClient.PostRequest<ServiceResult<List<TEntityDto>>>(entityFilter);
            return result.ProtectEntity();
        }
        public async Task<ServiceResult<List<TEntityDto>>> GetAllAsync(EntityFilter filterSetting)
        {
            ApiClient.apiUrl = apiControllerRoute + "/" + ApiConstants.Urls.GET_ALL_ASYNC;
            var result = await ApiClient.PostRequest<ServiceResult<List<TEntityDto>>>(filterSetting);
            return result.ProtectEntity();
        }
        public async Task<ServiceResult<TEntityDto>> GetByIdAsync(EntityFilter filterSetting)
        {
            ApiClient.apiUrl = apiControllerRoute + "/" + ApiConstants.Urls.GET_BY_IDAsync;
            var result = await ApiClient.PostRequest<ServiceResult<TEntityDto>>(filterSetting);
            return result.ProtectEntity();
        }
        public async Task<ServiceResult<List<TEntityDto>>> GetEntityHistoriesAsync(EntityFilter filterSetting)
        {
            ApiClient.apiUrl = apiControllerRoute + "/" + ApiConstants.Urls.GET_ENTITY_HISTORIES_ASYNC;
            var result = await ApiClient.PostRequest<ServiceResult<List<TEntityDto>>>(filterSetting);
            return result.ProtectEntity();
        }
    }

    public abstract class BaseEntityBffLayer<TEntityDto> : BaseEntityBffLayer<TEntityDto, TEntityDto, TEntityDto>
        where TEntityDto : class, IEntityDto, new()

    {
        protected BaseEntityBffLayer(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}
