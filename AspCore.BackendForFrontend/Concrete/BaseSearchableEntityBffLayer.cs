using AspCore.BackendForFrontend.Abstract;
using AspCore.DataSearch.Abstract;
using AspCore.Dependency.Concrete;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Entities.Search;
using AspCore.Extension;
using System;
using System.Collections.Generic;

namespace AspCore.BackendForFrontend.Concrete
{
    public abstract class BaseSearchableEntityBffLayer<TViewModel, TSearchableEntity,TSearchClient> : BaseEntityBffLayer<TViewModel, TSearchableEntity>, ISearchableEntityBffLayer<TViewModel, TSearchableEntity>
        where TViewModel : BaseViewModel<TSearchableEntity>, new()
        where TSearchableEntity : class, ISearchableEntity, new()
        where TSearchClient : class, IDataSearchClient<TSearchableEntity>
    {
        protected readonly TSearchClient _dataSearchClient;
        public BaseSearchableEntityBffLayer() : base()
        {
            _dataSearchClient = DependencyResolver.Current.GetService<TSearchClient>();
        }

        public ServiceResult<List<TViewModel>> FindBy(bool isActiveOnly, int startIndex, int takeCount)
        {
            var viewResult = new ServiceResult<List<TViewModel>>();

            ServiceResult<DataSearchResult<TSearchableEntity>> result = _dataSearchClient.FindBy(isActiveOnly, startIndex, takeCount);

            if (result.IsSucceededAndDataIncluded())
            {
                viewResult.ToViewModelResultFromSearchableEntityList(result);
            }
            return viewResult;
        }

        public ServiceResult<TViewModel> FindById(Guid Id, bool isActive)
        {
            var viewResult = new ServiceResult<TViewModel>();
            ServiceResult<DataSearchResult<TSearchableEntity>> result = null;

            result = _dataSearchClient.FindById(Id, isActive);

            if (result.IsSucceededAndDataIncluded())
            {
                viewResult.ToViewModelResultFromCacheEntity(result);
            }

            return viewResult;
        }

        public ServiceResult<List<TViewModel>> FindByIdList(List<Guid> idList, bool isActive)
        {
            var viewResult = new ServiceResult<List<TViewModel>>();
            ServiceResult<DataSearchResult<TSearchableEntity>> result = _dataSearchClient.FindByIdList(idList, isActive);
           
            if (result.IsSucceededAndDataIncluded())
            {
                viewResult.ToViewModelResultFromSearchableEntityList(result);
            }

            return viewResult;
        }

    }
}
