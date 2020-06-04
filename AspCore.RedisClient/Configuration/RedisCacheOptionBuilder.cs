using AspCore.Caching.Abstract;
using AspCore.Caching.Configuration;
using AspCore.ConfigurationAccess.Abstract;
using Microsoft.Extensions.DependencyInjection;
using ServiceStack.Redis;
using System;

namespace AspCore.RedisClient.Configuration
{
    public static class RedisCacheOptionBuilder
    {
        public static void AddRedisCache(this CacheOptionBuilder cacheOptionBuilder, Action<RedisCacheOption> option = null)
        {
            if (option != null)
            {
                RedisCacheOption redisCacheOption = new RedisCacheOption();
                option(redisCacheOption);

                ConfigureRedisOption(cacheOptionBuilder.services, redisCacheOption);

                cacheOptionBuilder.services.AddSingleton(typeof(ICacheService), sp =>
                {
                    IRedisClientsManager manager = sp.GetService<IRedisClientsManager>();
                    return new RedisCacheManager(manager);
                });
            }
        }

        public static void AddRedisCache(this CacheOptionBuilder cacheOptionBuilder, string configurationKey = null)
        {
            if (!string.IsNullOrEmpty(configurationKey))
            {
                cacheOptionBuilder.services.AddSingleton(typeof(IRedisClientsManager), sp =>
                {
                    var configurationAccessor = sp.GetRequiredService<IConfigurationAccessor>();
                    RedisCacheOption redisCacheOption = configurationAccessor.GetValueByKey<RedisCacheOption>(configurationKey);

                    if (redisCacheOption != null)
                    {
                        return ConfigureRedisOption(cacheOptionBuilder.services, redisCacheOption);
                    }
                    else return null;
                });

                cacheOptionBuilder.services.AddSingleton(typeof(ICacheService), sp =>
                {
                    IRedisClientsManager manager = sp.GetService<IRedisClientsManager>();
                    return new RedisCacheManager(manager);
                });
            }
        }

        private static IRedisClientsManager ConfigureRedisOption(IServiceCollection services, RedisCacheOption redisCacheOption)
        {
            if (redisCacheOption.servers.Length > 1)
            {
                var sentinel = new RedisSentinel(redisCacheOption.servers, redisCacheOption.masterName);
                sentinel.RedisManagerFactory = (master, slaves) => new RedisManagerPool(master);
                try
                {
                    return sentinel.Start();
                }
                catch(Exception ex)
                {
                    throw ex;
                }
               
            }
            else
            {
                return
                 new PooledRedisClientManager(redisCacheOption.servers)
                 {
                     ConnectTimeout = 100,
                 };
            }
        }
    }
}
