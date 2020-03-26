using AspCore.Entities.Cache;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using Nest;
using System;
using System.Collections.Generic;

namespace AspCore.CacheEntityAccess.ElasticSearch.Abstract
{
    public interface IESContext  
    {
        ServiceResult<bool> CreateIndex<T>(string indexName, string aliasName, int numberOfReplica, int numberOfShard) where T : class, ICacheEntity, new();

        ServiceResult<bool> CreateIndex(CreateIndexDescriptor createIndexDescriptor);

        ServiceResult<bool> DeleteIndex(string indexName);

        ServiceResult<bool> Add<T>(string aliasName, T document) where T : class, ICacheEntity, new();

        ServiceResult<bool> Update<T>(string aliasName, T document) where T : class, ICacheEntity, new();

        ServiceResult<bool> Delete<T>(string aliasName, T document) where T : class, ICacheEntity, new();

        ServiceResult<bool> BulkIndex<T>(string aliasName, List<T> documents) where T : class, ICacheEntity, new();

        ServiceResult<bool> BulkIndexWithBlockSize<T>(string aliasName, List<T> documents, int blockSize) where T : class, ICacheEntity, new();

        ServiceResult<CacheResult<T>> Search<T>(Func<SearchDescriptor<T>, ISearchRequest> selector) where T : class, ICacheEntity, new();

        ServiceResult<CacheResult<T>> Search<T>(ISearchRequest searchRequest) where T : class, ICacheEntity, new();

    }
}
