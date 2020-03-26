using AspCore.Caching.Abstract;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AspCore.Caching.Configuration
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigureCache(this IServiceCollection services, Action<CacheOptionBuilder> option)
        {
            using (CacheOptionBuilder cacheOptionBuilder = new CacheOptionBuilder(services))
            {
                option(cacheOptionBuilder);
            }

            return services;
        }

        public static IServiceCollection ConfigureApiClientWithCustomCacheService<TCacheService>(this IServiceCollection services, Action<CacheOptionBuilder> option)
             where TCacheService : class, ICacheService, new()
        {
            using (CacheOptionBuilder cacheOptionBuilder = new CacheOptionBuilder(services))
            {
                option(cacheOptionBuilder);
            }

            return services;
        }
    }
}
