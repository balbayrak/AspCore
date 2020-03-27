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
    public class CacheClient<T> : ICacheClient<T>
        where T : class, ICacheEntity, new()
    {
        private IAuthenticatedApiClient _apiClient { get; set; }
        public string cacheKey { get; private set; }

        private string _cacheApiRoute { get; }

        public CacheClient(string apiClientKey, string cacheKey, string cacheApiRoute)
        {
            _apiClient = ApiClientFactory.GetApiClient(apiClientKey);

            if (!cacheApiRoute.StartsWith("/"))
            {
                cacheApiRoute = "/" + cacheApiRoute;
            }
            _cacheApiRoute = cacheKey;
        }

        public ServiceResult<bool> Create(params T[] cacheItems)
        {
            _apiClient.apiUrl = _cacheApiRoute + "/" + ApiConstants.CacheApi_Urls.CREATE_ACTION_NAME;
            return _apiClient.PostRequest<ServiceResult<bool>>(cacheItems).Result;
        }

        public ServiceResult<CacheResult<T>> Read(Func<CacheSearchBuilder<T>, CacheSearchBuilder<T>> builder)
        {
            CacheSearchBuilder<T> searchBuilder = new CacheSearchBuilder<T>();
            searchBuilder = builder(searchBuilder);
            SearchRequestItem requestItem = searchBuilder.GetRequestItem(cacheKey);

            _apiClient.apiUrl = _cacheApiRoute + "/" + ApiConstants.CacheApi_Urls.READ_ACTION_NAME;
            return _apiClient.PostRequest<ServiceResult<CacheResult<T>>>(requestItem).Result;
        }

        public ServiceResult<CacheResult<T>> Read(T cacheItem)
        {
            _apiClient.apiUrl = _cacheApiRoute + "/" + ApiConstants.CacheApi_Urls.GETDATA_ACTION_NAME;
            return _apiClient.PostRequest<ServiceResult<CacheResult<T>>>(cacheItem).Result;
        }

        public ServiceResult<bool> Update(params T[] cacheItems)
        {
            _apiClient.apiUrl = _cacheApiRoute + "/" + ApiConstants.CacheApi_Urls.UPDATE_ACTION_NAME;
            return _apiClient.PostRequest<ServiceResult<bool>>(cacheItems).Result;
        }

        public ServiceResult<bool> Delete(params T[] cacheItems)
        {
            _apiClient.apiUrl = _cacheApiRoute + "/" + ApiConstants.CacheApi_Urls.DELETE_ACTION_NAME;
            return _apiClient.PostRequest<ServiceResult<bool>>(cacheItems).Result;
        }

        public ServiceResult<MinMax> MinMax(T cacheItem)
        {
            _apiClient.apiUrl = _cacheApiRoute + "/" + ApiConstants.CacheApi_Urls.MIN_MAX_ACTION_NAME;
            return _apiClient.PostRequest<ServiceResult<MinMax>>(cacheItem).Result;
        }
    }
}
