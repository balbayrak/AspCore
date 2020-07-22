using AspCore.BackendForFrontend.Abstract;
using AspCore.Caching.Abstract;
using AspCore.Entities.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AspCore.BackendForFrontend.Concrete
{
    public abstract class BaseBffLayer : IBffLayer
    {
        protected ICacheService CacheService;

        protected IApplicationCachedClient ApplicationCachedClient;
       
        public IBffApiClient ApiClient { get; private set; }

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
                ApiClient.ChangeApiSettingsKey(value);
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

        protected IServiceProvider ServiceProvider { get; private set; }

        protected BaseBffLayer(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;

            ApiClient = ServiceProvider.GetRequiredService<IBffApiClient>();

            CacheService = ServiceProvider.GetRequiredService<ICacheService>();

            ApplicationCachedClient = ServiceProvider.GetRequiredService<IApplicationCachedClient>();

            ApiClient.tokenStorageKey = ApplicationCachedClient.ApplicationUserKey;
        }

        public void SetApiClientTokenStorageKey(string tokenStorageKey)
        {
            ApiClient.tokenStorageKey = tokenStorageKey;
        }

        public void SetApiClientTokenStorageExpireTime(DateTime expireTime)
        {
            ApiClient.tokenStrorageExpireTime = expireTime;
        }

        public void SetAuthenticationToken(string key, AuthenticationToken authenticationToken)
        {
            DateTime? expire = null;
            if (ApiClient.tokenStrorageExpireTime == null || (ApiClient.tokenStrorageExpireTime != null && (ApiClient.tokenStrorageExpireTime == DateTime.MinValue || ApiClient.tokenStrorageExpireTime == DateTime.MaxValue)))
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
                    if (ApiClient.tokenStrorageExpireTime < authenticationToken.expires)
                    {
                        expire = authenticationToken.expires.AddMinutes(10);
                    }
                }
            }

            CacheService.SetObjectAsync(key, authenticationToken, expire, false);

        }
    }
}
