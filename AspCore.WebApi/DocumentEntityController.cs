using AspCore.Business.Abstract;
using AspCore.Entities.DocumentType;
using AspCore.Entities.EntityType;

namespace AspCore.WebApi
{
    public class DocumentEntityController<TEntityService, TEntity> : BaseDocumentEntityController<TEntityService, TEntity, Document, DocumentRequest, DocumentEntityRequest<TEntity>>
        where TEntityService : IDocumentEntityService<Document, TEntity>
        where TEntity : class, IDocumentEntity, new()
    {
    }
}
