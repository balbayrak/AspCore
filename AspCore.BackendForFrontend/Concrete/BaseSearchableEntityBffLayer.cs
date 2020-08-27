using AspCore.BackendForFrontend.Abstract;
using AspCore.DataSearch.Abstract;
using AspCore.Dtos.Dto;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Entities.Search;
using AspCore.Extension;
using AspCore.Mapper.Abstract;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public async Task<ServiceResult<List<TEntityDto>>> FindBy(bool isActiveOnly, int startIndex, int takeCount)
        {
            var viewResult = new ServiceResult<List<TEntityDto>>();

            ServiceResult<DataSearchResult<TSearchableEntity>> result = await DataSearchEngine.FindByAsync(isActiveOnly, startIndex, takeCount);
            
            if (result.IsSucceededAndDataIncluded())
            {
                viewResult.ToViewModelResultFromSearchableEntityList(_autoObjectMapper,result);
            }
            return viewResult;
        }

        public async Task<ServiceResult<TEntityDto>> FindById(Guid Id, bool isActive)
        {
            var viewResult = new ServiceResult<TEntityDto>();
            ServiceResult<DataSearchResult<TSearchableEntity>> result = null;

            result = await DataSearchEngine.FindByIdAsync(Id, isActive);

            if (result.IsSucceededAndDataIncluded())
            {
                viewResult.ToViewModelResultFromSearchableEntity(_autoObjectMapper,result);
            }

            return viewResult;
        }

        public async Task<ServiceResult<List<TEntityDto>>> FindByIdList(List<Guid> idList, bool isActive)
        {
            var viewResult = new ServiceResult<List<TEntityDto>>();
            ServiceResult<DataSearchResult<TSearchableEntity>> result = await DataSearchEngine.FindByIdListAsync(idList, isActive);
           
            if (result.IsSucceededAndDataIncluded())
            {
                viewResult.ToViewModelResultFromSearchableEntityList(_autoObjectMapper,result);
            }

            return viewResult;
        }

    }
}
