using AspCore.Dtos.Dto;
using AspCore.Entities.Constants;
using AspCore.Entities.DataTable;
using AspCore.Entities.DocumentType;
using AspCore.Entities.EntityFilter;
using AspCore.Entities.General;
using AspCore.Utilities.DataProtector;
using AspCore.Web.Abstract;
using AspCore.Web.Filters;
using AspCore.WebComponents.HtmlHelpers.DataTable.ModelBinder;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspCore.Web.Concrete
{
    public abstract class BaseWebEntityController<TEntityDto, TCreatedDto,TUpdatedDto,TBffLayer> : BaseWebController<Document, DocumentRequest>
        where TEntityDto : class,IEntityDto,new()
        where TCreatedDto : class,IEntityDto,new()
        where TUpdatedDto : class,IEntityDto,new()
        where TBffLayer : IDatatableEntityBffLayer<TEntityDto,TCreatedDto,TUpdatedDto>
    {
        protected TBffLayer BffLayer { get; }
        public BaseWebEntityController(IServiceProvider serviceProvider, TBffLayer bffLayer) : base(serviceProvider)
        {
            BffLayer = bffLayer;
        }

        [HttpPost]
        public JsonResult LoadTable([JQueryDataTablesModelBinder] JQueryDataTablesModel jQueryDataTablesModel)
        {
            JQueryDataTablesResponse response = BffLayer.GetAll(jQueryDataTablesModel);

            if (response != null)
            {
                return Json(response);
            }
            else
            {
                return Json(string.Empty);
            }
        }

        [HttpGet]
        [DataUnProtector("id")]
        public async Task<IActionResult> AddOrEdit(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                if (id != "-1")
                {
                    ServiceResult<TEntityDto> entityResult = await BffLayer.GetByIdAsync(new EntityFilter
                    {
                        id = new Guid(id)
                    });

                    if (entityResult.IsSucceededAndDataIncluded())
                    {
                        var viewModel = entityResult.Result;
                        return PartialView("AddOrEdit", viewModel);
                    }
                }
                else
                {
                    return PartialView("AddOrEdit", (TEntityDto) null);
                }
            }

            return BadRequest(string.Format(FrontEndConstants.ERROR_MESSAGES.PARAMETER_IS_NULL, nameof(id)));
        }

        [HttpPost]
        public async Task<string> Add(TCreatedDto createdDto)
        {
            if (createdDto != null)
            {
                AjaxResult result = new AjaxResult();

                ServiceResult<bool> addorUpdateResult = new ServiceResult<bool>();
                if (string.IsNullOrEmpty(createdDto.EncryptedId))
                {
                    addorUpdateResult = await BffLayer.AddAsync(new List<TCreatedDto> { createdDto });
                }
                if (addorUpdateResult.IsSucceeded)
                {
                    result.Result = AjaxResultTypeEnum.Succeed;
                    result.ResultText = addorUpdateResult.StatusMessage;
                }
                else
                {
                    result.Result = AjaxResultTypeEnum.Error;
                    result.ResultText = addorUpdateResult.ErrorMessage;
                }

                return JsonConvert.SerializeObject(result);
            }
            return null;
        }

        [HttpPost]
        public async Task<string> Edit(TUpdatedDto updatedDto)
        {
            if (updatedDto != null)
            {
                AjaxResult result = new AjaxResult();

                ServiceResult<bool> addorUpdateResult = new ServiceResult<bool>();
                if (!string.IsNullOrEmpty(updatedDto.EncryptedId))
                {
                    updatedDto.Id = new Guid(DataProtectorFactory.Instance.UnProtect(updatedDto.EncryptedId));
                    addorUpdateResult =await BffLayer.UpdateAsync(new List<TUpdatedDto> { updatedDto });
                }
                if (addorUpdateResult.IsSucceeded)
                {
                    result.Result = AjaxResultTypeEnum.Succeed;
                    result.ResultText = addorUpdateResult.StatusMessage;
                }
                else
                {
                    result.Result = AjaxResultTypeEnum.Error;
                    result.ResultText = addorUpdateResult.ErrorMessage;
                }

                return JsonConvert.SerializeObject(result);
            }
            return null;
        }

        [HttpPost]
        [DataUnProtector("id")]
        public void Delete(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                BffLayer.DeleteWithIDsAsync(new List<Guid> { new Guid(id) });
            }
        }
    }


    public abstract class BaseWebEntityController<TEntityDto, TBffLayer> : BaseWebEntityController<TEntityDto, TEntityDto, TEntityDto, TBffLayer>
        where TEntityDto : class, IEntityDto, new()
        where TBffLayer : IDatatableEntityBffLayer<TEntityDto>

    {
        protected BaseWebEntityController(IServiceProvider serviceProvider, TBffLayer bffLayer) : base(serviceProvider, bffLayer)
        {
        }
    }
}
