using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspCore.ApiClient.Entities.Concrete;
using AspCore.Business.Abstract;
using AspCore.Business.General;
using AspCore.Dependency.Concrete;
using AspCore.Entities.Constants;
using AspCore.Entities.EntityFilter;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Extension;

namespace AspCore.WebApi
{
    public class BaseEntityController<TEntityService, TEntity> : BaseController
        where TEntityService : IEntityService<TEntity>
        where TEntity : class, IEntity, new()
    {
        protected readonly TEntityService _service;
        public BaseEntityController()
        {
            _service = DependencyResolver.Current.GetService<TEntityService>();
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
            ServiceResult<TEntity> readResponse = _service.GetById(new EntityFilter<TEntity>
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

            ServiceResult<bool> response = _service.Add(entities);
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

            ServiceResult<bool> response = _service.Update(entities);
            return response.ToHttpResponse();
        }


        [ActionName(ApiConstants.Urls.DELETE)]
        [HttpPost]
        [ProducesResponseType(typeof(ServiceResult<bool>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize()]
        public IActionResult Delete([FromBody]TEntity[] entities)
        {
            if (entities == null)
            {
                return base.BadRequest(string.Format(BusinessConstants.BaseExceptionMessages.PARAMETER_IS_NULL, nameof(entities)));
            }

            ServiceResult<bool> response = _service.Delete(entities);
            return response.ToHttpResponse();

        }

        [ActionName(ApiConstants.Urls.DELETE_WITH_IDs)]
        [HttpPost]
        [ProducesResponseType(typeof(ServiceResult<bool>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize()]
        public IActionResult Delete([FromBody]Guid[] entities)
        {
            if (entities == null)
            {
                return base.BadRequest(string.Format(BusinessConstants.BaseExceptionMessages.PARAMETER_IS_NULL, nameof(entities)));
            }

            ServiceResult<bool> response = _service.Delete(entities);
            return response.ToHttpResponse();

        }


        [ActionName(ApiConstants.Urls.GET_ALL)]
        [HttpPost]
        [ProducesResponseType(500)]
        [Authorize()]
        public IActionResult GetAll(EntityFilter<TEntity> entityFilter)
        {
            ServiceResult<IList<TEntity>> response = _service.GetAll(entityFilter);
            return response.ToHttpResponse();
        }

        [ActionName(ApiConstants.Urls.GET_ALL_ASYNC)]
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize()]
        public async Task<IActionResult> GetAllAsync(EntityFilter<TEntity> filterSetting)
        {
            ServiceResult<IList<TEntity>> response = await _service.GetAllAsync(filterSetting);
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
            ServiceResult<TEntity> response = _service.GetById(filterSetting);
            return response.ToHttpResponse();
        }
    }
}
