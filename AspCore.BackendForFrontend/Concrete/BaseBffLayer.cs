using System;
using AspCore.ApiClient.Entities.Concrete;
using AspCore.BackendForFrontend.Abstract;
using AspCore.Dependency.Concrete;
using AspCore.Entities.Constants;
using AspCore.Storage.Abstract;

namespace AspCore.BackendForFrontend.Concrete
{
    public abstract class BaseBffLayer : IBFFLayer
    {
        protected IStorage _storage;

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

            _storage = DependencyResolver.Current.GetService<IStorage>();

            string tokenStorageKey = _storage.GetObject<string>(ApiConstants.Api_Keys.CUSTOM_TOKEN_STORAGE_KEY);
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

        public void SetAuthenticationToken(string key, AuthenticationTokenResponse authenticationToken)
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

            _storage.SetObject(key, authenticationToken, expire, false);

        }
    }
}
