using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using System.Collections.Generic;

namespace AspCore.Caching.Abstract
{
    public interface ICacheEntityService<TCacheEntity> where TCacheEntity : class, ICacheEntity, new()
    {
        ServiceResult<bool> Create(TCacheEntity cacheEntity);

        ServiceResult<bool> Update(TCacheEntity cacheEntity);

        ServiceResult<bool> Delete(TCacheEntity cacheEntity);

        ServiceResult<bool> UpdateList(List<TCacheEntity> cacheEntityList);

        ServiceResult<bool> CreateList(List<TCacheEntity> cacheEntityList);

        ServiceResult<bool> DeleteList(List<TCacheEntity> cacheEntityList);
    }
}
