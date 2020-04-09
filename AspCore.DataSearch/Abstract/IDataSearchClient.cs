using AspCore.Dependency.Abstract;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Entities.Search;
using System;
using System.Collections.Generic;

namespace AspCore.DataSearch.Abstract
{
    public interface IDataSearchClient<TSearchableEntity> : ITransientType
         where TSearchableEntity : class, ISearchableEntity, new()
    {
        ServiceResult<DataSearchResult<TSearchableEntity>> FindBy(bool isActiveOnly, int startIndex, int takeCount);
        ServiceResult<DataSearchResult<TSearchableEntity>> FindById(Guid Id, bool isActive);
        ServiceResult<DataSearchResult<TSearchableEntity>> FindByIdList(List<Guid> idList, bool isActive);
    }
}
