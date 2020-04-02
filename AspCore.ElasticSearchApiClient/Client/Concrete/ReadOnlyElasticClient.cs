using AspCore.ApiClient.Abstract;
using AspCore.ApiClient.Entities.Concrete;
using AspCore.ElasticSearchApiClient.QueryBuilder.Concrete;
using AspCore.Entities.Constants;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Entities.Search;
using System;

namespace AspCore.ElasticSearchApiClient
{
    public class ReadOnlyElasticClient<T> : IReadOnlyElasticClient<T>
        where T : class, ISearchableEntity, new()
    {
        protected IAuthenticatedApiClient _apiClient { get; set; }
        protected string _elasticApiRoute { get; }

        public ReadOnlyElasticClient(string apiClientKey, string elasticApiRoute)
        {
            _apiClient = ApiClientFactory.GetApiClient(apiClientKey);

            if (!elasticApiRoute.StartsWith("/"))
            {
                elasticApiRoute = "/" + elasticApiRoute;
            }
            _elasticApiRoute = elasticApiRoute;
        }

        public ServiceResult<DataSearchResult<T>> Read(Func<DataSearchBuilder<T>, DataSearchBuilder<T>> builder)
        {
            DataSearchBuilder<T> searchBuilder = new DataSearchBuilder<T>();
            searchBuilder = builder(searchBuilder);
            SearchRequestItem requestItem = searchBuilder.GetRequestItem();

            _apiClient.apiUrl = _elasticApiRoute + "/" + ApiConstants.DataSearchApi_Urls.READ_ACTION_NAME;
            return _apiClient.PostRequest<ServiceResult<DataSearchResult<T>>>(requestItem).Result;
        }

    }
}
