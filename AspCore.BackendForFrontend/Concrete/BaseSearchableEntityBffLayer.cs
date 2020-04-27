using AspCore.BackendForFrontend.Abstract;
using AspCore.DataSearch.Abstract;
using AspCore.Dependency.Concrete;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Entities.Search;
using AspCore.Extension;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace AspCore.BackendForFrontend.Concrete
{
    public abstract class BaseSearchableEntityBffLayer<TViewModel, TSearchableEntity,TSearchClient> : BaseEntityBffLayer<TViewModel, TSearchableEntity>, ISearchableEntityBffLayer<TViewModel, TSearchableEntity>
        where TViewModel : BaseViewModel<TSearchableEntity>, new()
        where TSearchableEntity : class, ISearchableEntity, new()
        where TSearchClient : class, IDataSearchClient<TSearchableEntity>
    {
        protected readonly TSearchClient DataSearchClient;
        public BaseSearchableEntityBffLayer(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            DataSearchClient = ServiceProvider.GetRequiredService<TSearchClient>();
        }

        public ServiceResult<List<TViewModel>> FindBy(bool isActiveOnly, int startIndex, int takeCount)
        {
            var viewResult = new ServiceResult<List<TViewModel>>();

            ServiceResult<DataSearchResult<TSearchableEntity>> result = DataSearchClient.FindBy(isActiveOnly, startIndex, takeCount);

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

            result = DataSearchClient.FindById(Id, isActive);

            if (result.IsSucceededAndDataIncluded())
            {
                viewResult.ToViewModelResultFromCacheEntity(result);
            }

            return viewResult;
        }

        public ServiceResult<List<TViewModel>> FindByIdList(List<Guid> idList, bool isActive)
        {
            var viewResult = new ServiceResult<List<TViewModel>>();
            ServiceResult<DataSearchResult<TSearchableEntity>> result = DataSearchClient.FindByIdList(idList, isActive);
           
            if (result.IsSucceededAndDataIncluded())
            {
                viewResult.ToViewModelResultFromSearchableEntityList(result);
            }

            return viewResult;
        }

    }
}
