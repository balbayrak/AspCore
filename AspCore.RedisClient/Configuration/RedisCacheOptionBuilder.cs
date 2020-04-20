using AspCore.Caching.Abstract;
using AspCore.Caching.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.RedisClient.Configuration
{
    public static class RedisCacheOptionBuilder
    {
        public static void AddRedisCache(this CacheOptionBuilder cacheOptionBuilder, Action<RedisCacheOption> option)
        {
            RedisCacheOption redisCacheOption = new RedisCacheOption();
            option(redisCacheOption);

            if (redisCacheOption.servers.Length > 1)
            {
                var sentinel = new RedisSentinel(redisCacheOption.servers, redisCacheOption.masterName);
                sentinel.RedisManagerFactory = (master, slaves) => new RedisManagerPool(master);
                cacheOptionBuilder.services.AddSingleton<IRedisClientsManager>(c => sentinel.Start());
            }
            else
            {
                cacheOptionBuilder.services.AddSingleton<IRedisClientsManager>(c =>
                 new PooledRedisClientManager(redisCacheOption.servers)
                 {
                     ConnectTimeout = 100,
                 });
            }

            cacheOptionBuilder.services.AddSingleton(typeof(ICacheService), sp =>
            {
                IRedisClientsManager manager = sp.GetService<IRedisClientsManager>();
                return new RedisCacheManager(manager);
            });
        }
    }
}
