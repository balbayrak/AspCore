using AspCore.Entities.Constants;
using AspCore.Entities.DataTable;
using AspCore.Entities.DocumentType;
using AspCore.Entities.EntityFilter;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Utilities.DataProtector;
using AspCore.Web.Abstract;
using AspCore.Web.Filters;
using AspCore.WebComponents.HtmlHelpers.DataTable.ModelBinder;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace AspCore.Web.Concrete
{
    public abstract class BaseWebEntityController<TEntity, TViewModel, TBffLayer> : BaseWebController<Document, DocumentRequest>
        where TEntity : class, IEntity, new()
        where TViewModel : BaseViewModel<TEntity>, new()
        where TBffLayer : IDatatableEntityBffLayer<TViewModel, TEntity>
    {
        protected TBffLayer BffLayer { get; private set; }
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
        public IActionResult AddOrEdit(string id)
        {
            TViewModel viewModel = null;
            if (!string.IsNullOrEmpty(id))
            {
                if (id != "-1")
                {
                    ServiceResult<TViewModel> entityResult = BffLayer.GetById(new EntityFilter<TEntity>
                    {
                        id = new Guid(id)
                    }).Result;

                    if (entityResult.IsSucceededAndDataIncluded())
                    {
                        viewModel = entityResult.Result;
                        return PartialView("AddOrEdit", viewModel);
                    }
                }
                else
                {
                    return PartialView("AddOrEdit");
                }
            }

            return BadRequest(string.Format(FrontEndConstants.ERROR_MESSAGES.PARAMETER_IS_NULL, nameof(id)));
        }

        [HttpPost]
        public string AddOrEdit(TViewModel viewModel)
        {
            if (viewModel != null)
            {
                AjaxResult result = new AjaxResult();

                ServiceResult<bool> addorUpdateResult = new ServiceResult<bool>();
                if (!string.IsNullOrEmpty(viewModel.dataEntity.EncryptedId))
                {
                    viewModel.dataEntity.Id = new Guid(DataProtectorFactory.Instance.UnProtect(viewModel.dataEntity.EncryptedId));
                    addorUpdateResult = BffLayer.Update(new List<TViewModel> { viewModel }).Result;
                }
                else
                {
                    addorUpdateResult = BffLayer.Add(new List<TViewModel> { viewModel }).Result;
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
                ServiceResult<bool> serviceResult = BffLayer.DeleteWithIDs(new List<Guid> { new Guid(id) }).Result;
            }
        }
    }
}
