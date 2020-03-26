using AspCore.BackendForFrontend.Abstract;
using AspCore.Caching.Abstract;
using AspCore.Dependency.Concrete;
using AspCore.Entities.Authentication;
using AspCore.Entities.Constants;
using System;

namespace AspCore.BackendForFrontend.Concrete
{
    public abstract class BaseBffLayer : IBFFLayer
    {
        protected ICacheService _cache;

        public IBffApiClient apiClient { get; private set; }

        private string _apiClientKey { get; set; }
        protected string apiClientKey
        {
            get
            {
                return _apiClientKey;
            }
            set
            {
                _apiClientKey = value;
                apiClient.ChangeApiSettingsKey(value);
            }
        }

        private string _apiControllerRoute;
        protected string apiControllerRoute
        {
            get { return _apiControllerRoute; }
            set
            {
                if (!value.StartsWith("/"))
                {
                    value = "/" + value;
                }
                _apiControllerRoute = value;
            }
        }

        protected BaseBffLayer()
        {
            apiClient = DependencyResolver.Current.GetService<IBffApiClient>();

            _cache = DependencyResolver.Current.GetService<ICacheService>();

            string tokenStorageKey = _cache.GetObject<string>(ApiConstants.Api_Keys.CUSTOM_TOKEN_STORAGE_KEY);
           
            apiClient.tokenStorageKey = tokenStorageKey;
        }

        public void SetApiClientTokenStorageKey(string tokenStorageKey)
        {
            apiClient.tokenStorageKey = tokenStorageKey;
        }

        public void SetApiClientTokenStorageExpireTime(DateTime expireTime)
        {
            apiClient.tokenStrorageExpireTime = expireTime;
        }

        public void SetAuthenticationToken(string key, AuthenticationToken authenticationToken)
        {
            DateTime? expire = null;
            if (apiClient.tokenStrorageExpireTime == null || (apiClient.tokenStrorageExpireTime != null && (apiClient.tokenStrorageExpireTime == DateTime.MinValue || apiClient.tokenStrorageExpireTime == DateTime.MaxValue)))
            {
                if (authenticationToken.expires != DateTime.MinValue && authenticationToken.expires != DateTime.MaxValue)
                {
                    expire = authenticationToken.expires.AddMinutes(10);
                }
                else
                {
                    expire = null;
                }
            }
            else
            {
                if (authenticationToken.expires != DateTime.MinValue && authenticationToken.expires != DateTime.MaxValue)
                {
                    if (apiClient.tokenStrorageExpireTime < authenticationToken.expires)
                    {
                        expire = authenticationToken.expires.AddMinutes(10);
                    }
                }
            }

            _cache.SetObject(key, authenticationToken, expire, false);

        }
    }
}
