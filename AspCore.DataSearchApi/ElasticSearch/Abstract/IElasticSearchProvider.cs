using AspCore.ElasticSearch.Abstract;
using AspCore.ElasticSearchApiClient.QueryBuilder.Concrete;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Entities.Search;
using Nest;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AspCore.DataSearchApi.ElasticSearch.Abstract
{
    public interface IElasticSearchProvider<TSearchableEntity>
          where TSearchableEntity : class, ISearchableEntity, new()
    {
        IESContext context { get; }

        Task<ServiceResult<bool>> ResetIndex(InitIndexRequest initRequest);
        Task<ServiceResult<bool>> InitIndex(InitIndexRequest initRequest);
        Task<ServiceResult<bool>> CreateIndexItem(TSearchableEntity[] searchableEntities);
        Task<ServiceResult<DataSearchResult<TSearchableEntity>>> ReadIndexItem(SearchRequestItem searchRequestItem);
        Task<ServiceResult<bool>> UpdateIndexItem( TSearchableEntity[] searchableEntities);
        Task<ServiceResult<bool>> DeleteIndexItem(TSearchableEntity[] searchableEntities);
    }
}
