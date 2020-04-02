using AspCore.DataSearchApi.ElasticSearch.Abstract;
using AspCore.DataSearchApi.ElasticSearch.Convertors;
using AspCore.ElasticSearch.Abstract;
using AspCore.ElasticSearch.General;
using AspCore.ElasticSearchApiClient.QueryBuilder.Concrete;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Entities.Search;
using AspCore.Extension;
using Nest;
using System;
using System.Collections.Generic;

namespace AspCore.DataSearchApi.ElasticSearch.Concrete
{
    public class ElasticSearchProvider<T> : IElasticSearchProvider<T>
        where T : class, ISearchableEntity, new()
    {
        private readonly IESContext _context;
        public ElasticSearchProvider(IESContext context)
        {
            _context = context;
        }
     
        public ServiceResult<bool> CreateSearchableEntity(string indexKey, T searchableEntity)
        {
            ServiceResult<bool> result = new ServiceResult<bool>();
            try
            {
                searchableEntity.CreatedDate = DateTime.Now;
                searchableEntity.LastUpdateDate = DateTime.Now;
                searchableEntity.IsDeleted = false;
                result = _context.Add(indexKey, searchableEntity);
            }
            catch (Exception ex)
            {
                result.ErrorMessage(ESConstants.ErrorMessages.ES_CREATE_INDEX_ITEM_ERROR_OCCURRED, ex);
            }

            return result;
        }

        public ServiceResult<bool> CreateSearchableEntityList(string indexKey, List<T> searchableEntityList)
        {
            ServiceResult<bool> result = new ServiceResult<bool>();
            try
            {
                foreach (var searchableEntity in searchableEntityList)
                {
                    searchableEntity.CreatedDate = DateTime.Now;
                    searchableEntity.LastUpdateDate = DateTime.Now;
                    searchableEntity.IsDeleted = false;
                }

                result = _context.BulkIndex(indexKey, searchableEntityList);
            }
            catch (Exception ex)
            {
                result.ErrorMessage(ESConstants.ErrorMessages.ES_BULK_INDEX_ERROR_OCCURRED, ex);
            }

            return result;
        }

        public ServiceResult<bool> DeleteSearchableEntity(string indexKey, T searchableEntity)
        {
            ServiceResult<bool> result = new ServiceResult<bool>();
            try
            {
                searchableEntity.LastUpdateDate = DateTime.Now;
                searchableEntity.IsDeleted = true;
                result = _context.Delete(indexKey, searchableEntity);
            }
            catch (Exception ex)
            {
                result.ErrorMessage(ESConstants.ErrorMessages.ES_DELETE_INDEX_ITEM_ERROR_OCCURRED, ex);
            }

            return result;
        }

        public ServiceResult<bool> DeleteSearchableEntityList(string indexKey, List<T> searchableEntityList)
        {
            ServiceResult<bool> result = new ServiceResult<bool>();
            try
            {
                foreach (var searchableEntity in searchableEntityList)
                {
                    searchableEntity.LastUpdateDate = DateTime.Now;
                    searchableEntity.IsDeleted = true;
                }

                result = _context.BulkIndex(indexKey, searchableEntityList);
            }
            catch (Exception ex)
            {
                result.ErrorMessage(ESConstants.ErrorMessages.ES_BULK_INDEX_ERROR_OCCURRED, ex);
            }

            return result;
        }

        public ServiceResult<DataSearchResult<T>> ReadSearchableEntity(SearchRequestItem searchRequestItem)
        {
            ServiceResult<DataSearchResult<T>> result = new ServiceResult<DataSearchResult<T>>();
            try
            {
                QueryContainer query = null;
                if (searchRequestItem.queryContainer != null)
                    query = searchRequestItem.queryContainer.GetInnerContainer<T>();

                QueryContainer postFilterQuery = null;
                if (searchRequestItem.postFilterQueryContainer != null)
                    postFilterQuery = searchRequestItem.postFilterQueryContainer.GetInnerContainer<T>();

                SourceFilter sourceFilter = null;
                if (searchRequestItem.sourceFilter != null)
                    sourceFilter = searchRequestItem.sourceFilter.GetSourceFilter();

                List<ISort> sortItems = null;
                if (searchRequestItem.sortItem != null)
                    sortItems = searchRequestItem.sortItem.GetSortItems<T>();

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

                SearchRequest searchRequest = new SearchRequest<T>(Indices.Index(searchRequestItem.indexKey));

                searchRequest.Size = searchRequestItem.size;
                searchRequest.From = searchRequestItem.from;
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

        public ServiceResult<bool> UpdateSearchableEntity(string indexKey, T searchableEntity)
        {
            ServiceResult<bool> result = new ServiceResult<bool>();
            try
            {
                searchableEntity.LastUpdateDate = DateTime.Now;
                searchableEntity.IsDeleted = false;
                result = _context.Update(indexKey, searchableEntity);
            }
            catch (Exception ex)
            {
                result.ErrorMessage(ESConstants.ErrorMessages.ES_UPDATE_INDEX_ITEM_ERROR_OCCURRED, ex);
            }

            return result;
        }

        public ServiceResult<bool> UpdateSearchableEntityList(string indexKey, List<T> searchableEntityList)
        {
            ServiceResult<bool> result = new ServiceResult<bool>();
            try
            {
                foreach (var searchableEntity in searchableEntityList)
                {
                    searchableEntity.LastUpdateDate = DateTime.Now;
                    searchableEntity.IsDeleted = false;
                }

                result = _context.BulkIndex(indexKey, searchableEntityList);
            }
            catch (Exception ex)
            {
                result.ErrorMessage(ESConstants.ErrorMessages.ES_BULK_INDEX_ERROR_OCCURRED, ex);
            }

            return result;
        }
    }
}
