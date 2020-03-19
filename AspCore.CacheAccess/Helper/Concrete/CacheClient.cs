using AspCore.ApiClient.Abstract;
using AspCore.ApiClient.Entities.Concrete;
using AspCore.CacheAccess.General;
using AspCore.CacheAccess.Helper.Abstract;
using AspCore.CacheAccess.QueryBuilder.Concrete;
using AspCore.CacheAccess.QueryResult;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using System;
using System.Collections.Generic;

namespace AspCore.CacheAccess.Helper.Concrete
{
    public class CacheClient<T> : ICacheClient<T>
        where T : class, IEntity, new()
    {
        private IAuthenticatedApiClient _apiClient { get; set; }
        private string _cacheKey { get; }

        public CacheClient(string apiClientKey, string cacheKey)
        {
            _apiClient = ApiClientFactory.GetApiClient(apiClientKey);

            if (!cacheKey.StartsWith("/"))
            {
                cacheKey = "/" + cacheKey;
            }
            _cacheKey = cacheKey;
        }

        public ServiceResult<bool> Create(params T[] cacheItems)
        {
            _apiClient.apiUrl = _cacheKey + "/" + CacheClientConstants.CacheApiActions.CREATE_ACTION_NAME;
            return _apiClient.PostRequest<ServiceResult<bool>>(cacheItems).Result;
        }

        public ServiceResult<CacheResult<T>> Read(Func<CacheSearchBuilder<T>, CacheSearchBuilder<T>> builder)
        {
            CacheSearchBuilder<T> searchBuilder = new CacheSearchBuilder<T>();
            searchBuilder = builder(searchBuilder);
            SearchRequestItem requestItem = searchBuilder.GetRequestItem(_cacheKey);

            _apiClient.apiUrl = _cacheKey + "/" + CacheClientConstants.CacheApiActions.READ_ACTION_NAME;
            return _apiClient.PostRequest<ServiceResult<CacheResult<T>>>(requestItem).Result;
        }

        public ServiceResult<CacheResult<T>> Read(T cacheItem)
        {
            _apiClient.apiUrl = _cacheKey + "/" + CacheClientConstants.CacheApiActions.GETDATA_ACTION_NAME;
            return _apiClient.PostRequest<ServiceResult<CacheResult<T>>>(cacheItem).Result;
        }

        public ServiceResult<bool> Update(params T[] cacheItems)
        {
            _apiClient.apiUrl = _cacheKey + "/" + CacheClientConstants.CacheApiActions.UPDATE_ACTION_NAME;
            return _apiClient.PostRequest<ServiceResult<bool>>(cacheItems).Result;
        }

        public ServiceResult<bool> Delete(params T[] cacheItems)
        {
            _apiClient.apiUrl = _cacheKey + "/" + CacheClientConstants.CacheApiActions.DELETE_ACTION_NAME;
            return _apiClient.PostRequest<ServiceResult<bool>>(cacheItems).Result;
        }

        public ServiceResult<CacheResult<T>> MinMax(T cacheItem)
        {
            _apiClient.apiUrl = _cacheKey + "/" + CacheClientConstants.CacheApiActions.MIN_MAX_ACTION_NAME;
            return _apiClient.PostRequest<ServiceResult<CacheResult<T>>>(cacheItem).Result;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
