using AspCore.ElasticSearchApiClient.QueryBuilder.Concrete;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using System.Threading.Tasks;

namespace AspCore.ElasticSearchApiClient
{
    public interface IElasticClient<T>  : IReadOnlyElasticClient<T>
        where T : class, ISearchableEntity,new()
    {
        Task<ServiceResult<bool>> ResetIndex(bool initWithData);
        Task<ServiceResult<bool>> Create(params T[] searchableEntities);
        Task<ServiceResult<bool>> Update(params T[] searchableEntities);
        Task<ServiceResult<bool>> Delete(params T[] searchableEntities);

        

    }
}
