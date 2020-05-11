using Microsoft.Extensions.DependencyInjection;
using System;
using AspCore.Entities.Configuration;
using AspCore.Caching.Configuration;
using Microsoft.AspNetCore.Http;
using System.Linq;
using AspCore.Caching.Abstract;
using AspCore.Caching.Concrete;

namespace AspCore.Web.Configuration.Options
{
    public class CacheOptionConfiguration : ConfigurationOption
    {
        public CacheOptionConfiguration(IServiceCollection services) : base(services)
        {
        }

        public ApiClientConfigurationOption AddCacheService(Action<CacheOptionBuilder> option)
        {
            var httpContextAccessor = services.FirstOrDefault(d => d.ServiceType == typeof(IHttpContextAccessor));
            if (httpContextAccessor == null)
            {
                services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            }

            services.AddSingleton<ICookieService, CookieCacheManager>();

            var cacheStorageOptionBuilder = new CacheOptionBuilder(services);
            option(cacheStorageOptionBuilder);

            return new ApiClientConfigurationOption(services);
        }
    }
}
