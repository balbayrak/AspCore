using System.Threading.Tasks;
using AspCore.Entities.DocumentType;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;

namespace AspCore.BackendForFrontend.Abstract
{
    public interface IDocumentEntityBffLayer<TViewModel, TEntity, TDocument> : IEntityBffLayer<TViewModel, TEntity>
         where TViewModel : BaseViewModel<TEntity>, new()
         where TEntity : class, IDocumentEntity, new()
         where TDocument : class, IDocument, new()
    {
        Task<ServiceResult<TDocument>> AddDocument(IDocumentEntityRequest<TDocument, TEntity> documentEntityRequest);

        Task<ServiceResult<bool>> UpdateDocument(IDocumentEntityRequest<TDocument, TEntity> documentEntityRequest);

        Task<ServiceResult<bool>> DeleteDocument(IDocumentEntityRequest<TDocument, TEntity> documentEntityRequest);

        Task<ServiceResult<TDocument>> ReadDocument(IDocumentRequest<TDocument> documentEntityRequest);
    }
}
