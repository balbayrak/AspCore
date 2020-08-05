using System;
using System.Threading.Tasks;
using AspCore.BackendForFrontend.Abstract;
using AspCore.Dtos.Dto;
using AspCore.Entities.Constants;
using AspCore.Entities.DocumentType;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Mapper.Abstract;

namespace AspCore.BackendForFrontend.Concrete
{
    public abstract class BaseDocumentEntityBffLayer<TEntityDto, TEntity, TDocument> : BaseEntityBffLayer<TEntityDto>, IDocumentEntityBffLayer<TEntityDto, TEntity, TDocument>
        
        where TEntity : class, IDocumentEntity, new()
        where TDocument : class, IDocument, new()
        where TEntityDto : class,IEntityDto,new()
    {
        public BaseDocumentEntityBffLayer(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }

        public async Task<ServiceResult<TDocument>> AddDocument(IDocumentEntityRequest<TDocument, TEntity> documentEntityRequest)
        {
            ApiClient.apiUrl = apiControllerRoute + "/" + ApiConstants.Urls.ADDDOCUMENT;
            var result = await ApiClient.PostRequest<ServiceResult<TDocument>>(documentEntityRequest);
            return result;
        }

        public async Task<ServiceResult<bool>> DeleteDocument(IDocumentEntityRequest<TDocument, TEntity> documentEntityRequest)
        {
            ApiClient.apiUrl = apiControllerRoute + "/" + ApiConstants.Urls.DELETEDOCUMENT;
            var result = await ApiClient.PostRequest<ServiceResult<bool>>(documentEntityRequest);
            return result;
        }

        public async Task<ServiceResult<TDocument>> ReadDocument(IDocumentRequest<TDocument> documentEntityRequest)
        {
            ApiClient.apiUrl = apiControllerRoute + "/" + ApiConstants.Urls.READDOCUMENT;
            var result = await ApiClient.PostRequest<ServiceResult<TDocument>>(documentEntityRequest);
            return result;
        }

        public async Task<ServiceResult<bool>> UpdateDocument(IDocumentEntityRequest<TDocument, TEntity> documentEntityRequest)
        {
            ApiClient.apiUrl = apiControllerRoute + "/" + ApiConstants.Urls.UPDATEDOCUMENT;
            var result = await ApiClient.PostRequest<ServiceResult<bool>>(documentEntityRequest);
            return result;
        }
    }
}
