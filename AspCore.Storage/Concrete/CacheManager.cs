using AspCore.Storage.Abstract;
using AspCore.Storage.Extension;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;

namespace AspCore.Storage.Concrete
{
    public class CacheManager : ICacheService
    {
        private readonly IDistributedCache _cache;
        private string _uniqueKey;

        public CacheManager(IDistributedCache cache, Guid? uniqueKey)
        {
            _cache = cache;
            _uniqueKey = uniqueKey.HasValue ? uniqueKey.Value.ToString("N") : Guid.NewGuid().ToString("N");
        }

        public bool ExpireEntryIn(string key, TimeSpan timeSpan)
        {
            return true;
        }

        public T GetObject<T>(string key)
        {
            key = $"{_uniqueKey}_{key}";
            return _cache.GetValue<T>(key);
        }

        public async Task<T> GetObjectAsync<T>(string key)
        {
            key = $"{_uniqueKey}_{key}";
            var data = await _cache.GetValueAsync<T>(key);
            return data;
        }

        public async Task<bool> SetObjectAsync<T>(string key, T obj, DateTime? expires = null)
        {
            key = $"{_uniqueKey}_{key}";
            DistributedCacheEntryOptions cacheEntryOptions;
            if (expires.HasValue)
                cacheEntryOptions = new DistributedCacheEntryOptions().SetAbsoluteExpiration(expires.Value);
            else
                cacheEntryOptions = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1));
            return await _cache.SetValueAsync(key, obj, cacheEntryOptions);
        }

        public bool Remove(string key)
        {
            try
            {
                key = $"{_uniqueKey}_{key}";
                _cache.Remove(key);
                return true;
            }
            catch
            {
                // loglama
            }
            return false;
        }

        /// <summary>
        /// expires null olarak bırakıldığında default olarak 1 gün set edilir.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="expires"></param>
        /// <returns></returns>
        public bool SetObject<T>(string key, T obj, DateTime? expires = null)
        {
            try
            {
                key = $"{_uniqueKey}_{key}";
                Remove(key);

                DistributedCacheEntryOptions cacheEntryOptions;
                if (expires.HasValue)
                    cacheEntryOptions = new DistributedCacheEntryOptions().SetAbsoluteExpiration(expires.Value);
                else
                    cacheEntryOptions = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1));
                _cache.SetValue(key, obj, cacheEntryOptions);
                
                return true;
            }
            catch
            {
                // loglama
            }

            return false;
        }

        public async Task<bool> RemoveAsync(string key)
        {
            try
            {
                key = $"{_uniqueKey}_{key}";
                await _cache.RemoveAsync(key);
                return true;
            }
            catch
            {
                //loglama
            }
            return false;
        }
    }
}
