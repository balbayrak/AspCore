using AspCore.Dependency.Abstract;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Entities.Search;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspCore.DataSearch.Abstract
{
    public interface IDataSearchEngine<TSearchableEntity> : ITransientType
        where TSearchableEntity : class, ISearchableEntity, new()
    {
        Task<ServiceResult<bool>> CreateAsync(params TSearchableEntity[] searchableEntities);
        Task<ServiceResult<bool>> UpdateAsync(params TSearchableEntity[] searchableEntities);
        Task<ServiceResult<bool>> DeleteAsync(params TSearchableEntity[] searchableEntities);
        Task<ServiceResult<DataSearchResult<TSearchableEntity>>> FindByAsync(bool isActiveOnly, int startIndex, int takeCount);
        Task<ServiceResult<DataSearchResult<TSearchableEntity>>> FindByIdAsync(Guid Id, bool isActive);
        Task<ServiceResult<DataSearchResult<TSearchableEntity>>> FindByIdListAsync(List<Guid> idList, bool isActive);

    }
}
