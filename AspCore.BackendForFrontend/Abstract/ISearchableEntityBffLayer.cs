using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using System;
using System.Collections.Generic;
using System.Text;
using AspCore.Dtos.Dto;
using System.Threading.Tasks;

namespace AspCore.BackendForFrontend.Abstract
{
    public interface ISearchableEntityBffLayer<TSearchableEntity, TCreatedDto, TUpdatedDto> : IEntityBffLayer<TSearchableEntity, TCreatedDto, TUpdatedDto>
         where TSearchableEntity : class, IEntityDto, new()
         where TCreatedDto : class, IEntityDto, new()
         where TUpdatedDto : class, IEntityDto, new()

    {
        Task<ServiceResult<List<TSearchableEntity>>> FindBy(bool isActiveOnly, int startIndex, int takeCount);

        Task<ServiceResult<TSearchableEntity>> FindById(Guid Id, bool isActive);

        Task<ServiceResult<List<TSearchableEntity>>> FindByIdList(List<Guid> idList, bool isActive);
    }

    public interface ISearchableEntityBffLayer<TSearchableEntity> : ISearchableEntityBffLayer<TSearchableEntity, TSearchableEntity,
            TSearchableEntity>
        where TSearchableEntity : class, ISearchableEntityDto, new()

    {

    }
}
