using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AspCore.Business.General;
using AspCore.Dependency.Concrete;
using AspCore.DocumentManagement.Viewer;
using AspCore.Entities.Constants;
using AspCore.Entities.DocumentType;
using AspCore.Entities.General;
using AspCore.Extension;

namespace AspCore.WebApi
{
    public class BaseDocumentViewerController<TDocument, TViewRequest> : BaseController
        where TDocument : class, IDocument, new()
        where TViewRequest : IDocumentApiViewRequest<TDocument, ViewerToolbarSetting>
    {
        private IDocumentViewer<TDocument> _service;
        public BaseDocumentViewerController() : base()
        {
            _service = DependencyResolver.Current.GetService<IDocumentViewer<TDocument>>();
        }


        [ActionName(ApiConstants.Urls.VIEWDOCUMENTS)]
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [ProducesResponseType(typeof(ServiceResult<string>), 200)]
        [Authorize()]
        public IActionResult ViewDocuments(TViewRequest viewRequest)
        {
            if (viewRequest == null)
            {
                return base.BadRequest(string.Format(BusinessConstants.BaseExceptionMessages.PARAMETER_IS_NULL, nameof(viewRequest)));
            }

            if (!ModelState.IsValid)
            {
                return base.BadRequest(BusinessConstants.BaseExceptionMessages.MODEL_INVALID);
            }

            ServiceResult<string> response = _service.ViewDocuments(viewRequest);
            return response.ToHttpResponse();
        }

    }
}
