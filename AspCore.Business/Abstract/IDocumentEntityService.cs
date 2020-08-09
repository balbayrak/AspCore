
using AspCore.Dtos.Dto;
using AspCore.Entities.DocumentType;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;

namespace AspCore.Business.Abstract
{
    public interface IDocumentEntityService<TDocument, TEntity,TEntityDto, in TCreatedDto, in TUpdatedDto> : IEntityService<TEntity, TEntityDto, TCreatedDto,TUpdatedDto>
        where TDocument : class, IDocument, new()
        where TEntity : class, IDocumentEntity, new()
        where TEntityDto : class, IEntityDto, new()
        where TCreatedDto : class, IEntityDto, new()
        where TUpdatedDto : class, IEntityDto, new()
    {
        ServiceResult<TDocument> CreateDocument(IDocumentEntityRequest<TDocument, TEntity> documentRequest);
        ServiceResult<TDocument> ReadDocument(IDocumentRequest<TDocument> documentRequest);
        ServiceResult<bool> UpdateDocument(IDocumentEntityRequest<TDocument, TEntity> documentRequest);
        ServiceResult<bool> DeleteDocument(IDocumentEntityRequest<TDocument, TEntity> documentRequest);
    }

    public interface IDocumentEntityService<TDocument, TEntity, TEntityDto> : IDocumentEntityService<TDocument, TEntity, TEntityDto,TEntityDto,TEntityDto>
        where TDocument : class, IDocument, new()
        where TEntity : class, IDocumentEntity, new()
        where TEntityDto : class, IEntityDto, new()
    {

    }
}
