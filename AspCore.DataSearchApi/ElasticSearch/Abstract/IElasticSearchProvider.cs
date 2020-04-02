using AspCore.Dependency.Abstract;
using AspCore.ElasticSearchApiClient.QueryBuilder.Concrete;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Entities.Search;
using System.Collections.Generic;

namespace AspCore.DataSearchApi.ElasticSearch.Abstract
{
    public interface IElasticSearchProvider<T> : ITransientType where T : class, ISearchableEntity, new()
    {
        ServiceResult<bool> CreateSearchableEntity(string indexKey, T searchableEntity);

        ServiceResult<bool> UpdateSearchableEntity(string indexKey, T searchableEntity);

        ServiceResult<bool> DeleteSearchableEntity(string indexKey, T searchableEntity);

        ServiceResult<DataSearchResult<T>> ReadSearchableEntity(SearchRequestItem searchRequestItem);

        ServiceResult<bool> UpdateSearchableEntityList(string indexKey, List<T> searchableEntityList);

        ServiceResult<bool> DeleteSearchableEntityList(string indexKey, List<T> searchableEntityList);

        ServiceResult<bool> CreateSearchableEntityList(string indexKey, List<T> searchableEntityList);
    }
}
