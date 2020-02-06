using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using AspCore.Business.Abstract;
using AspCore.Business.General;
using AspCore.Business.Task.Concrete;
using AspCore.Dependency.Concrete;
using AspCore.Entities.General;
using AspCore.Entities.User;
using AspCore.Extension;

namespace AspCore.WebApi
{
    public class BaseTaskController<TActiveUser, TEntity, TEntityService> : BaseController
        where TEntityService : ITaskService<TActiveUser, TEntity>
        where TEntity : class, new()
        where TActiveUser : class, IActiveUser, new()
    {
        protected readonly TEntityService _service;
        public BaseTaskController()
        {
            _service = DependencyResolver.Current.GetService<TEntityService>();
        }

        [ActionName("Liveness")]
        [HttpGet]
        [ProducesResponseType(typeof(ServiceResult<bool>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult Liveness()
        {
            ServiceResult<bool> response = new ServiceResult<bool>();
            response.IsSucceeded = true;
            return response.ToHttpResponse();
        }

        [NonAction]
        protected virtual ServiceResult<bool> ReadinessInternal(Guid id)
        {
            ServiceResult<bool> result = new ServiceResult<bool>();
            result.IsSucceeded = false;
            result.Result = false;
            return result;
        }

        [ActionName("Readiness")]
        [HttpGet]
        [ProducesResponseType(typeof(ServiceResult<bool>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult Readiness(Guid id)
        {
            if (id == Guid.Empty)
            {
                return base.BadRequest(string.Format(BusinessConstants.BaseExceptionMessages.PARAMETER_IS_GUID_EMPTY, nameof(id)));
            }
            ServiceResult<bool> response = ReadinessInternal(id);

            return response.ToHttpResponse();
        }

        [ActionName("Create")]
        [HttpPost]
        [ProducesResponseType(typeof(ServiceResult<bool>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult Create([FromBody] TEntity[] entities)
        {
            if (entities == null)
            {
                return base.BadRequest(string.Format(BusinessConstants.BaseExceptionMessages.PARAMETER_IS_NULL, nameof(entities)));
            }

            if (!ModelState.IsValid)
            {
                return base.BadRequest(BusinessConstants.BaseExceptionMessages.MODEL_INVALID);
            }

            TaskEntity<TEntity>[] tasks = entities.Select(t => new TaskEntity<TEntity>
            {
                entity = t
            }).ToArray();
            ServiceResult<bool> response = _service.RunAction(tasks);
            return response.ToHttpResponse();
        }

        [ActionName("Update")]
        [HttpPost]
        [ProducesResponseType(typeof(ServiceResult<bool>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
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

            TaskEntity<TEntity>[] tasks = entities.Select(t => new TaskEntity<TEntity>
            {
                entity = t
            }).ToArray();

            ServiceResult<bool> response = _service.RunAction(tasks);
            return response.ToHttpResponse();
        }


        [ActionName("Delete")]
        [HttpPost]
        [ProducesResponseType(typeof(ServiceResult<bool>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult Delete([FromBody]TEntity[] entities)
        {
            if (entities == null)
            {
                return base.BadRequest(string.Format(BusinessConstants.BaseExceptionMessages.PARAMETER_IS_NULL, nameof(entities)));
            }

            if (!ModelState.IsValid)
            {
                return base.BadRequest(BusinessConstants.BaseExceptionMessages.MODEL_INVALID);
            }

            TaskEntity<TEntity>[] tasks = entities.Select(t => new TaskEntity<TEntity>
            {
                entity = t
            }).ToArray();

            ServiceResult<bool> response = _service.RunAction(tasks);
            return response.ToHttpResponse();

        }

    }
}
