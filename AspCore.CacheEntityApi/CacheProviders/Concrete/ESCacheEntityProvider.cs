using AspCore.CacheEntityAccess.ElasticSearch.Abstract;
using AspCore.CacheEntityAccess.General;
using AspCore.CacheEntityApi.CacheProviders.Abstract;
using AspCore.CacheEntityApi.Convertors;
using AspCore.CacheEntityClient.QueryBuilder.Concrete;
using AspCore.Entities.Cache;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Extension;
using Nest;
using System;
using System.Collections.Generic;

namespace AspCore.CacheEntityApi.CacheProviders.Concrete
{
    public class ESCacheEntityProvider<T> : ICacheEntityProvider<T>
        where T : class, ICacheEntity, new()
    {
        private readonly IESContext _context;
        public ESCacheEntityProvider(IESContext context)
        {
            _context = context;
        }
     
        public ServiceResult<bool> CreateCacheItem(string cacheName, T cacheItem)
        {
            ServiceResult<bool> result = new ServiceResult<bool>();
            try
            {
                cacheItem.CreatedDate = DateTime.Now;
                cacheItem.LastUpdateDate = DateTime.Now;
                cacheItem.IsDeleted = false;
                result = _context.Add(cacheName, cacheItem);
            }
            catch (Exception ex)
            {
                result.ErrorMessage(ESConstants.ErrorMessages.ES_CREATE_INDEX_ITEM_ERROR_OCCURRED, ex);
            }

            return result;
        }

        public ServiceResult<bool> CreateCacheItemList(string cacheName, List<T> cacheItemList)
        {
            ServiceResult<bool> result = new ServiceResult<bool>();
            try
            {
                foreach (var cacheItem in cacheItemList)
                {
                    cacheItem.CreatedDate = DateTime.Now;
                    cacheItem.LastUpdateDate = DateTime.Now;
                    cacheItem.IsDeleted = false;
                }

                result = _context.BulkIndex(cacheName, cacheItemList);
            }
            catch (Exception ex)
            {
                result.ErrorMessage(ESConstants.ErrorMessages.ES_BULK_INDEX_ERROR_OCCURRED, ex);
            }

            return result;
        }

        public ServiceResult<bool> DeleteCacheItem(string cacheName, T cacheItem)
        {
            ServiceResult<bool> result = new ServiceResult<bool>();
            try
            {
                cacheItem.LastUpdateDate = DateTime.Now;
                cacheItem.IsDeleted = true;
                result = _context.Delete(cacheName, cacheItem);
            }
            catch (Exception ex)
            {
                result.ErrorMessage(ESConstants.ErrorMessages.ES_DELETE_INDEX_ITEM_ERROR_OCCURRED, ex);
            }

            return result;
        }

        public ServiceResult<bool> DeleteCacheItemList(string cacheName, List<T> cacheItemList)
        {
            ServiceResult<bool> result = new ServiceResult<bool>();
            try
            {
                foreach (var cacheItem in cacheItemList)
                {
                    cacheItem.LastUpdateDate = DateTime.Now;
                    cacheItem.IsDeleted = true;
                }

                result = _context.BulkIndex(cacheName, cacheItemList);
            }
            catch (Exception ex)
            {
                result.ErrorMessage(ESConstants.ErrorMessages.ES_BULK_INDEX_ERROR_OCCURRED, ex);
            }

            return result;
        }

        public ServiceResult<MinMax> MinMaxCacheItem(SearchRequestItem cacheRequestItem)
        {
            ServiceResult<MinMax> result = new ServiceResult<MinMax>();
            try
            {
                AggregationDictionary aggs = null;
                if (!string.IsNullOrEmpty(cacheRequestItem.IdFieldPropertyName))
                {
                    aggs = new AggregationDictionary();
                    aggs.Add(ESConstants.AGGREGATION_KEYS.VALUE_MIN_AGG, new MinAggregation(ESConstants.AGGREGATION_KEYS.VALUE_MIN_AGG, Infer.Field(cacheRequestItem.IdFieldPropertyName)));
                    aggs.Add(ESConstants.AGGREGATION_KEYS.VALUE_MAX_AGG, new MaxAggregation(ESConstants.AGGREGATION_KEYS.VALUE_MAX_AGG, Infer.Field(cacheRequestItem.IdFieldPropertyName)));
                }

                ISearchRequest searchRequest = new SearchRequest<T>(Indices.Index(cacheRequestItem.cacheName));

                if (aggs != null)
                    searchRequest.Aggregations = aggs;

                ServiceResult<CacheResult<T>> cacheResult = _context.Search<T>(searchRequest);

                if (cacheResult.IsSucceededAndDataIncluded() && cacheResult.Result.minMax != null)
                {
                    result.IsSucceeded = true;
                    result.Result = new MinMax();
                    result.Result.maxValue = cacheResult.Result.minMax.maxValue;
                    result.Result.minValue = cacheResult.Result.minMax.minValue;
                }
                else
                {
                    result.ErrorMessage = cacheResult.ErrorMessage;
                    result.ExceptionMessage = cacheResult.ExceptionMessage;
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage(ESConstants.ErrorMessages.ES_SEARCH_INDEX_ITEM_ERROR_OCCURRED, ex);
            }

            return result;
        }

        public ServiceResult<CacheResult<T>> ReadCacheItem(SearchRequestItem cacheRequestItem)
        {
            ServiceResult<CacheResult<T>> result = new ServiceResult<CacheResult<T>>();
            try
            {
                QueryContainer query = null;
                if (cacheRequestItem.queryContainer != null)
                    query = cacheRequestItem.queryContainer.GetInnerContainer<T>();

                QueryContainer postFilterQuery = null;
                if (cacheRequestItem.postFilterQueryContainer != null)
                    postFilterQuery = cacheRequestItem.postFilterQueryContainer.GetInnerContainer<T>();

                SourceFilter sourceFilter = null;
                if (cacheRequestItem.sourceFilter != null)
                    sourceFilter = cacheRequestItem.sourceFilter.GetSourceFilter();

                List<ISort> sortItems = null;
                if (cacheRequestItem.sortItem != null)
                    sortItems = cacheRequestItem.sortItem.GetSortItems<T>();

                AggregationDictionary aggs = null;
                if (!string.IsNullOrEmpty(cacheRequestItem.IdFieldPropertyName))
                {
                    aggs = new AggregationDictionary();
                    aggs.Add(ESConstants.AGGREGATION_KEYS.VALUE_COUNT_AGG, new ValueCountAggregation(ESConstants.AGGREGATION_KEYS.VALUE_COUNT_AGG, Infer.Field(cacheRequestItem.IdFieldPropertyName)));
                    aggs.Add(ESConstants.AGGREGATION_KEYS.VALUE_MIN_AGG, new MinAggregation(ESConstants.AGGREGATION_KEYS.VALUE_MIN_AGG, Infer.Field(cacheRequestItem.IdFieldPropertyName)));
                    aggs.Add(ESConstants.AGGREGATION_KEYS.VALUE_MAX_AGG, new MaxAggregation(ESConstants.AGGREGATION_KEYS.VALUE_MAX_AGG, Infer.Field(cacheRequestItem.IdFieldPropertyName)));
                    if (postFilterQuery != null)
                    {
                        FilterAggregation filterAggregation = new FilterAggregation(ESConstants.AGGREGATION_KEYS.VALUE_SEARCH_COUNT_AGG);
                        filterAggregation.Filter = postFilterQuery;
                        filterAggregation.Aggregations = new AggregationDictionary();
                        filterAggregation.Aggregations.Add(ESConstants.AGGREGATION_KEYS.VALUE_SEARCH_COUNT_AGG, new ValueCountAggregation(ESConstants.AGGREGATION_KEYS.VALUE_SEARCH_COUNT_AGG, Infer.Field(cacheRequestItem.IdFieldPropertyName)));
                        aggs.Add(ESConstants.AGGREGATION_KEYS.VALUE_SEARCH_COUNT_AGG, filterAggregation);

                    }
                }

                SearchRequest searchRequest = new SearchRequest<T>(Indices.Index(cacheRequestItem.cacheName));

                searchRequest.Size = cacheRequestItem.size;
                searchRequest.From = cacheRequestItem.from;
                searchRequest.Query = query;
                searchRequest.PostFilter = postFilterQuery;
                searchRequest.Source = sourceFilter;
                searchRequest.Sort = sortItems;
                if (aggs != null)
                    searchRequest.Aggregations = aggs;

                result = _context.Search<T>(searchRequest);

            }
            catch (Exception ex)
            {
                result.ErrorMessage(ESConstants.ErrorMessages.ES_SEARCH_INDEX_ITEM_ERROR_OCCURRED, ex);
            }

            return result;
        }

        public ServiceResult<bool> UpdateCacheItem(string cacheName, T cacheItem)
        {
            ServiceResult<bool> result = new ServiceResult<bool>();
            try
            {
                cacheItem.LastUpdateDate = DateTime.Now;
                cacheItem.IsDeleted = false;
                result = _context.Update(cacheName, cacheItem);
            }
            catch (Exception ex)
            {
                result.ErrorMessage(ESConstants.ErrorMessages.ES_UPDATE_INDEX_ITEM_ERROR_OCCURRED, ex);
            }

            return result;
        }

        public ServiceResult<bool> UpdateCacheItemList(string cacheName, List<T> cacheItemList)
        {
            ServiceResult<bool> result = new ServiceResult<bool>();
            try
            {
                foreach (var cacheItem in cacheItemList)
                {
                    cacheItem.LastUpdateDate = DateTime.Now;
                    cacheItem.IsDeleted = false;
                }

                result = _context.BulkIndex(cacheName, cacheItemList);
            }
            catch (Exception ex)
            {
                result.ErrorMessage(ESConstants.ErrorMessages.ES_BULK_INDEX_ERROR_OCCURRED, ex);
            }

            return result;
        }
    }
}
