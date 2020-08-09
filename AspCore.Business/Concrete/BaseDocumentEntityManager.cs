using AspCore.Business.Abstract;
using AspCore.Business.General;
using AspCore.DataAccess.Abstract;
using AspCore.DocumentManagement.Uploader;
using AspCore.Dtos.Dto;
using AspCore.Entities.DocumentType;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Extension;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AspCore.Business.Concrete
{
    public abstract class BaseDocumentEntityManager<TDataAccess,TDocument, TEntity,TEntityDto,TCreatedDto,TUpdatedDto> : BaseEntityManager<TDataAccess, TEntity, TEntityDto, TCreatedDto,TUpdatedDto>, IDocumentEntityService<TDocument, TEntity, TEntityDto, TCreatedDto,TUpdatedDto>
        where TDocument : class, IDocument, new()
        where TEntity : class, IDocumentEntity, new()
        where TDataAccess : IEntityRepository<TEntity>
        where TEntityDto : class, IEntityDto, new()
        where TCreatedDto : class, IEntityDto, new()
        where TUpdatedDto : class, IEntityDto, new()
    {
        private IDocumentUploader<TDocument> DocumentUploader { get; }

        protected BaseDocumentEntityManager(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            DocumentUploader = ServiceProvider.GetRequiredService<IDocumentUploader<TDocument>>();
        }

        public ServiceResult<TDocument> CreateDocument(IDocumentEntityRequest<TDocument, TEntity> documentRequest)
        {
            ServiceResult<TDocument> result = new ServiceResult<TDocument>();
            try
            {
                result = DocumentUploader.Create(documentRequest);
                if (result.IsSucceededAndDataIncluded())
                {
                    documentRequest.entity.DocumentUrl = result.Result.url;
                    ServiceResult<bool> dataAccessResult = DataAccess.Add(documentRequest.entity);
                    if (!dataAccessResult.IsSucceeded)
                    {
                        result.IsSucceeded = false;
                        result.ErrorMessage = dataAccessResult.ErrorMessage;
                        result.ExceptionMessage = dataAccessResult.ExceptionMessage;
                    }
                    else
                    {
                        documentRequest.document.url = result.Result.url;
                        ServiceResult<bool> deleteResult = DocumentUploader.Delete(documentRequest);
                        if (!deleteResult.IsSucceeded)
                        {
                            result.WarningMessage = BusinessConstants.DocumentUploaderErrorMessages.DOCUMENT_DELETE_AFTER_DATAACCESS_METHOD_ERROR;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage(BusinessConstants.DocumentUploaderErrorMessages.DOCUMENT_CREATE_METHOD_ERROR, ex);
            }
            return result;
        }

        public ServiceResult<bool> DeleteDocument(IDocumentEntityRequest<TDocument, TEntity> documentRequest)
        {
            ServiceResult<bool> result = new ServiceResult<bool>();
            try
            {
                result = DocumentUploader.Delete(documentRequest);
                if (result.IsSucceeded)
                {
                    if (documentRequest.deleteEntityWithDocument)
                    {
                        result = DataAccess.Delete(documentRequest.entity);
                    }
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage(BusinessConstants.DocumentUploaderErrorMessages.DOCUMENT_DELETE_METHOD_ERROR, ex);
            }
            return result;
        }

        public ServiceResult<TDocument> ReadDocument(IDocumentRequest<TDocument> documentRequest)
        {
            ServiceResult<TDocument> result = new ServiceResult<TDocument>();
            try
            {
                result = DocumentUploader.Read(documentRequest);
            }
            catch (Exception ex)
            {
                result.ErrorMessage(BusinessConstants.DocumentUploaderErrorMessages.DOCUMENT_READ_METHOD_ERROR, ex);
            }
            return result;
        }

        public ServiceResult<bool> UpdateDocument(IDocumentEntityRequest<TDocument, TEntity> documentRequest)
        {
            ServiceResult<bool> result = new ServiceResult<bool>();
            try
            {
                result = DocumentUploader.Update(documentRequest);
            }
            catch (Exception ex)
            {
                result.ErrorMessage(BusinessConstants.DocumentUploaderErrorMessages.DOCUMENT_UPDATE_METHOD_ERROR, ex);
            }
            return result;
        }
    }

    public abstract class BaseDocumentEntityManager<TDataAccess, TDocument, TEntity,  TEntityDto> :
        BaseDocumentEntityManager<TDataAccess,TDocument, TEntity, TEntityDto, TEntityDto, TEntityDto>
        where TDocument : class, IDocument, new()
        where TEntity : class, IDocumentEntity, new()
        where TDataAccess : IEntityRepository<TEntity>
        where TEntityDto : class, IEntityDto, new()
       
    {
        protected BaseDocumentEntityManager(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}
