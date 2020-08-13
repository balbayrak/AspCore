using AspCore.ElasticSearchApiClient.QueryBuilder.Concrete;
using AspCore.Entities.Constants;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using System.Threading.Tasks;

namespace AspCore.ElasticSearchApiClient
{
    public class ElasticClient<T> : ReadOnlyElasticClient<T>, IElasticClient<T>
        where T : class, ISearchableEntity, new()
    {
        public ElasticClient(string apiClientKey, string elasticApiRoute) : base(apiClientKey,elasticApiRoute)
        {
           
        }

        public async Task<ServiceResult<bool>> Create(params T[] searchableEntities)
        {
            _apiClient.apiUrl = _elasticApiRoute + "/" + ApiConstants.DataSearchApi_Urls.CREATE_ACTION_NAME;
            return await _apiClient.PostRequest<ServiceResult<bool>>(searchableEntities);
        }

        public async Task<ServiceResult<bool>> Update(params T[] searchableEntities)
        {
            _apiClient.apiUrl = _elasticApiRoute + "/" + ApiConstants.DataSearchApi_Urls.UPDATE_ACTION_NAME;
            return await _apiClient.PostRequest<ServiceResult<bool>>(searchableEntities);
        }

        public async Task<ServiceResult<bool>> Delete(params T[] searchableEntities)
        {
            _apiClient.apiUrl = _elasticApiRoute + "/" + ApiConstants.DataSearchApi_Urls.DELETE_ACTION_NAME;
            return await _apiClient.PostRequest<ServiceResult<bool>>(searchableEntities);
        }

        public async Task<ServiceResult<bool>> ResetIndex(bool initWithData)
        {
            _apiClient.apiUrl = _elasticApiRoute + "/" + ApiConstants.DataSearchApi_Urls.RESET_INDEX_ACTION_NAME;
            return await _apiClient.PostRequest<ServiceResult<bool>>(new InitIndexRequest
            {
                initializeWithData = initWithData
            });
        }
    }
}
