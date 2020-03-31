using AspCore.CacheEntityClient.QueryBuilder.Concrete;
using AspCore.Entities.Cache;
using AspCore.Entities.Constants;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using System;

namespace AspCore.CacheEntityClient
{
    internal class CacheClient<T> : ReadOnlyCacheClient<T>, ICacheClient<T>
        where T : class, ICacheEntity, new()
    {
        public CacheClient(string apiClientKey, string cacheKey, string cacheApiRoute) : base(apiClientKey,cacheKey,cacheApiRoute)
        {
           
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
