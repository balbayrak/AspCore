using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using System;
using System.Collections.Generic;

namespace AspCore.Caching.Abstract
{
    public interface ICacheEntityReader<TCacheEntity> where TCacheEntity : class, ICacheEntity, new()
    {
        ServiceResult<TCacheEntity> FindById(Guid Id, bool isActive);

        ServiceResult<List<TCacheEntity>> FindBy(bool isActive, int startIndex, int takeCount, out int totalCount);

        ServiceResult<List<TCacheEntity>> FindByIdList(List<Guid> idList, bool isActiveOnly, int takeCount);
    }
}
