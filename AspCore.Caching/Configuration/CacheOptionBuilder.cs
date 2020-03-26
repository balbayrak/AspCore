using AspCore.Caching.Abstract;
using AspCore.Caching.Concrete;
using AspCore.Entities.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace AspCore.Caching.Configuration
{
    public class CacheOptionBuilder : ConfigurationOption, IDisposable
    {
        public CacheOptionBuilder(IServiceCollection services) :base(services)
        {
        }

        public void AddCookieCache()
        {
            var httpContextAccessor = services.FirstOrDefault(d => d.ServiceType == typeof(IHttpContextAccessor));
            if (httpContextAccessor == null)
            {
                services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            }

            services.AddSingleton<ICacheService, CookieCacheManager>();
        }

        public void AddMemoryCache()
        {
            services.AddMemoryCache();
            services.AddSingleton<ICacheService, MemoryCacheManager>();
        }

        public void AddCustomCache<T>()
        where T : class, ICacheService, new()
        {
            var httpContextAccessor = services.FirstOrDefault(d => d.ServiceType == typeof(IHttpContextAccessor));
            if (httpContextAccessor == null)
            {
                services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            }

            var accessTokenService = services.FirstOrDefault(d => d.ServiceType == typeof(ICacheService));
            if (accessTokenService == null)
            {
                services.Remove(accessTokenService);
            }

            services.AddSingleton<ICacheService, T>();
        }

        public void Dispose()
        {
        }
    }
}
