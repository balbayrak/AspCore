using AspCore.Business.Abstract;
using AspCore.Business.General;
using AspCore.ElasticSearchApiClient.QueryBuilder.Concrete;
using AspCore.Entities.Constants;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Extension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspCore.BusinessApi
{
    public class BaseSearchableEntityController<TEntity, TEntityService> : BaseEntityController<TEntity, TEntityService>
        where TEntityService : ISearchableEntityService<TEntity, TEntity>
        where TEntity : class, ISearchableEntity, new()
    {

        [ActionName(ApiConstants.DataSearchApi_Urls.RESET_INDEX_ACTION_NAME)]
        [HttpPost]
        [ProducesResponseType(typeof(ServiceResult<bool>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize()]
        public IActionResult ResetIndex(InitIndexRequest initRequest)
        {
            if (initRequest == null)
            {
                return base.BadRequest(string.Format(BusinessConstants.BaseExceptionMessages.PARAMETER_IS_NULL, nameof(initRequest)));
            }

            if (!ModelState.IsValid)
            {
                return base.BadRequest(BusinessConstants.BaseExceptionMessages.MODEL_INVALID);
            }

            ServiceResult<bool> response = _service.ResetSearchableData(initRequest.initializeWithData);
            return response.ToHttpResponse();
        }

    }
}
