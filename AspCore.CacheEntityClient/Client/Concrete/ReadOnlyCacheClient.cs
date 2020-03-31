using AspCore.ApiClient.Abstract;
using AspCore.ApiClient.Entities.Concrete;
using AspCore.CacheEntityClient.QueryBuilder.Concrete;
using AspCore.Entities.Cache;
using AspCore.Entities.Constants;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using System;

namespace AspCore.CacheEntityClient
{
    internal class ReadOnlyCacheClient<T> : IReadOnlyCacheClient<T>
        where T : class, ICacheEntity, new()
    {
        protected IAuthenticatedApiClient _apiClient { get; set; }
        public string cacheKey { get; private set; }
        protected string _cacheApiRoute { get; }

        public ReadOnlyCacheClient(string apiClientKey, string cacheKey, string cacheApiRoute)
        {
            _apiClient = ApiClientFactory.GetApiClient(apiClientKey);

            if (!cacheApiRoute.StartsWith("/"))
            {
                cacheApiRoute = "/" + cacheApiRoute;
            }
            _cacheApiRoute = cacheApiRoute;
            this.cacheKey = cacheKey;
        }

        public ServiceResult<CacheResult<T>> Read(Func<CacheSearchBuilder<T>, CacheSearchBuilder<T>> builder)
        {
            CacheSearchBuilder<T> searchBuilder = new CacheSearchBuilder<T>();
            searchBuilder = builder(searchBuilder);
            SearchRequestItem requestItem = searchBuilder.GetRequestItem(cacheKey);

            _apiClient.apiUrl = _cacheApiRoute + "/" + ApiConstants.CacheApi_Urls.READ_ACTION_NAME;
            return _apiClient.PostRequest<ServiceResult<CacheResult<T>>>(requestItem).Result;
        }

    }
}
