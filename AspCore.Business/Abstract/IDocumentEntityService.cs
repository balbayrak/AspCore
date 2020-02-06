using AspCore.Entities.DocumentType;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;

namespace AspCore.Business.Abstract
{
    public interface IDocumentEntityService<TDocument, TEntity> : IEntityService<TEntity>
        where TDocument : class, IDocument, new()
        where TEntity : class, IDocumentEntity, new()
    {
        ServiceResult<TDocument> CreateDocument(IDocumentEntityRequest<TDocument, TEntity> documentRequest);
        ServiceResult<TDocument> ReadDocument(IDocumentRequest<TDocument> documentRequest);
        ServiceResult<bool> UpdateDocument(IDocumentEntityRequest<TDocument, TEntity> documentRequest);
        ServiceResult<bool> DeleteDocument(IDocumentEntityRequest<TDocument, TEntity> documentRequest);
    }
}
