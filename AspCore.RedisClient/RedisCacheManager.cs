using AspCore.Caching.Abstract;
using AspCore.Utilities;
using AspCore.Utilities.DataProtector;
using Newtonsoft.Json;
using ServiceStack.Redis;
using System;
using System.Threading.Tasks;

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
                        var unProtectJson = DataProtectorFactory.Instance.UnProtect(entity);
                        return JsonConvert.DeserializeObject<T>(unProtectJson.UnCompressString());
                    }
                }
            }
            return default(T);
        }

        public async Task<T> GetObjectAsync<T>(string key)
        {
            var data = await Task.Run((() => GetObject<T>(key)));
            return data;
        }


        public async Task<bool> SetObjectAsync<T>(string key, T obj, DateTime? expires = null, bool? sameSiteStrict = null)
        {
            var data = await Task.Run((() => SetObject(key, obj, expires, sameSiteStrict)));
            return data;

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
                var protectJson = DataProtectorFactory.Instance.Protect(json);

                using (IRedisClient redis = _redisClientsManager.GetClient())
                {
                    return redis.Set(key, protectJson, expires.Value);
                }
            }
            return false;
        }
    }
}
