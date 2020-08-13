using AspCore.DataSearchApi.ElasticSearch.Abstract;
using AspCore.Dependency.Concrete;
using AspCore.ElasticSearchApiClient.QueryBuilder.Concrete;
using AspCore.Entities.Constants;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Entities.Search;
using AspCore.Extension;
using AspCore.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace AspCore.DataSearchApi
{
    public abstract class BaseElasticSearchController<TSearchableEntity> : BaseController
        where TSearchableEntity : class, ISearchableEntity, new()
    {
        protected IElasticSearchProvider<TSearchableEntity> _elasticSearchProvider;

        protected IServiceProvider ServiceProvider { get; private set; }
        public BaseElasticSearchController(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            _elasticSearchProvider = ServiceProvider.GetRequiredService<IElasticSearchProvider<TSearchableEntity>>();
        }

        [NonAction]
        protected virtual async Task<IActionResult> ResetIndexWithData(InitIndexRequest initRequest)
        {
            ServiceResult<bool> result = await _elasticSearchProvider.ResetIndex(initRequest);
            return result.ToHttpResponse();
        }

        [NonAction]
        protected virtual async Task<IActionResult> CreateIndexItem(TSearchableEntity[] searchableEntities)
        {
            ServiceResult<bool> result = await _elasticSearchProvider.CreateIndexItem(searchableEntities);
            return result.ToHttpResponse();
        }

        [NonAction]
        protected virtual async Task<IActionResult> ReadIndexItem(SearchRequestItem searchRequestItem)
        {
            ServiceResult<DataSearchResult<TSearchableEntity>> result = await _elasticSearchProvider.ReadIndexItem(searchRequestItem);
            return result.ToHttpResponse();
        }

        [NonAction]
        protected virtual async Task<IActionResult> UpdateIndexItem(TSearchableEntity[] searchableEntities)
        {
            ServiceResult<bool> result = await _elasticSearchProvider.UpdateIndexItem(searchableEntities);
            return result.ToHttpResponse();
        }

        [NonAction]
        protected virtual async Task<IActionResult> DeleteIndexItem(TSearchableEntity[] searchableEntities)
        {
            ServiceResult<bool> result = await _elasticSearchProvider.DeleteIndexItem(searchableEntities);
            return result.ToHttpResponse();
        }

        [Authorize]
        [ActionName(ApiConstants.DataSearchApi_Urls.RESET_INDEX_ACTION_NAME)]
        [HttpPost]

        public async Task<IActionResult> ResetIndex(InitIndexRequest initIndexRequest)
        {
            return await ResetIndexWithData(initIndexRequest);
        }

        [Authorize]
        [ActionName(ApiConstants.DataSearchApi_Urls.CREATE_ACTION_NAME)]
        [HttpPost]

        public async Task<IActionResult> Create(TSearchableEntity[] searchableEntities)
        {
            return await CreateIndexItem(searchableEntities);
        }

        [Authorize]
        [ActionName(ApiConstants.DataSearchApi_Urls.READ_ACTION_NAME)]
        [HttpPost]
        public async Task<IActionResult> Read(SearchRequestItem searchRequestItem)
        {
            return await ReadIndexItem(searchRequestItem);
        }

        [Authorize]
        [ActionName(ApiConstants.DataSearchApi_Urls.UPDATE_ACTION_NAME)]
        [HttpPost]

        public async Task<IActionResult> Update(TSearchableEntity[] searchableEntities)
        {
            return await UpdateIndexItem(searchableEntities);
        }

        [Authorize]
        [ActionName(ApiConstants.DataSearchApi_Urls.DELETE_ACTION_NAME)]
        [HttpPost]

        public async Task<IActionResult> Delete(TSearchableEntity[] searchableEntities)
        {
            return await DeleteIndexItem(searchableEntities);
        }

    }
}
