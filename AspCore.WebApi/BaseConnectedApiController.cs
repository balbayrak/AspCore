using Microsoft.AspNetCore.Mvc;
using System;
using AspCore.Business.Abstract;
using AspCore.Business.General;
using AspCore.Entities.General;
using AspCore.Extension;

namespace AspCore.WebApi
{
    public class BaseConnectedApiController<T> : BaseController
        where T : IConnectedApiService
    {
        protected T _service;
        public BaseConnectedApiController(T service)
        {
            _service = service;
        }


        [NonAction]
        protected virtual ServiceResult<bool> ReadinessInternal(Guid id)
        {
            ServiceResult<bool> result = new ServiceResult<bool>();
            result.IsSucceeded = false;
            result.Result = false;
            return result;
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
    }
}
