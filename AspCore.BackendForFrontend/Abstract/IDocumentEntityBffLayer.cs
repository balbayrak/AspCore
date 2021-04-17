using System.Threading.Tasks;
using AspCore.Dtos.Dto;
using AspCore.Entities.DocumentType;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Mapper.Abstract;

namespace AspCore.BackendForFrontend.Abstract
{
    public interface IDocumentEntityBffLayer<TEntity, TDocument,TEntityDto,TCreateDto,TUpdateDto> : IEntityBffLayer<TEntityDto,TCreateDto,TUpdateDto>
         where TEntity : class, IDocumentEntity, new()
         where TDocument : class, IDocument, new()
         where TEntityDto : class, IEntityDto,new()
         where TCreateDto : class, IEntityDto,new()
         where TUpdateDto : class, IEntityDto,new()
    {
        Task<ServiceResult<TDocument>> AddDocument(IDocumentEntityRequest<TDocument, TEntity> documentEntityRequest);

        Task<ServiceResult<bool>> UpdateDocument(IDocumentEntityRequest<TDocument, TEntity> documentEntityRequest);

        Task<ServiceResult<bool>> DeleteDocument(IDocumentEntityRequest<TDocument, TEntity> documentEntityRequest);

        Task<ServiceResult<TDocument>> ReadDocument(IDocumentRequest<TDocument> documentEntityRequest);
    }

    public interface IDocumentEntityBffLayer<TEntity, TDocument, TEntityDto> : IDocumentEntityBffLayer<TEntity, TDocument, TEntityDto
            , TEntityDto, TEntityDto>
        where TEntity : class, IDocumentEntity, new()
        where TDocument : class, IDocument, new()
        where TEntityDto : class, IEntityDto, new()
    {

    }
}
