using AspCore.ElasticSearch.Abstract;
using AspCore.ElasticSearchApiClient.QueryBuilder.Concrete;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Entities.Search;
using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.DataSearchApi.ElasticSearch.Abstract
{
    public interface IElasticSearchProvider<TSearchableEntity>
          where TSearchableEntity : class, ISearchableEntity, new()
    {
        IESContext context { get; }
        ServiceResult<TSearchableEntity[]> GetSearchableEntities();
        ServiceResult<bool> ResetIndex(InitIndexRequest initRequest);
        ServiceResult<bool> InitIndex(InitIndexRequest initRequest);
        ServiceResult<bool> CreateIndexItem(TSearchableEntity[] searchableEntities);
        ServiceResult<DataSearchResult<TSearchableEntity>> ReadIndexItem(SearchRequestItem searchRequestItem);
        ServiceResult<bool> UpdateIndexItem( TSearchableEntity[] searchableEntities);
        ServiceResult<bool> DeleteIndexItem(TSearchableEntity[] searchableEntities);
    }
}
