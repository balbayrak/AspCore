using AspCore.Dependency.Abstract;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Entities.Search;
using System;
using System.Collections.Generic;
using AspCore.Dtos.Dto;
using AspCore.Dtos.Search;

namespace AspCore.DataSearch.Abstract
{
    public interface IDataSearchEngine<TSearchableEntity> : ITransientType
        where TSearchableEntity : class, ISearchableEntity, new()
    {
        ServiceResult<bool> Create(params TSearchableEntity[] searchableEntities);
        ServiceResult<bool> Update(params TSearchableEntity[] searchableEntities);
        ServiceResult<bool> Delete(params TSearchableEntity[] searchableEntities);
        ServiceResult<DataSearchResult<TSearchableEntity>> FindBy(bool isActiveOnly, int startIndex, int takeCount);
        ServiceResult<DataSearchResult<TSearchableEntity>> FindById(Guid Id, bool isActive);
        ServiceResult<DataSearchResult<TSearchableEntity>> FindByIdList(List<Guid> idList, bool isActive);

    }
}
