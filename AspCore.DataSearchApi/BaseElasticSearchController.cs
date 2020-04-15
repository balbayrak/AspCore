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

namespace AspCore.DataSearchApi
{
    public abstract class BaseElasticSearchController<TSearchableEntity> : BaseController
        where TSearchableEntity : class, ISearchableEntity, new()
    {
        protected IElasticSearchProvider<TSearchableEntity> _elasticSearchProvider;

        public BaseElasticSearchController()
        {
            _elasticSearchProvider = DependencyResolver.Current.GetService<IElasticSearchProvider<TSearchableEntity>>();
        }

        [NonAction]
        protected virtual IActionResult ResetIndexWithData(InitIndexRequest initRequest)
        {
            ServiceResult<bool> result = _elasticSearchProvider.ResetIndex(initRequest);
            return result.ToHttpResponse();
        }

        [NonAction]
        protected virtual IActionResult CreateIndexItem(TSearchableEntity[] searchableEntities)
        {
            ServiceResult<bool> result = _elasticSearchProvider.CreateIndexItem(searchableEntities);
            return result.ToHttpResponse();
        }

        [NonAction]
        protected virtual IActionResult ReadIndexItem(SearchRequestItem searchRequestItem)
        {
            ServiceResult<DataSearchResult<TSearchableEntity>> result = _elasticSearchProvider.ReadIndexItem(searchRequestItem);
            return result.ToHttpResponse();
        }

        [NonAction]
        protected virtual IActionResult UpdateIndexItem(TSearchableEntity[] searchableEntities)
        {
            ServiceResult<bool> result = _elasticSearchProvider.UpdateIndexItem(searchableEntities);
            return result.ToHttpResponse();
        }

        [NonAction]
        protected virtual IActionResult DeleteIndexItem(TSearchableEntity[] searchableEntities)
        {
            ServiceResult<bool> result = _elasticSearchProvider.DeleteIndexItem(searchableEntities);
            return result.ToHttpResponse();
        }

        [Authorize]
        [ActionName(ApiConstants.DataSearchApi_Urls.RESET_INDEX_ACTION_NAME)]
        [HttpPost]

        public IActionResult ResetIndex(InitIndexRequest initIndexRequest)
        {
            return ResetIndexWithData(initIndexRequest);
        }

        [Authorize]
        [ActionName(ApiConstants.DataSearchApi_Urls.CREATE_ACTION_NAME)]
        [HttpPost]

        public IActionResult Create(TSearchableEntity[] searchableEntities)
        {
            return CreateIndexItem(searchableEntities);
        }

        [Authorize]
        [ActionName(ApiConstants.DataSearchApi_Urls.READ_ACTION_NAME)]
        [HttpPost]
        public IActionResult Read(SearchRequestItem searchRequestItem)
        {
            return ReadIndexItem(searchRequestItem);
        }

        [Authorize]
        [ActionName(ApiConstants.DataSearchApi_Urls.UPDATE_ACTION_NAME)]
        [HttpPost]

        public IActionResult Update(TSearchableEntity[] searchableEntities)
        {
            return UpdateIndexItem(searchableEntities);
        }

        [Authorize]
        [ActionName(ApiConstants.DataSearchApi_Urls.DELETE_ACTION_NAME)]
        [HttpPost]

        public IActionResult Delete(TSearchableEntity[] searchableEntities)
        {
            return DeleteIndexItem(searchableEntities);
        }

    }
}
