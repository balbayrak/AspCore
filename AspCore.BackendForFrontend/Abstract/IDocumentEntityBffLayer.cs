using System.Threading.Tasks;
using AspCore.Dtos.Dto;
using AspCore.Entities.DocumentType;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Mapper.Abstract;

namespace AspCore.BackendForFrontend.Abstract
{
    public interface IDocumentEntityBffLayer<TEntityDto, TEntity, TDocument> : IEntityBffLayer<TEntityDto>
         where TEntity : class, IDocumentEntity, new()
         where TDocument : class, IDocument, new()
         where TEntityDto : class, IEntityDto,new()
    {
        Task<ServiceResult<TDocument>> AddDocument(IDocumentEntityRequest<TDocument, TEntity> documentEntityRequest);

        Task<ServiceResult<bool>> UpdateDocument(IDocumentEntityRequest<TDocument, TEntity> documentEntityRequest);

        Task<ServiceResult<bool>> DeleteDocument(IDocumentEntityRequest<TDocument, TEntity> documentEntityRequest);

        Task<ServiceResult<TDocument>> ReadDocument(IDocumentRequest<TDocument> documentEntityRequest);
    }
}
