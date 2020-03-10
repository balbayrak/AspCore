using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using AspCore.BackendForFrontend.Abstract;
using AspCore.Dependency.Concrete;
using AspCore.Entities.Constants;
using AspCore.Entities.DataTable;
using AspCore.Entities.DocumentType;
using AspCore.Entities.EntityFilter;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Web.Abstract;
using AspCore.Web.Filters;
using AspCore.WebComponents.HtmlHelpers.DataTable.ModelBinder;

namespace AspCore.Web.Concrete
{
    public class BaseWebEntityController<TEntity, TViewModel, TBffLayer> : BaseWebController<Document, DocumentRequest>
        where TEntity : class, IEntity, new()
        where TViewModel : BaseViewModel<TEntity>, new()
        where TBffLayer : IDatatableEntityBffLayer<TViewModel, TEntity>
    {
        protected TBffLayer BffLayer { get; private set; }
        public BaseWebEntityController()
        {
            BffLayer = DependencyResolver.Current.GetService<TBffLayer>();
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

            return BadRequest(string.Format(FrontEndConstants.ERROR_MESSAGES.PARAMETER_IS_NULL, nameof(id)));
        }

        [HttpPost]
        public string AddOrEdit(TViewModel viewModel)
        {
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
