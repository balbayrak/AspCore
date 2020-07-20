using System;
using AspCore.Business.Abstract;
using AspCore.Business.General;
using AspCore.DataAccess.Abstract;
using AspCore.Dependency.Concrete;
using AspCore.DocumentManagement.Uploader;
using AspCore.Entities.DocumentType;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Extension;
using Microsoft.Extensions.DependencyInjection;

namespace AspCore.Business.Concrete
{
    public abstract class BaseDocumentEntityManager<TDocument, TEntity, TDataAccess> : BaseEntityManager<TDataAccess, TEntity>, IDocumentEntityService<TDocument, TEntity>
        where TDocument : class, IDocument, new()
        where TEntity : class, IDocumentEntity, new()
        where TDataAccess : IEntityRepository<TEntity>
    {
        private IDocumentUploader<TDocument> _documentUploader { get; set; }

        protected BaseDocumentEntityManager(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _documentUploader = ServiceProvider.GetRequiredService<IDocumentUploader<TDocument>>();
        }

        public ServiceResult<TDocument> CreateDocument(IDocumentEntityRequest<TDocument, TEntity> documentRequest)
        {
            ServiceResult<TDocument> result = new ServiceResult<TDocument>();
            try
            {
                result = _documentUploader.Create(documentRequest);
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
                        ServiceResult<bool> deleteResult = _documentUploader.Delete(documentRequest);
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
                result = _documentUploader.Delete(documentRequest);
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
                result = _documentUploader.Read(documentRequest);
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
                result = _documentUploader.Update(documentRequest);
            }
            catch (Exception ex)
            {
                result.ErrorMessage(BusinessConstants.DocumentUploaderErrorMessages.DOCUMENT_UPDATE_METHOD_ERROR, ex);
            }
            return result;
        }
    }
}
