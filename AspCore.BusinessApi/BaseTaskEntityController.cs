using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspCore.Business.Abstract;
using AspCore.Business.General;
using AspCore.Entities.EntityFilter;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Entities.User;
using AspCore.Extension;

namespace AspCore.BusinessApi
{
    public class BaseTaskEntityController<TActiveUser, TEntity, TEntityService> : BaseTaskController<TActiveUser, TEntity, TEntityService>
        where TEntityService : ITaskEntityService<TActiveUser, TEntity>
        where TEntity : class, IEntity, new()
        where TActiveUser : class, IActiveUser, new()
    {
        public BaseTaskEntityController(TEntityService entityService) : base(entityService)
        {
        }

        protected override ServiceResult<bool> ReadinessInternal(Guid id)
        {
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

            return response;
        }

        [ActionName("GetAll")]
        [HttpPost]
        [ProducesResponseType(500)]
        public IActionResult GetAll(EntityFilter<TEntity> entityFilter)
        {
            ServiceResult<IList<TEntity>> response = _service.GetAll(entityFilter);
            return response.ToHttpResponse();
        }

        [ActionName("GetAllAsync")]
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> GetAllAsync(EntityFilter<TEntity> filterSetting)
        {
            ServiceResult<IList<TEntity>> response = await _service.GetAllAsync(filterSetting);
            return response.ToHttpResponse();
        }

        [ActionName("GetById")]
        [HttpPost]

        [ProducesResponseType(500)]
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
