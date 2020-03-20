using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using AspCore.Entities.Configuration;
using AspCore.Storage.Abstract;
using AspCore.Storage.Concrete.Storage;

namespace AspCore.ApiClient.Configuration
{
    public class ApiClientStorageBuilder : ConfigurationOption, IDisposable
    {
        public ApiClientStorageBuilder(IServiceCollection services) : base(services)
        {

        }
        public ApiClientOptionBuilder AddApiClientStorage(Action<ApiClientOption> option)
        {
            ApiClientOption apiClientOption = new ApiClientOption();
            option.Invoke(apiClientOption);

            var httpContextAccessor = services.FirstOrDefault(d => d.ServiceType == typeof(IHttpContextAccessor));
            if (httpContextAccessor == null)
            {
                services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            }

            if (apiClientOption.tokenStorage == EnumStorage.Cookie)
            {
                services.AddSingleton<IStorage, CookieStorage>();
            }
            else if (apiClientOption.tokenStorage == EnumStorage.MemoryCache)
            {
                services.AddMemoryCache();
                services.AddSingleton<IStorage, MemCacheStorage>();
            }

            return new ApiClientOptionBuilder(services);
        }

        public ApiClientOptionBuilder AddApiClientStorage<T>(Action<ApiClientOption> option)
        where T : class, IStorage, new()
        {
            var httpContextAccessor = services.FirstOrDefault(d => d.ServiceType == typeof(IHttpContextAccessor));
            if (httpContextAccessor == null)
            {
                services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            }

            var accessTokenService = services.FirstOrDefault(d => d.ServiceType == typeof(IStorage));
            if (accessTokenService == null)
            {
                services.Remove(accessTokenService);
            }

            services.AddSingleton<IStorage, T>();

            return new ApiClientOptionBuilder(services);
        }

        public void Dispose()
        {
            
        }
    }
}
