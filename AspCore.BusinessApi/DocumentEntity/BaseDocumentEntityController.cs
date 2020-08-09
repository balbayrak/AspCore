using AspCore.Business.Abstract;
using AspCore.Business.General;
using AspCore.Entities.Constants;
using AspCore.Entities.DocumentType;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Extension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using AspCore.Dtos.Dto;

namespace AspCore.BusinessApi.DocumentEntity
{
    public class BaseDocumentEntityController<TEntityService, TEntity, TEntityDto ,TCreatedDto,TUpdatedDto,TDocument, TDocumentRequest, TDocumentEntityRequest> : BaseEntityController<TEntity, TEntityDto, TCreatedDto,TUpdatedDto, TEntityService>
        where TEntityService : IDocumentEntityService<TDocument, TEntity, TEntityDto,TCreatedDto,TUpdatedDto>
        where TEntity : class, IDocumentEntity, new()
        where TDocument : class, IDocument, new()
        where TDocumentEntityRequest : class, IDocumentEntityRequest<TDocument, TEntity>, new()
        where TDocumentRequest : class, IDocumentRequest<TDocument>, new()
        where TEntityDto : class, IEntityDto, new()
        where TCreatedDto : class, IEntityDto, new()
        where TUpdatedDto : class, IEntityDto, new()
    {

        public BaseDocumentEntityController(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }

        [ActionName(ApiConstants.Urls.ADDDOCUMENT)]
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Authorize()]
        public IActionResult AddDocument(TDocumentEntityRequest documentEntityRequest)
        {
            if (documentEntityRequest == null)
            {
                return base.BadRequest(string.Format(BusinessConstants.BaseExceptionMessages.PARAMETER_IS_NULL, nameof(documentEntityRequest)));
            }

            if (!ModelState.IsValid)
            {
                return base.BadRequest(BusinessConstants.BaseExceptionMessages.MODEL_INVALID);
            }

            ServiceResult<TDocument> response = Service.CreateDocument(documentEntityRequest);
            return response.ToHttpResponse();
        }

        [ActionName(ApiConstants.Urls.UPDATEDOCUMENT)]
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(typeof(ServiceResult<bool>), 200)]
        [ProducesResponseType(500)]
        [Authorize()]
        public IActionResult UpdateDocument(TDocumentEntityRequest documentEntityRequest)
        {
            if (documentEntityRequest == null)
            {
                return base.BadRequest(string.Format(BusinessConstants.BaseExceptionMessages.PARAMETER_IS_NULL, nameof(documentEntityRequest)));
            }

            if (!ModelState.IsValid)
            {
                return base.BadRequest(BusinessConstants.BaseExceptionMessages.MODEL_INVALID);
            }

            ServiceResult<bool> response = Service.UpdateDocument(documentEntityRequest);
            return response.ToHttpResponse();
        }


        [ActionName(ApiConstants.Urls.DELETEDOCUMENT)]
        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(typeof(ServiceResult<bool>), 200)]
        [ProducesResponseType(500)]
        [Authorize()]
        public IActionResult DeleteDocument(TDocumentEntityRequest documentEntityRequest)
        {
            if (documentEntityRequest == null)
            {
                return base.BadRequest(string.Format(BusinessConstants.BaseExceptionMessages.PARAMETER_IS_NULL, nameof(documentEntityRequest)));
            }

            if (!ModelState.IsValid)
            {
                return base.BadRequest(BusinessConstants.BaseExceptionMessages.MODEL_INVALID);
            }

            ServiceResult<bool> response = Service.DeleteDocument(documentEntityRequest);
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

            ServiceResult<TDocument> response = Service.ReadDocument(documentRequest);
            return response.ToHttpResponse();
        }
    }

    public class BaseDocumentEntityController<TEntityService, TEntity, TEntityDto, TDocument, TDocumentRequest,
        TDocumentEntityRequest> :
        BaseDocumentEntityController<TEntityService, TEntity, TEntityDto, TEntityDto, TEntityDto, TDocument,
            TDocumentRequest, TDocumentEntityRequest>
        where TEntityService : IDocumentEntityService<TDocument, TEntity, TEntityDto>
        where TEntity : class, IDocumentEntity, new()
        where TDocument : class, IDocument, new()
        where TDocumentEntityRequest : class, IDocumentEntityRequest<TDocument, TEntity>, new()
        where TDocumentRequest : class, IDocumentRequest<TDocument>, new()
        where TEntityDto : class, IEntityDto, new()
    {
        public BaseDocumentEntityController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}
