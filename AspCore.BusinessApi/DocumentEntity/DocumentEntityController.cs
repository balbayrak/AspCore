using AspCore.Business.Abstract;
using AspCore.Dtos.Dto;
using AspCore.Entities.DocumentType;
using AspCore.Entities.EntityType;
using System;

namespace AspCore.BusinessApi.DocumentEntity
{
    public class DocumentEntityController<TEntityService, TEntity,TEntityDto,TCreatedDto,TUpdatedDto> : BaseDocumentEntityController<TEntityService, TEntity, TEntityDto, Document, DocumentRequest, DocumentEntityRequest<TEntity>>
        where TEntityService : IDocumentEntityService<Document, TEntity, TEntityDto, TCreatedDto,TUpdatedDto>, IDocumentEntityService<Document, TEntity, TEntityDto>
        where TEntity : class, IDocumentEntity, new()
        where TEntityDto : class, IEntityDto, new()
        where TUpdatedDto : class, IEntityDto, new()
        where TCreatedDto : class, IEntityDto, new()
    {
        public DocumentEntityController(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }
    }
}
