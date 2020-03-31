﻿using AspCore.Dependency.Concrete;
using AspCore.Entities.Cache;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.CacheEntityAccess.ElasticSearch.Abstract;
using AspCore.CacheEntityAccess.General;
using AspCore.Extension;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AspCore.CacheEntityAccess.ElasticSearch.Concrete
{
    public class ESContext : IESContext
    {
        private IElasticClient _elasticClient;

        public ESContext()
        {
            _elasticClient = DependencyResolver.Current.GetService<IElasticClient>();
        }

        public ServiceResult<bool> BulkIndex<T>(string aliasName, List<T> documents)
            where T : class, ICacheEntity, new()
        {
            ServiceResult<bool> result = new ServiceResult<bool>();
            try
            {
                var response = _elasticClient.IndexMany(documents, aliasName);
                if (response.IsValid && response.OriginalException == null)
                {
                    result.IsSucceeded = true;
                    result.Result = true;
                }
                else
                {
                    result.ErrorMessage(ESConstants.ErrorMessages.ES_BULK_INDEX_ERROR_OCCURRED, response.OriginalException);
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage(ESConstants.ErrorMessages.ES_BULK_INDEX_ERROR_OCCURRED, ex);
            }

            return result;
        }

        public ServiceResult<bool> BulkIndexWithBlockSize<T>(string aliasName, List<T> documents, int blockSize)
                  where T : class, ICacheEntity, new()
        {
            ServiceResult<bool> result = new ServiceResult<bool>();
            try
            {
                BulkResponse response = null;
                int take = blockSize;
                int startIndex = 0;
                int indexer = 1;
                int remain = documents.Count;
                bool breakFlag = false;
                while (true)
                {
                    if (remain > blockSize)
                    {
                        take = blockSize;
                    }
                    else
                    {
                        take = remain + 1;
                        breakFlag = true;
                    }

                    response = _elasticClient.IndexMany(documents.Skip(startIndex).Take(take).ToList(), aliasName);
                    if (!response.IsValid) break;
                    remain -= blockSize;
                    startIndex = indexer * blockSize;
                    indexer++;
                    if (breakFlag) break;
                }

                if (response.IsValid && response.OriginalException == null)
                {
                    result.IsSucceeded = true;
                    result.Result = true;
                }
                else
                {
                    result.ErrorMessage(ESConstants.ErrorMessages.ES_BULK_INDEX_ERROR_OCCURRED, response.OriginalException);
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage(ESConstants.ErrorMessages.ES_BULK_INDEX_ERROR_OCCURRED, ex);
            }
            return result;
        }


        public ServiceResult<bool> CreateIndex<T>(string indexName, string aliasName, int numberOfReplica, int numberOfShard)
                  where T : class, ICacheEntity, new()
        {
            ServiceResult<bool> result = new ServiceResult<bool>();
            try
            {
                var createIndexDescriptor = new CreateIndexDescriptor(indexName)
                    .Settings
                    (
                    t => t.NumberOfReplicas(numberOfReplica)
                    .NumberOfShards(numberOfShard))
                    .Map<T>(m => m.AutoMap())
                    .Aliases(a => a.Alias(aliasName));

                var response = _elasticClient.Indices.Create(createIndexDescriptor);

                if (response.IsValid && response.OriginalException == null)
                {
                    result.IsSucceeded = true;
                    result.Result = true;
                }
                else
                {
                    result.ErrorMessage(ESConstants.ErrorMessages.ES_CREATE_INDEX_ERROR_OCCURRED, response.OriginalException);
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage(ESConstants.ErrorMessages.ES_CREATE_INDEX_ERROR_OCCURRED, ex);
            }

            return result;
        }

        public ServiceResult<bool> CreateIndex(CreateIndexDescriptor createIndexDescriptor)
        {
            ServiceResult<bool> result = new ServiceResult<bool>();
            try
            {
                var response = _elasticClient.Indices.Create(createIndexDescriptor);
                if (response.IsValid && response.OriginalException == null)
                {
                    result.IsSucceeded = true;
                    result.Result = true;
                }
                else
                {
                    result.ErrorMessage(ESConstants.ErrorMessages.ES_CREATE_INDEX_ERROR_OCCURRED, response.OriginalException);
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage(ESConstants.ErrorMessages.ES_CREATE_INDEX_ERROR_OCCURRED, ex);
            }

            return result;

        }

        public ServiceResult<bool> DeleteIndex(string indexName)
        {
            ServiceResult<bool> result = new ServiceResult<bool>();
            try
            {
                var response = _elasticClient.Indices.Delete(Indices.Index(indexName));

                if (response.IsValid && response.OriginalException == null)
                {
                    result.IsSucceeded = true;
                    result.Result = true;
                }
                else
                {
                    result.ErrorMessage(ESConstants.ErrorMessages.ES_DELETE_INDEX_ERROR_OCCURRED, response.OriginalException);
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage(ESConstants.ErrorMessages.ES_DELETE_INDEX_ERROR_OCCURRED, ex);
            }

            return result;
        }

        public ServiceResult<bool> Add<T>(string aliasName, T document)
                  where T : class, ICacheEntity, new()
        {
            ServiceResult<bool> result = new ServiceResult<bool>();
            try
            {
                var response = _elasticClient.Index(document, i => i
                 .Index(aliasName));

                if (response.IsValid && response.OriginalException == null)
                {
                    result.IsSucceeded = true;
                    result.Result = true;
                }
                else
                {
                    result.ErrorMessage(ESConstants.ErrorMessages.ES_CREATE_INDEX_ITEM_ERROR_OCCURRED, response.OriginalException);
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage(ESConstants.ErrorMessages.ES_CREATE_INDEX_ITEM_ERROR_OCCURRED, ex);
            }

            return result;
        }

        public ServiceResult<bool> Update<T>(string aliasName, T document)
                  where T : class, ICacheEntity, new()
        {
            ServiceResult<bool> result = new ServiceResult<bool>();
            try
            {
                var response = _elasticClient.Update(DocumentPath<T>
               .Id(Id.From(document)),
               u => u
                   .Index(aliasName)
                   .DocAsUpsert(true)
                   .Doc(document));

                if (response.IsValid && response.OriginalException == null)
                {
                    result.IsSucceeded = true;
                    result.Result = true;
                }
                else
                {
                    result.ErrorMessage(ESConstants.ErrorMessages.ES_UPDATE_INDEX_ITEM_ERROR_OCCURRED, response.OriginalException);
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage(ESConstants.ErrorMessages.ES_UPDATE_INDEX_ITEM_ERROR_OCCURRED, ex);
            }

            return result;
        }

        public ServiceResult<bool> Delete<T>(string aliasName, T document)
                  where T : class, ICacheEntity, new()
        {
            ServiceResult<bool> result = new ServiceResult<bool>();
            try
            {
                var response = _elasticClient.Delete(DocumentPath<T>.Id(Id.From(document)),
                    u => u
                   .Index(aliasName));

                if (response.IsValid && response.OriginalException == null)
                {
                    result.IsSucceeded = true;
                    result.Result = true;
                }
                else
                {
                    result.ErrorMessage(ESConstants.ErrorMessages.ES_DELETE_INDEX_ITEM_ERROR_OCCURRED, response.OriginalException);
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage(ESConstants.ErrorMessages.ES_DELETE_INDEX_ITEM_ERROR_OCCURRED, ex);
            }

            return result;
        }

        public ServiceResult<CacheResult<T>> Search<T>(Func<SearchDescriptor<T>, ISearchRequest> selector)
                  where T : class, ICacheEntity, new()
        {
            ServiceResult<CacheResult<T>> result = new ServiceResult<CacheResult<T>>();
            try
            {
                ISearchResponse<T> response = _elasticClient.Search<T>(selector);
                
                ValueAggregate totalAggregate = null;
                if (response.Aggregations.ContainsKey(ESConstants.AGGREGATION_KEYS.VALUE_COUNT_AGG))
                    totalAggregate = response.Aggregations.ValueCount(ESConstants.AGGREGATION_KEYS.VALUE_COUNT_AGG);
               
                SingleBucketAggregate searchAggregate = null;
                if (response.Aggregations.ContainsKey(ESConstants.AGGREGATION_KEYS.VALUE_SEARCH_COUNT_AGG))
                    searchAggregate = (SingleBucketAggregate)response.Aggregations[ESConstants.AGGREGATION_KEYS.VALUE_SEARCH_COUNT_AGG];

                ValueAggregate minAggregate = null;
                if (response.Aggregations.ContainsKey(ESConstants.AGGREGATION_KEYS.VALUE_MIN_AGG))
                    minAggregate = (ValueAggregate)response.Aggregations[ESConstants.AGGREGATION_KEYS.VALUE_MIN_AGG];


                ValueAggregate maxAggregate = null;
                if (response.Aggregations.ContainsKey(ESConstants.AGGREGATION_KEYS.VALUE_MAX_AGG))
                    maxAggregate = (ValueAggregate)response.Aggregations[ESConstants.AGGREGATION_KEYS.VALUE_MAX_AGG];

                if (response.IsValid && response.OriginalException == null && response.Documents != null)
                {
                    result.IsSucceeded = true;
                    result.Result = new CacheResult<T>();
                    result.Result.cacheItems = response.Documents.ToList();
                    result.Result.totalCount = totalAggregate != null ? (totalAggregate.Value.HasValue ? Convert.ToInt32(totalAggregate.Value.Value) : 0) : 0;
                    result.Result.searchCount = searchAggregate != null ? Convert.ToInt32(searchAggregate.DocCount) : 0;
                    result.Result.minMax = new MinMax();
                    result.Result.minMax.minValue = minAggregate != null ? Convert.ToDecimal(minAggregate.Value) : 0;
                    result.Result.minMax.maxValue = maxAggregate != null ? Convert.ToDecimal(maxAggregate.Value) : 0;
                }
                else
                {
                    result.ErrorMessage(ESConstants.ErrorMessages.ES_SEARCH_INDEX_ITEM_ERROR_OCCURRED, response.OriginalException);
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage(ESConstants.ErrorMessages.ES_SEARCH_INDEX_ITEM_ERROR_OCCURRED, ex);
            }

            return result;
        }

        public ServiceResult<CacheResult<T>> Search<T>(ISearchRequest searchRequest)
                  where T : class, ICacheEntity, new()
        {
            ServiceResult<CacheResult<T>> result = new ServiceResult<CacheResult<T>>();
            try
            {
                var response = _elasticClient.Search<T>(searchRequest);

                ValueAggregate totalAggregate = null;
                if (response.Aggregations.ContainsKey(ESConstants.AGGREGATION_KEYS.VALUE_COUNT_AGG))
                    totalAggregate = response.Aggregations.ValueCount(ESConstants.AGGREGATION_KEYS.VALUE_COUNT_AGG);

                SingleBucketAggregate searchAggregate = null;
                if (response.Aggregations.ContainsKey(ESConstants.AGGREGATION_KEYS.VALUE_SEARCH_COUNT_AGG))
                    searchAggregate = (SingleBucketAggregate)response.Aggregations[ESConstants.AGGREGATION_KEYS.VALUE_SEARCH_COUNT_AGG];

                ValueAggregate minAggregate = null;
                if (response.Aggregations.ContainsKey(ESConstants.AGGREGATION_KEYS.VALUE_MIN_AGG))
                    minAggregate = (ValueAggregate)response.Aggregations[ESConstants.AGGREGATION_KEYS.VALUE_MIN_AGG];


                ValueAggregate maxAggregate = null;
                if (response.Aggregations.ContainsKey(ESConstants.AGGREGATION_KEYS.VALUE_MAX_AGG))
                    maxAggregate = (ValueAggregate)response.Aggregations[ESConstants.AGGREGATION_KEYS.VALUE_MAX_AGG];

                if (response.IsValid && response.OriginalException == null && response.Documents != null)
                {
                    result.IsSucceeded = true;
                    result.Result = new CacheResult<T>();
                    result.Result.cacheItems = response.Documents.ToList();
                    result.Result.totalCount = totalAggregate != null ? (totalAggregate.Value.HasValue ? Convert.ToInt32(totalAggregate.Value.Value) : 0) : 0;
                    result.Result.searchCount = searchAggregate != null ? Convert.ToInt32(searchAggregate.DocCount) : 0;
                    result.Result.minMax = new MinMax();
                    result.Result.minMax.minValue = minAggregate != null ? Convert.ToDecimal(minAggregate.Value) : 0;
                    result.Result.minMax.maxValue = maxAggregate != null ? Convert.ToDecimal(maxAggregate.Value) : 0;
                }
                else
                {
                    result.ErrorMessage(ESConstants.ErrorMessages.ES_SEARCH_INDEX_ITEM_ERROR_OCCURRED, response.OriginalException);
                }
            }
            catch (Exception ex)
            {
                result.ErrorMessage(ESConstants.ErrorMessages.ES_SEARCH_INDEX_ITEM_ERROR_OCCURRED, ex);
            }

            return result;
        }

    }
}