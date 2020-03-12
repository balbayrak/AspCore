using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using AspCore.Entities.Constants;
using AspCore.Entities.EntityFilter;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Web.Abstract;
using AspCore.Web.Filters;

namespace AspCore.Web.Concrete
{
    public abstract class BaseWebDocumentEntityController<TEntity, TViewModel, TBffLayer> : BaseWebEntityController<TEntity, TViewModel, TBffLayer>
        where TEntity : class, IDocumentEntity, new()
        where TViewModel : BaseViewModel<TEntity>, new()
        where TBffLayer : IDatatableEntityBffLayer<TViewModel, TEntity>
    {
        [HttpGet]
        [DataUnProtector("id")]
        public IActionResult DownloadDocumentEntity(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                ServiceResult<TViewModel> entityResult = BffLayer.GetById(new EntityFilter<TEntity>
                {
                    id = new Guid(id)
                }).Result;

                if (entityResult.IsSucceededAndDataIncluded())
                {
                   return DownloadDocument(entityResult.Result.dataEntity.DocumentUrl);
                }
            }
            return BadRequest(string.Format(FrontEndConstants.ERROR_MESSAGES.PARAMETER_IS_NULL, nameof(id)));
        }
    }
}
