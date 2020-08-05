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
using AspCore.Dtos.Dto;
using AspCore.Mapper.Abstract;

namespace AspCore.BackendForFrontend.Concrete
{
    public abstract class BaseSearchableEntityBffLayer<TSearchableEntity,TEntityDto, TCreatedDto,TUpdatedDto,TDataSearchEngine> : BaseEntityBffLayer<TEntityDto, TCreatedDto, TUpdatedDto>, ISearchableEntityBffLayer<TEntityDto,TCreatedDto,TUpdatedDto>
        where TSearchableEntity : class, ISearchableEntity, new()
        where TDataSearchEngine : class, IDataSearchEngine<TSearchableEntity>
        where TEntityDto : class, IEntityDto, new()
        where TCreatedDto : class, IEntityDto, new()
        where TUpdatedDto : class, IEntityDto, new()

    {
        protected readonly TDataSearchEngine DataSearchEngine;
        private readonly IAutoObjectMapper _autoObjectMapper;
        public BaseSearchableEntityBffLayer(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            DataSearchEngine = ServiceProvider.GetRequiredService<TDataSearchEngine>();
            _autoObjectMapper = ServiceProvider.GetRequiredService<IAutoObjectMapper>();
        }

        public ServiceResult<List<TEntityDto>> FindBy(bool isActiveOnly, int startIndex, int takeCount)
        {
            var viewResult = new ServiceResult<List<TEntityDto>>();

            ServiceResult<DataSearchResult<TSearchableEntity>> result = DataSearchEngine.FindBy(isActiveOnly, startIndex, takeCount);
            
            if (result.IsSucceededAndDataIncluded())
            {
                viewResult.ToViewModelResultFromSearchableEntityList(_autoObjectMapper,result);
            }
            return viewResult;
        }

        public ServiceResult<TEntityDto> FindById(Guid Id, bool isActive)
        {
            var viewResult = new ServiceResult<TEntityDto>();
            ServiceResult<DataSearchResult<TSearchableEntity>> result = null;

            result = DataSearchEngine.FindById(Id, isActive);

            if (result.IsSucceededAndDataIncluded())
            {
                viewResult.ToViewModelResultFromCacheEntity(_autoObjectMapper,result);
            }

            return viewResult;
        }

        public ServiceResult<List<TEntityDto>> FindByIdList(List<Guid> idList, bool isActive)
        {
            var viewResult = new ServiceResult<List<TEntityDto>>();
            ServiceResult<DataSearchResult<TSearchableEntity>> result = DataSearchEngine.FindByIdList(idList, isActive);
           
            if (result.IsSucceededAndDataIncluded())
            {
                viewResult.ToViewModelResultFromSearchableEntityList(_autoObjectMapper,result);
            }

            return viewResult;
        }

    }
}
