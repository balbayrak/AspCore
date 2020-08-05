using AspCore.Entities.Constants;
using AspCore.Entities.EntityFilter;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Web.Abstract;
using AspCore.Web.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using AspCore.Dtos.Dto;

namespace AspCore.Web.Concrete
{
    public abstract class BaseWebDocumentEntityController<TEntityDto, TBffLayer> : BaseWebEntityController<TEntityDto, TBffLayer>
        where TBffLayer : IDatatableEntityBffLayer<TEntityDto>
        where TEntityDto : class,IDocumentEntityDto,new()
    {
        public BaseWebDocumentEntityController(IServiceProvider serviceProvider, TBffLayer bffLayer) : base(serviceProvider, bffLayer)
        {

        }
        [HttpGet]
        [DataUnProtector("id")]
        public IActionResult DownloadDocumentEntity(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                ServiceResult<TEntityDto> entityResult = BffLayer.GetById(new EntityFilter
                {
                    id = new Guid(id)
                }).Result;

                if (entityResult.IsSucceededAndDataIncluded())
                {
                    return DownloadDocument(entityResult.Result.DocumentUrl);
                }
            }
            return BadRequest(string.Format(FrontEndConstants.ERROR_MESSAGES.PARAMETER_IS_NULL, nameof(id)));
        }
    }
}
