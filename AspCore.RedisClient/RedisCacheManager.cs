﻿using AspCore.Caching.Abstract;
using AspCore.Utilities;
using Newtonsoft.Json;
using ServiceStack.Redis;
using System;

namespace AspCore.RedisClient
{
    public class RedisCacheManager : CacheManager, ICacheService
    {
        private readonly IRedisClientsManager _redisClientsManager;

        public RedisCacheManager(IRedisClientsManager redisClientsManager)
        {
            _redisClientsManager = redisClientsManager;
        }

        public bool ExpireEntryIn(string key, TimeSpan timeSpan)
        {
            if (key == null)
            {
                return false;
            }
            else
            {
                key = $"{uniqueCacheKey}_{key}";
                using (IRedisClient redis = _redisClientsManager.GetClient())
                {
                    if (redis.ExpireEntryIn(key, timeSpan))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        public T GetObject<T>(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                key = $"{uniqueCacheKey}_{key}";
                using (IRedisClient redis = _redisClientsManager.GetClient())
                {
                    string entity = redis.Get<string>(key);
                    if (!string.IsNullOrEmpty(entity))
                    {
                        return JsonConvert.DeserializeObject<T>(entity.UnCompressString());
                    }
                }
            }
            return default(T);
        }

        public bool Remove(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                key = $"{uniqueCacheKey}_{key}";
                using (IRedisClient redis = _redisClientsManager.GetClient())
                {
                    return redis.Remove(key);
                }
            }

            return false;
        }

        public void RemoveAll()
        {
            using (IRedisClient redis = _redisClientsManager.GetClient())
            {
                redis.RemoveByPattern($"{uniqueCacheKey}*");
            }
        }

        public bool SetObject<T>(string key, T obj, DateTime? expires = null, bool? sameSiteStrict = null)
        {
            if (!string.IsNullOrEmpty(key))
            {
                key = $"{uniqueCacheKey}_{key}";
                Remove(key);

                expires = expires ?? DateTime.UtcNow.AddMinutes(30);

                JsonSerializerSettings serializerSettings = new JsonSerializerSettings();
                serializerSettings.NullValueHandling = NullValueHandling.Ignore;
                string json = JsonConvert.SerializeObject(obj, Formatting.None, serializerSettings).CompressString();
                using (IRedisClient redis = _redisClientsManager.GetClient())
                {
                    return redis.Set(key, json, expires.Value);
                }
            }
            return false;
        }
    }
}