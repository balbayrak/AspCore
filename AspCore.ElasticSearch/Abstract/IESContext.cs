using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Entities.Search;
using Nest;
using System;
using System.Collections.Generic;

namespace AspCore.ElasticSearch.Abstract
{
    public interface IESContext  
    {
        ServiceResult<bool> ExistIndex(string indexName);

        ServiceResult<bool> CreateIndex<T>(string indexName, string aliasName, int numberOfReplica, int numberOfShard) where T : class, ISearchableEntity, new();

        ServiceResult<bool> CreateIndex(CreateIndexDescriptor createIndexDescriptor);

        ServiceResult<bool> DeleteIndex(string indexName);

        ServiceResult<bool> Add<T>(string aliasName, T document) where T : class, ISearchableEntity, new();

        ServiceResult<bool> Update<T>(string aliasName, T document) where T : class, ISearchableEntity, new();

        ServiceResult<bool> Delete<T>(string aliasName, T document) where T : class, ISearchableEntity, new();

        ServiceResult<bool> BulkIndex<T>(string aliasName, List<T> documents) where T : class, ISearchableEntity, new();

        ServiceResult<bool> BulkIndexWithBlockSize<T>(string aliasName, List<T> documents, int blockSize) where T : class, ISearchableEntity, new();

        ServiceResult<DataSearchResult<T>> Search<T>(Func<SearchDescriptor<T>, ISearchRequest> selector) where T : class, ISearchableEntity, new();

        ServiceResult<DataSearchResult<T>> Search<T>(ISearchRequest searchRequest) where T : class, ISearchableEntity, new();

    }
}
