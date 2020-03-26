using AspCore.Caching.Abstract;
using AspCore.Caching.Concrete;
using AspCore.Entities.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AspCore.ApiClient.Configuration
{
    public class ApiClientCacheBuilder : ConfigurationOption, IDisposable
    {
        public ApiClientCacheBuilder(IServiceCollection services) : base(services)
        {}

        public ApiClientOptionBuilder AddMemoryCache()
        {
            var httpContextAccessor = services.FirstOrDefault(d => d.ServiceType == typeof(IHttpContextAccessor));
            if (httpContextAccessor == null)
            {
                services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            }

            services.AddMemoryCache();
            services.AddSingleton<ICacheService, MemoryCacheManager>();

            return new ApiClientOptionBuilder(services);
        }

        public ApiClientOptionBuilder AddCustomCacheService<T>()
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

            return new ApiClientOptionBuilder(services);
        }

        public void Dispose()
        {}
    }
}
