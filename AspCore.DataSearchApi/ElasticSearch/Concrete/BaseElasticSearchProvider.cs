using AspCore.Business.Abstract;
using AspCore.DataSearchApi.ElasticSearch.Abstract;
using AspCore.DataSearchApi.ElasticSearch.Convertors;
using AspCore.ElasticSearch.Abstract;
using AspCore.ElasticSearch.General;
using AspCore.ElasticSearchApiClient.QueryBuilder.Concrete;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Entities.Search;
using AspCore.Extension;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AspCore.DataSearchApi.ElasticSearch.Concrete
{
    public abstract class BaseElasticSearchProvider<TSearchableEntity,TSearchableEntityService> : IElasticSearchProvider<TSearchableEntity>
          where TSearchableEntity : class, ISearchableEntity, new()
          where TSearchableEntityService : ISearchableEntityService<TSearchableEntity>
    {
        public IESContext context { get; private set; }

        protected abstract CreateIndexDescriptor createIndexDescriptor { get; }

        protected string indexKey { get; private set; }

        protected string aliasKey { get; private set; }

        protected IServiceProvider ServiceProvider { get; private set; }
        public BaseElasticSearchProvider(IServiceProvider serviceProvider, string indexKey)
        {
            ServiceProvider = serviceProvider;
            this.aliasKey = indexKey;
            this.indexKey = string.Format("{0}_{1}", indexKey, DateTime.Now.ToString("yyyyMMddHHss"));
            this.context = ServiceProvider.GetRequiredService<IESContext>();
        }

        public ServiceResult<bool> ResetIndex(InitIndexRequest initRequest)
        {
            ServiceResult<bool> result = new ServiceResult<bool>();

            try
            {
                ServiceResult<bool> existResult = context.ExistIndex(aliasKey);
                if (existResult.IsSucceeded)
                {
                    if (existResult.Result)
                    {
                        ServiceResult<bool> deleteResult = context.DeleteIndex(indexKey);
                        if (!(deleteResult.IsSucceeded && deleteResult.Result))
                        {
                            result.ErrorMessage = deleteResult.ErrorMessage;
                            result.ExceptionMessage = deleteResult.ExceptionMessage;
                        }
                    }

                    if (string.IsNullOrEmpty(result.ErrorMessage))
                    {
                        result = context.CreateIndex(createIndexDescriptor);

                        if (string.IsNullOrEmpty(result.ErrorMessage) && initRequest.initializeWithData)
                        {
                            using (var scope = ServiceProvider.CreateScope())
                            {
                                var searchableEntityService = scope.ServiceProvider.GetRequiredService<TSearchableEntityService>();
                                ServiceResult<TSearchableEntity[]> entityResult = searchableEntityService.GetSearchableEntities();
                                if (entityResult.IsSucceededAndDataIncluded())
                                {
                                    result = context.BulkIndex(aliasKey, entityResult.Result.ToList(), 1000);
                                }
                                else
                                {
                                    result.ErrorMessage = entityResult.ErrorMessage;
                                    result.ExceptionMessage = entityResult.ExceptionMessage;
                                }
                            }
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

            return result;
        }

        public ServiceResult<bool> InitIndex(InitIndexRequest initRequest)
        {
            ServiceResult<bool> result = new ServiceResult<bool>();

            try
            {
                ServiceResult<bool> existResult = context.ExistIndex(aliasKey);
                if (existResult.IsSucceeded)
                {
                    if (!existResult.Result)
                    {

                        result = context.CreateIndex(createIndexDescriptor);

                        if (result.IsSucceeded && initRequest.initializeWithData)
                        {

                            using (var scope = ServiceProvider.CreateScope())
                            {
                                var searchableEntityService = scope.ServiceProvider.GetRequiredService<TSearchableEntityService>();

                                ServiceResult<TSearchableEntity[]> entityResult = searchableEntityService.GetSearchableEntities();
                                if (entityResult.IsSucceededAndDataIncluded())
                                {
                                    result = context.BulkIndex(aliasKey, entityResult.Result.ToList(), 1000);
                                    result.IsSucceeded = true;
                                }
                                else
                                {
                                    context.DeleteIndex(indexKey);
                                    result.ErrorMessage = entityResult.ErrorMessage;
                                    result.ExceptionMessage = entityResult.ExceptionMessage;
                                }
                            }
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

            return result;
        }

        public ServiceResult<bool> CreateIndexItem( TSearchableEntity[] searchableEntities)
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
                    result = context.Add(aliasKey, searchableEntities[0]);
                }
                else
                {
                    result = context.BulkIndex(aliasKey, searchableEntities.ToList(), 1000);
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage(ESConstants.ErrorMessages.ES_CREATE_INDEX_ITEM_ERROR_OCCURRED, ex);
            }

            return result;
        }
        public ServiceResult<DataSearchResult<TSearchableEntity>> ReadIndexItem(SearchRequestItem searchRequestItem)
        {
            ServiceResult<DataSearchResult<TSearchableEntity>> result = new ServiceResult<DataSearchResult<TSearchableEntity>>();
            try
            {
                searchRequestItem.indexKey = aliasKey;
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

                result = context.Search<TSearchableEntity>(searchRequest);

            }
            catch (Exception ex)
            {
                result.ErrorMessage(ESConstants.ErrorMessages.ES_SEARCH_INDEX_ITEM_ERROR_OCCURRED, ex);
            }

            return result;
        }
        public ServiceResult<bool> UpdateIndexItem( TSearchableEntity[] searchableEntities)
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
                    result = context.Update(aliasKey, searchableEntities[0]);
                }
                else
                {
                    result = context.BulkIndex(aliasKey, searchableEntities.ToList());
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage(ESConstants.ErrorMessages.ES_UPDATE_INDEX_ITEM_ERROR_OCCURRED, ex);
            }

            return result;
        }
        public ServiceResult<bool> DeleteIndexItem(TSearchableEntity[] searchableEntities)
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
                    result = context.Update(aliasKey, searchableEntities[0]);
                }
                else
                {
                    result = context.BulkIndex(aliasKey, searchableEntities.ToList());
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage(ESConstants.ErrorMessages.ES_DELETE_INDEX_ITEM_ERROR_OCCURRED, ex);
            }

            return result;
        }
    }
}

