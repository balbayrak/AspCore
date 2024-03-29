﻿using AspCore.Business.Abstract;
using AspCore.Business.General;
using AspCore.DataAccess.Abstract;
using AspCore.DocumentManagement.Uploader;
using AspCore.Entities.DocumentType;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Extension;
using Microsoft.Extensions.DependencyInjection;
using System;
using AspCore.Dtos.Dto;

namespace AspCore.Business.Concrete
{
    public abstract class DocumentEntityManager<TEntity,TEntityDto,TCreatedDto,TUpdatedDto, TDataAccess> : BaseEntityManager<TDataAccess, TEntity,TEntityDto,TCreatedDto,TUpdatedDto>, IDocumentEntityService<Document, TEntity, TEntityDto,TCreatedDto,TUpdatedDto>
        where TEntity : class, IDocumentEntity, new()
        where TDataAccess : IEntityRepository<TEntity>
        where TEntityDto : class, IEntityDto, new()
        where TUpdatedDto : class, IEntityDto, new()
        where TCreatedDto : class, IEntityDto, new()
    {
        private IDocumentUploader<Document> _documentUploader { get; set; }

        public DocumentEntityManager(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _documentUploader = ServiceProvider.GetRequiredService<IDocumentUploader<Document>>();
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

    public abstract class   DocumentEntityManager<TEntity, TEntityDto, TDataAccess> : DocumentEntityManager<TEntity, TEntityDto, TEntityDto,
            TEntityDto, TDataAccess> where TEntity : class, IDocumentEntity, new() where TEntityDto : class, IEntityDto, new() where TDataAccess : IEntityRepository<TEntity>
    {
        protected DocumentEntityManager(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}
