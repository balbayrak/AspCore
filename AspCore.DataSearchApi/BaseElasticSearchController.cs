using AspCore.ConfigurationAccess.Abstract;
using AspCore.DataSearchApi.ElasticSearch.Convertors;
using AspCore.Dependency.Concrete;
using AspCore.ElasticSearch.Abstract;
using AspCore.ElasticSearch.Configuration;
using AspCore.ElasticSearch.General;
using AspCore.ElasticSearchApiClient.QueryBuilder.Concrete;
using AspCore.Entities.Constants;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Entities.Search;
using AspCore.Extension;
using AspCore.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AspCore.DataSearchApi
{
    public abstract class BaseElasticSearchController<TSearchableEntity> : BaseController
        where TSearchableEntity : class, ISearchableEntity, new()
    {
        protected IESContext _context;

        protected abstract string configurationKey { get; }

        protected virtual CreateIndexDescriptor createIndexDescriptor { get; }

        protected IConfigurationAccessor _configurationAccessor;

        public BaseElasticSearchController()
        {
            _context = DependencyResolver.Current.GetService<IESContext>();
            _configurationAccessor = DependencyResolver.Current.GetService<IConfigurationAccessor>();
        }

        [NonAction]
        protected string GetIndexAliasName()
        {
            string controllerName = ControllerContext.ActionDescriptor.ControllerName;
            ElasticSearchApiOption elasticSearchApiOption = _configurationAccessor.GetValueByKey<ElasticSearchApiOption>(configurationKey);

            if (elasticSearchApiOption != null)
            {
                return elasticSearchApiOption.ElasticSearchIndices.FirstOrDefault(t => t.ApiControllerName.Equals(controllerName, System.StringComparison.InvariantCultureIgnoreCase))?.IndexKey;
            }

            return string.Empty;
        }

        [NonAction]
        protected virtual IActionResult CreateIndex(InitIndexRequest initRequest)
        {
            ServiceResult<bool> result = new ServiceResult<bool>();

            try
            {
                ServiceResult<bool> existResult = _context.ExistIndex(initRequest.indexKey);
                if (existResult.IsSucceeded)
                {
                    if (existResult.Result)
                    {
                        ServiceResult<bool> deleteResult = _context.DeleteIndex(initRequest.indexKey);
                        if (!(deleteResult.IsSucceeded && deleteResult.Result))
                        {
                            result.ErrorMessage = deleteResult.ErrorMessage;
                            result.ExceptionMessage = deleteResult.ExceptionMessage;
                        }

                        if (!string.IsNullOrEmpty(result.ErrorMessage))
                        {
                            result =_context.CreateIndex(createIndexDescriptor);
                        }
                    }
                }
                else
                {
                    result.ErrorMessage = existResult.ErrorMessage;
                    result.ExceptionMessage = existResult.ExceptionMessage;
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage(ESConstants.ErrorMessages.ES_INIT_INDEX_ERROR_OCCURRED, ex);
            }

            return result.ToHttpResponse();
        }

        [NonAction]
        protected virtual IActionResult CreateIndexItem(TSearchableEntity[] searchableEntities)
        {
            ServiceResult<bool> result = new ServiceResult<bool>();

            try
            {
                foreach (var searchableEntity in searchableEntities)
                {
                    searchableEntity.CreatedDate = DateTime.Now;
                    searchableEntity.LastUpdateDate = DateTime.Now;
                    searchableEntity.IsDeleted = false;
                }

                if (searchableEntities.Length == 1)
                {
                    result = _context.Add(GetIndexAliasName(), searchableEntities[0]);
                }
                else
                {
                    result = _context.BulkIndex(GetIndexAliasName(), searchableEntities.ToList());
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage(ESConstants.ErrorMessages.ES_CREATE_INDEX_ITEM_ERROR_OCCURRED, ex);
            }

            return result.ToHttpResponse();
        }

        [NonAction]
        protected virtual IActionResult ReadIndexItem(SearchRequestItem searchRequestItem)
        {
            ServiceResult<DataSearchResult<TSearchableEntity>> result = new ServiceResult<DataSearchResult<TSearchableEntity>>();
            try
            {
                searchRequestItem.indexKey = GetIndexAliasName();
                QueryContainer query = null;
                if (searchRequestItem.queryContainer != null)
                    query = searchRequestItem.queryContainer.GetInnerContainer<TSearchableEntity>();

                QueryContainer postFilterQuery = null;
                if (searchRequestItem.postFilterQueryContainer != null)
                    postFilterQuery = searchRequestItem.postFilterQueryContainer.GetInnerContainer<TSearchableEntity>();

                SourceFilter sourceFilter = null;
                if (searchRequestItem.sourceFilter != null)
                    sourceFilter = searchRequestItem.sourceFilter.GetSourceFilter();

                List<ISort> sortItems = null;
                if (searchRequestItem.sortItem != null)
                    sortItems = searchRequestItem.sortItem.GetSortItems<TSearchableEntity>();

                AggregationDictionary aggs = null;
                if (!string.IsNullOrEmpty(searchRequestItem.IdFieldPropertyName))
                {
                    aggs = new AggregationDictionary();
                    aggs.Add(ESConstants.AGGREGATION_KEYS.VALUE_COUNT_AGG, new ValueCountAggregation(ESConstants.AGGREGATION_KEYS.VALUE_COUNT_AGG, Infer.Field(searchRequestItem.IdFieldPropertyName)));
                    aggs.Add(ESConstants.AGGREGATION_KEYS.VALUE_MIN_AGG, new MinAggregation(ESConstants.AGGREGATION_KEYS.VALUE_MIN_AGG, Infer.Field(searchRequestItem.IdFieldPropertyName)));
                    aggs.Add(ESConstants.AGGREGATION_KEYS.VALUE_MAX_AGG, new MaxAggregation(ESConstants.AGGREGATION_KEYS.VALUE_MAX_AGG, Infer.Field(searchRequestItem.IdFieldPropertyName)));
                    if (postFilterQuery != null)
                    {
                        FilterAggregation filterAggregation = new FilterAggregation(ESConstants.AGGREGATION_KEYS.VALUE_SEARCH_COUNT_AGG);
                        filterAggregation.Filter = postFilterQuery;
                        filterAggregation.Aggregations = new AggregationDictionary();
                        filterAggregation.Aggregations.Add(ESConstants.AGGREGATION_KEYS.VALUE_SEARCH_COUNT_AGG, new ValueCountAggregation(ESConstants.AGGREGATION_KEYS.VALUE_SEARCH_COUNT_AGG, Infer.Field(searchRequestItem.IdFieldPropertyName)));
                        aggs.Add(ESConstants.AGGREGATION_KEYS.VALUE_SEARCH_COUNT_AGG, filterAggregation);

                    }
                }

                SearchRequest searchRequest = new SearchRequest<TSearchableEntity>(Indices.Index(searchRequestItem.indexKey));

                searchRequest.Size = searchRequestItem.size;
                searchRequest.From = searchRequestItem.from;
                searchRequest.Query = query;
                searchRequest.PostFilter = postFilterQuery;
                searchRequest.Source = sourceFilter;
                searchRequest.Sort = sortItems;
                if (aggs != null)
                    searchRequest.Aggregations = aggs;

                result = _context.Search<TSearchableEntity>(searchRequest);

            }
            catch (Exception ex)
            {
                result.ErrorMessage(ESConstants.ErrorMessages.ES_SEARCH_INDEX_ITEM_ERROR_OCCURRED, ex);
            }

            return result.ToHttpResponse();
        }

        [NonAction]
        protected virtual IActionResult UpdateIndexItem(TSearchableEntity[] searchableEntities)
        {
            ServiceResult<bool> result = new ServiceResult<bool>();

            try
            {
                foreach (var searchableEntity in searchableEntities)
                {
                    searchableEntity.LastUpdateDate = DateTime.Now;
                    searchableEntity.IsDeleted = false;
                }

                if (searchableEntities.Length == 1)
                {
                    result = _context.Update(GetIndexAliasName(), searchableEntities[0]);
                }
                else
                {
                    result = _context.BulkIndex(GetIndexAliasName(), searchableEntities.ToList());
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage(ESConstants.ErrorMessages.ES_CREATE_INDEX_ITEM_ERROR_OCCURRED, ex);
            }

            return result.ToHttpResponse();
        }

        [NonAction]
        protected virtual IActionResult DeleteIndexItem(TSearchableEntity[] searchableEntities)
        {
            ServiceResult<bool> result = new ServiceResult<bool>();

            try
            {
                foreach (var searchableEntity in searchableEntities)
                {
                    searchableEntity.LastUpdateDate = DateTime.Now;
                    searchableEntity.IsDeleted = true;
                }

                if (searchableEntities.Length == 1)
                {
                    result = _context.Update(GetIndexAliasName(), searchableEntities[0]);
                }
                else
                {
                    result = _context.BulkIndex(GetIndexAliasName(), searchableEntities.ToList());
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage(ESConstants.ErrorMessages.ES_CREATE_INDEX_ITEM_ERROR_OCCURRED, ex);
            }

            return result.ToHttpResponse();
        }

        [Authorize]
        [ActionName(ApiConstants.DataSearchApi_Urls.INIT_INDEX_ACTION_NAME)]
        [HttpPost]

        public IActionResult InitIndex(InitIndexRequest initIndexRequest)
        {
            return CreateIndex(initIndexRequest);
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
