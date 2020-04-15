using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.BackendForFrontend.Abstract
{
    public interface ISearchableEntityBffLayer<TViewModel, TSearchableEntity> : IEntityBffLayer<TViewModel, TSearchableEntity>
         where TViewModel : BaseViewModel<TSearchableEntity>
         where TSearchableEntity : class, ISearchableEntity, new()
    {
        ServiceResult<List<TViewModel>> FindBy(bool isActiveOnly, int startIndex, int takeCount);

        ServiceResult<TViewModel> FindById(Guid Id, bool isActive);

        ServiceResult<List<TViewModel>> FindByIdList(List<Guid> idList, bool isActive);
    }
}
