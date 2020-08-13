using AspCore.ElasticSearchApiClient.QueryBuilder.Concrete;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Entities.Search;
using System;
using System.Threading.Tasks;

namespace AspCore.ElasticSearchApiClient
{
    public interface IReadOnlyElasticClient<T>
        where T : class, ISearchableEntity, new()
    {
        Task<ServiceResult<DataSearchResult<T>>> Read(Func<DataSearchBuilder<T>, DataSearchBuilder<T>> builder);
    }
}
