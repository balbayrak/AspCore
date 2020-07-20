using AspCore.Business.Abstract;
using AspCore.Business.General;
using AspCore.Entities.Constants;
using AspCore.Entities.EntityFilter;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Extension;
using AspCore.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspCore.BusinessApi
{
    public class BaseEntityController<TEntity, TEntityService> : BaseController
        where TEntityService : IEntityService<TEntity>
        where TEntity : class, IEntity, new()
    {
        protected IServiceProvider ServiceProvider { get; }
        protected readonly object ServiceProviderLock = new object();

        protected TService LazyGetRequiredService<TService>(ref TService reference)
            => LazyGetRequiredService(typeof(TService), ref reference);

        protected TRef LazyGetRequiredService<TRef>(Type serviceType, ref TRef reference)
        {
            if (reference == null)
            {
                lock (ServiceProviderLock)
                {
                    if (reference == null)
                    {
                        reference = (TRef)ServiceProvider.GetRequiredService(serviceType);
                    }
                }
            }

            return reference;
        }

        protected  TEntityService Service => LazyGetRequiredService(ref _service);
        private TEntityService _service;
        public BaseEntityController(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        [ActionName(ApiConstants.Urls.LIVENESS)]
        [HttpGet]
        [ProducesResponseType(typeof(ServiceResult<bool>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [AllowAnonymous]

        public IActionResult Liveness()
        {
            ServiceResult<bool> response = new ServiceResult<bool>();

            response.IsSucceeded = true;
            response.Result = true;
            return response.ToHttpResponse();
        }

        [ActionName(ApiConstants.Urls.READINESS)]
        [HttpGet]
        [ProducesResponseType(typeof(ServiceResult<bool>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [AllowAnonymous]
        public IActionResult Readiness(Guid id)
        {
            if (id == Guid.Empty)
            {
                return base.BadRequest(string.Format(BusinessConstants.BaseExceptionMessages.PARAMETER_IS_GUID_EMPTY, nameof(id)));
            }
            ServiceResult<TEntity> readResponse = Service.GetById(new EntityFilter<TEntity>
            {
                id = id,
            });

            ServiceResult<bool> response = new ServiceResult<bool>();
            response.IsSucceeded = false;
            response.Result = false;

            if (readResponse.IsSucceededAndDataIncluded())
            {
                response.IsSucceeded = true;
                response.Result = true;
            }
            else
            {
                response.ErrorMessage = readResponse.ErrorMessage;
                response.ExceptionMessage = readResponse.ExceptionMessage;
            }

            return response.ToHttpResponse();
        }


        [ActionName(ApiConstants.Urls.ADD)]
        [HttpPost]
        [ProducesResponseType(typeof(ServiceResult<bool>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize()]
        public IActionResult Add([FromBody]TEntity[] entities)
        {
            if (entities == null)
            {
                return base.BadRequest(string.Format(BusinessConstants.BaseExceptionMessages.PARAMETER_IS_NULL, nameof(entities)));
            }

            if (!ModelState.IsValid)
            {
                return base.BadRequest(BusinessConstants.BaseExceptionMessages.MODEL_INVALID);
            }

            ServiceResult<bool> response = Service.Add(entities);
            return response.ToHttpResponse();
        }

        [ActionName(ApiConstants.Urls.ADDAsync)]
        [HttpPost]
        [ProducesResponseType(typeof(ServiceResult<bool>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize()]
        public async Task<IActionResult> AddAsync([FromBody] TEntity[] entities)
        {
            if (entities == null)
            {
                return base.BadRequest(string.Format(BusinessConstants.BaseExceptionMessages.PARAMETER_IS_NULL, nameof(entities)));
            }

            if (!ModelState.IsValid)
            {
                return base.BadRequest(BusinessConstants.BaseExceptionMessages.MODEL_INVALID);
            }

            ServiceResult<bool> response = await Service.AddAsync(entities);
            return response.ToHttpResponse();
        }


        [ActionName(ApiConstants.Urls.UPDATE)]
        [HttpPost]
        [ProducesResponseType(typeof(ServiceResult<bool>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize()]
        public IActionResult Update([FromBody]TEntity[] entities)
        {
            if (entities == null)
            {
                return base.BadRequest(string.Format(BusinessConstants.BaseExceptionMessages.PARAMETER_IS_NULL, nameof(entities)));
            }

            if (!ModelState.IsValid)
            {
                return base.BadRequest(BusinessConstants.BaseExceptionMessages.MODEL_INVALID);
            }

            ServiceResult<bool> response = Service.Update(entities);
            return response.ToHttpResponse();
        }
        [ActionName(ApiConstants.Urls.UPDATEAsync)]
        [HttpPost]
        [ProducesResponseType(typeof(ServiceResult<bool>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize()]
        public async Task<IActionResult> UpdateAsync([FromBody] TEntity[] entities)
        {
            if (entities == null)
            {
                return base.BadRequest(string.Format(BusinessConstants.BaseExceptionMessages.PARAMETER_IS_NULL, nameof(entities)));
            }

            if (!ModelState.IsValid)
            {
                return base.BadRequest(BusinessConstants.BaseExceptionMessages.MODEL_INVALID);
            }

            ServiceResult<bool> response =await Service.UpdateAsync(entities);
            return response.ToHttpResponse();
        }


        [ActionName(ApiConstants.Urls.DELETE_WITH_IDs)]
        [HttpPost]
        [ProducesResponseType(typeof(ServiceResult<bool>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize()]
        public IActionResult Delete([FromBody] Guid[] entities)
        {
            if (entities == null)
            {
                return base.BadRequest(string.Format(BusinessConstants.BaseExceptionMessages.PARAMETER_IS_NULL, nameof(entities)));
            }

            ServiceResult<bool> response = Service.Delete(entities);
            return response.ToHttpResponse();

        }

        [ActionName(ApiConstants.Urls.DELETEAsync)]
        [HttpPost]
        [ProducesResponseType(typeof(ServiceResult<bool>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize()]
        public async Task<IActionResult> DeleteAsync([FromBody] Guid[] entities)
        {
            if (entities == null)
            {
                return base.BadRequest(string.Format(BusinessConstants.BaseExceptionMessages.PARAMETER_IS_NULL, nameof(entities)));
            }

            ServiceResult<bool> response = await Service.DeleteAsync(entities);
            return response.ToHttpResponse();

        }

        [ActionName(ApiConstants.Urls.GET_ALL)]
        [HttpPost]
        [ProducesResponseType(500)]
        [Authorize()]
        public IActionResult GetAll(EntityFilter<TEntity> entityFilter)
        {
            ServiceResult<IList<TEntity>> response = Service.GetAll(entityFilter);
            return response.ToHttpResponse();
        }

        [ActionName(ApiConstants.Urls.GET_ALL_ASYNC)]
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize()]
        public async Task<IActionResult> GetAllAsync(EntityFilter<TEntity> filterSetting)
        {
            ServiceResult<IList<TEntity>> response = await Service.GetAllAsync(filterSetting);
            return response.ToHttpResponse();
        }

        [ActionName(ApiConstants.Urls.GET_BY_ID)]
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize()]
        public IActionResult GetById(EntityFilter<TEntity> filterSetting)
        {
            if (filterSetting.id == Guid.Empty)
            {
                return base.BadRequest(string.Format(BusinessConstants.BaseExceptionMessages.PARAMETER_IS_GUID_EMPTY, nameof(filterSetting.id)));
            }
            ServiceResult<TEntity> response = Service.GetById(filterSetting);
            return response.ToHttpResponse();
        }

        [ActionName(ApiConstants.Urls.GET_ENTITY_HISTORIES_ASYNC)]
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize()]
        public async Task<IActionResult> GetEntityHistoriesAsync(EntityFilter<TEntity> filterSetting)
        {
            ServiceResult<List<TEntity>> response = await Service.GetHistoriesByIdAsync(filterSetting);
            return response.ToHttpResponse();
        }
    }
}
