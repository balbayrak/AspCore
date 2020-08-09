using AspCore.ConfigurationAccess.Abstract;
using AspCore.Entities.Configuration;
using AspCore.Storage.Abstract;
using AspCore.Storage.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace AspCore.Storage.Configuration
{
    public class CacheOptionBuilder : ConfigurationOption, IDisposable
    {
        public CacheOptionBuilder(IServiceCollection services) : base(services)
        {
        }


        public void AddMemoryCache(Action<StorageOption> option = null)
        {
            StorageOption storageOption = null;
            option?.Invoke(storageOption);
            services.AddDistributedMemoryCache();

            services.AddSingleton<ICacheService>(sp =>
            {
                var distributedCache = sp.GetRequiredService<IDistributedCache>();
                Guid? uniqueKey = storageOption == null ? null : storageOption.uniqueKey;

                return new CacheManager(distributedCache, uniqueKey);
            });
        }

        public void AddRedisCache(Action<RedisCacheOption> option)
        {
            if (option != null)
            {
                RedisCacheOption redisCacheOption = new RedisCacheOption();
                option(redisCacheOption);

                AddRedisCache(redisCacheOption, null);
            }
        }

        public void AddRedisCache(string configurationKey)
        {
            AddRedisCache(null, configurationKey);
        }

        private void AddRedisCache(RedisCacheOption cacheOption, string configurationKey)
        {
            if (!string.IsNullOrEmpty(configurationKey))
            {
                services.AddSingleton<RedisCacheBuilder>(sp =>
                {
                    var configurationAccessor = sp.GetRequiredService<IConfigurationAccessor>();
                    return new RedisCacheBuilder(configurationAccessor, configurationKey);

                });
            }
            else if (cacheOption != null)
            {
                services.AddSingleton(sp =>
                {
                    return new RedisCacheBuilder(cacheOption);
                });
            }
            services.AddDistributedRedisCache(option =>
            {
                option.InstanceName = "";
            });
            services.ConfigureOptions<ConfigureRedisCacheOption>();

            services.AddSingleton(typeof(ICacheService), sp =>
            {
                var redisCacheBuilder = sp.GetRequiredService<RedisCacheBuilder>();
                var distributedCache = sp.GetRequiredService<IDistributedCache>();
                return new RedisCacheManager(distributedCache, redisCacheBuilder.cacheOption.uniqueKey ?? Guid.NewGuid());

            });

        }

        public void AddCustomCache<T>()
        where T : class, ICacheService, new()
        {
            var httpContextAccessor = services.FirstOrDefault(d => d.ServiceType == typeof(IHttpContextAccessor));
            if (httpContextAccessor == null)
            {
                services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            }

            services.AddSingleton<ICacheService, T>();
        }

        public void Dispose()
        {
        }
    }
}
