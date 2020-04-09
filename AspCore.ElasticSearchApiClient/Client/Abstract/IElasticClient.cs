using AspCore.ElasticSearchApiClient.QueryBuilder.Concrete;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;

namespace AspCore.ElasticSearchApiClient
{
    public interface IElasticClient<T>  : IReadOnlyElasticClient<T>
        where T : class, ISearchableEntity,new()
    {
        ServiceResult<bool> ResetIndex(bool initWithData);
        ServiceResult<bool> Create(params T[] searchableEntities);
        ServiceResult<bool> Update(params T[] searchableEntities);
        ServiceResult<bool> Delete(params T[] searchableEntities);

    }
}
