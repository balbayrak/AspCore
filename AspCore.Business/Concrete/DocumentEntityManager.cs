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

namespace AspCore.Business.Concrete
{
    public abstract class DocumentEntityManager<TEntity, TDataAccess> : BaseEntityManager<TDataAccess, TEntity>, IDocumentEntityService<Document, TEntity>
        where TEntity : class, IDocumentEntity, new()
        where TDataAccess : IEntityRepository<TEntity>
    {
        private IDocumentUploader<Document> _documentUploader { get; set; }

        public DocumentEntityManager()
        {
            _documentUploader = DependencyResolver.Current.GetService<IDocumentUploader<Document>>();
        }

        public ServiceResult<Document> CreateDocument(IDocumentEntityRequest<Document, TEntity> documentRequest)
        {
            ServiceResult<Document> result = new ServiceResult<Document>();
            try
            {
                result = _documentUploader.Create(documentRequest);
                if (result.IsSucceededAndDataIncluded())
                {
                    documentRequest.entity.DocumentUrl = result.Result.url;
                    ServiceResult<bool> dataAccessResult = _dataAccess.Add(documentRequest.entity);
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

        public ServiceResult<bool> DeleteDocument(IDocumentEntityRequest<Document, TEntity> documentRequest)
        {
            ServiceResult<bool> result = new ServiceResult<bool>();
            try
            {
                result = _documentUploader.Delete(documentRequest);
                if (result.IsSucceeded)
                {
                    if (documentRequest.deleteEntityWithDocument)
                    {
                        result = _dataAccess.Delete(documentRequest.entity);
                    }
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage(BusinessConstants.DocumentUploaderErrorMessages.DOCUMENT_DELETE_METHOD_ERROR, ex);
            }
            return result;
        }

        public ServiceResult<Document> ReadDocument(IDocumentRequest<Document> documentRequest)
        {
            ServiceResult<Document> result = new ServiceResult<Document>();
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

        public ServiceResult<bool> UpdateDocument(IDocumentEntityRequest<Document, TEntity> documentRequest)
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
