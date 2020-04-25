using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AspCore.Business.General;
using AspCore.Dependency.Concrete;
using AspCore.DocumentManagement.Uploader;
using AspCore.Entities.Constants;
using AspCore.Entities.DocumentType;
using AspCore.Entities.General;
using AspCore.Extension;
using AspCore.WebApi;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace AspCore.BusinessApi.DocumentEntity
{
    public abstract class BaseDocumentUploaderController<TDocument, TDocumentRequest> : BaseController
        where TDocument : class, IDocument, new()
        where TDocumentRequest : class, IDocumentRequest<TDocument>, new()
    {
        private IDocumentUploader<TDocument> _documentUploader;
        protected IServiceProvider ServiceProvider { get; private set; }
        public BaseDocumentUploaderController(IServiceProvider serviceProvider) : base()
        {
            ServiceProvider = serviceProvider;
            _documentUploader = ServiceProvider.GetRequiredService<IDocumentUploader<TDocument>>();
        }

        [ActionName(ApiConstants.Urls.ADDDOCUMENT)]
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize()]
        public IActionResult Create(TDocumentRequest documentRequest)
        {
            if (documentRequest == null)
            {
                return base.BadRequest(string.Format(BusinessConstants.BaseExceptionMessages.PARAMETER_IS_NULL, nameof(documentRequest)));
            }

            if (!ModelState.IsValid)
            {
                return base.BadRequest(BusinessConstants.BaseExceptionMessages.MODEL_INVALID);
            }

            ServiceResult<TDocument> response = _documentUploader.Create(documentRequest);
            return response.ToHttpResponse();
        }

        [ActionName(ApiConstants.Urls.UPDATEDOCUMENT)]
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(typeof(ServiceResult<bool>), 200)]
        [ProducesResponseType(500)]
        [Authorize()]
        public IActionResult UpdateDocument(TDocumentRequest documentRequest)
        {
            if (documentRequest == null)
            {
                return base.BadRequest(string.Format(BusinessConstants.BaseExceptionMessages.PARAMETER_IS_NULL, nameof(documentRequest)));
            }

            if (!ModelState.IsValid)
            {
                return base.BadRequest(BusinessConstants.BaseExceptionMessages.MODEL_INVALID);
            }

            ServiceResult<bool> response = _documentUploader.Update(documentRequest);
            return response.ToHttpResponse();
        }


        [ActionName(ApiConstants.Urls.DELETEDOCUMENT)]
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(typeof(ServiceResult<bool>), 200)]
        [ProducesResponseType(500)]
        [Authorize()]
        public IActionResult DeleteDocument(TDocumentRequest documentRequest)
        {
            if (documentRequest == null)
            {
                return base.BadRequest(string.Format(BusinessConstants.BaseExceptionMessages.PARAMETER_IS_NULL, nameof(documentRequest)));
            }

            if (!ModelState.IsValid)
            {
                return base.BadRequest(BusinessConstants.BaseExceptionMessages.MODEL_INVALID);
            }

            ServiceResult<bool> response = _documentUploader.Delete(documentRequest);
            return response.ToHttpResponse();
        }


        [ActionName(ApiConstants.Urls.READDOCUMENT)]
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize()]
        public IActionResult ReadDocument(TDocumentRequest documentRequest)
        {
            if (documentRequest == null)
            {
                return base.BadRequest(string.Format(BusinessConstants.BaseExceptionMessages.PARAMETER_IS_NULL, nameof(documentRequest)));
            }

            if (!ModelState.IsValid)
            {
                return base.BadRequest(BusinessConstants.BaseExceptionMessages.MODEL_INVALID);
            }

            ServiceResult<TDocument> response = _documentUploader.Read(documentRequest);
            return response.ToHttpResponse();
        }
    }
}
