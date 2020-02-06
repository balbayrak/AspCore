﻿using System.Threading.Tasks;
using AspCore.BackendForFrontend.Abstract;
using AspCore.Entities.Constants;
using AspCore.Entities.DocumentType;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;

namespace AspCore.BackendForFrontend.Concrete
{
    public abstract class BaseDocumentEntityBffLayer<TViewModel, TEntity, TDocument> : BaseEntityBffLayer<TViewModel, TEntity>, IDocumentEntityBffLayer<TViewModel, TEntity, TDocument>
        where TViewModel : BaseViewModel<TEntity>, new()
        where TEntity : class, IDocumentEntity, new()
        where TDocument : class, IDocument, new()
    {
        public BaseDocumentEntityBffLayer() : base()
        {

        }

        public async Task<ServiceResult<TDocument>> AddDocument(IDocumentEntityRequest<TDocument, TEntity> documentEntityRequest)
        {
            apiClient.apiUrl = apiControllerRoute + "/" + ApiConstants.Urls.ADDDOCUMENT;
            var result = await apiClient.PostRequest<ServiceResult<TDocument>>(documentEntityRequest);
            return result;
        }

        public async Task<ServiceResult<bool>> DeleteDocument(IDocumentEntityRequest<TDocument, TEntity> documentEntityRequest)
        {
            apiClient.apiUrl = apiControllerRoute + "/" + ApiConstants.Urls.DELETEDOCUMENT;
            var result = await apiClient.PostRequest<ServiceResult<bool>>(documentEntityRequest);
            return result;
        }

        public async Task<ServiceResult<TDocument>> ReadDocument(IDocumentRequest<TDocument> documentEntityRequest)
        {
            apiClient.apiUrl = apiControllerRoute + "/" + ApiConstants.Urls.READDOCUMENT;
            var result = await apiClient.PostRequest<ServiceResult<TDocument>>(documentEntityRequest);
            return result;
        }

        public async Task<ServiceResult<bool>> UpdateDocument(IDocumentEntityRequest<TDocument, TEntity> documentEntityRequest)
        {
            apiClient.apiUrl = apiControllerRoute + "/" + ApiConstants.Urls.UPDATEDOCUMENT;
            var result = await apiClient.PostRequest<ServiceResult<bool>>(documentEntityRequest);
            return result;
        }
    }
}
