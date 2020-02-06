using System;
using System.Collections.Generic;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;

namespace AspCore.Caching.Abstract
{
    public interface ICacheHelper<TCacheEntity> where TCacheEntity : class, IEntity, new()
    {
        ServiceResult<TCacheEntity> FindById(Guid Id, bool isActive);

        ServiceResult<List<TCacheEntity>> FindBy(bool isActive, int startIndex, int takeCount, out int totalCount);

        ServiceResult<List<TCacheEntity>> FindByIdList(List<Guid> idList, bool isActiveOnly, int takeCount);

        ServiceResult<bool> Create(TCacheEntity cacheEntity);

        ServiceResult<bool> Update(TCacheEntity cacheEntity);

        ServiceResult<bool> Delete(TCacheEntity cacheEntity);

        ServiceResult<bool> UpdateList(List<TCacheEntity> cacheEntityList);

        ServiceResult<bool> CreateList(List<TCacheEntity> cacheEntityList);

        ServiceResult<bool> DeleteList(List<TCacheEntity> cacheEntityList);
    }
}
