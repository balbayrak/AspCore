using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Entities.Search;
using Nest;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspCore.ElasticSearch.Abstract
{
    public interface IESContext  
    {
        Task<ServiceResult<bool>> ExistIndex(string indexName);

        Task<ServiceResult<bool>> CreateIndex<T>(string indexName, string aliasName, int numberOfReplica, int numberOfShard) where T : class, ISearchableEntity, new();

        Task<ServiceResult<bool>> CreateIndex(CreateIndexDescriptor createIndexDescriptor);

        Task<ServiceResult<bool>> DeleteIndex(string indexName);

        Task<ServiceResult<bool>> Add<T>(string aliasName, T document) where T : class, ISearchableEntity, new();

        Task<ServiceResult<bool>> Update<T>(string aliasName, T document) where T : class, ISearchableEntity, new();

        Task<ServiceResult<bool>> Delete<T>(string aliasName, T document) where T : class, ISearchableEntity, new();

        Task<ServiceResult<bool>> BulkIndex<T>(string aliasName, List<T> documents, int blockSize=1000) where T : class, ISearchableEntity, new();

        Task<ServiceResult<DataSearchResult<T>>> Search<T>(Func<SearchDescriptor<T>, ISearchRequest> selector) where T : class, ISearchableEntity, new();

        Task<ServiceResult<DataSearchResult<T>>> Search<T>(ISearchRequest searchRequest) where T : class, ISearchableEntity, new();

    }
}
