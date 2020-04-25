using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AspCore.Business.General;
using AspCore.Dependency.Concrete;
using AspCore.DocumentManagement.Viewer;
using AspCore.Entities.Constants;
using AspCore.Entities.DocumentType;
using AspCore.Entities.General;
using AspCore.Extension;
using AspCore.WebApi;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace AspCore.BusinessApi.DocumentEntity
{
    public class BaseDocumentViewerController<TDocument, TViewRequest> : BaseController
        where TDocument : class, IDocument, new()
        where TViewRequest : IDocumentApiViewRequest<TDocument, ViewerToolbarSetting>
    {
        private readonly IDocumentViewer<TDocument> _documentViewer;
        protected IServiceProvider ServiceProvider { get; private set; }
        public BaseDocumentViewerController(IServiceProvider serviceProvider) : base()
        {
            ServiceProvider = serviceProvider;
            _documentViewer = ServiceProvider.GetRequiredService<IDocumentViewer<TDocument>>();
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

            ServiceResult<string> response = _documentViewer.ViewDocuments(viewRequest);
            return response.ToHttpResponse();
        }

    }
}
