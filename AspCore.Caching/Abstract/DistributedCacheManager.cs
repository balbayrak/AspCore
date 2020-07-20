using AspCore.Caching.Extension;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;

namespace AspCore.Caching.Abstract
{
    public abstract class DistributedCacheManager
    {
        private readonly IDistributedCache _cache;
        protected string UniqueCacheKey { get; }

        protected DistributedCacheManager(IDistributedCache cache)
        {
            _cache = cache;
            UniqueCacheKey = Guid.NewGuid().ToString("N");
        }


        public bool ExpireEntryIn(string key, TimeSpan timeSpan)
        {
            return true;
        }

        public T GetObject<T>(string key)
        {
            key = $"{UniqueCacheKey}_{key}";
            return _cache.GetValue<T>(key);
        }

        public async Task<T> GetObjectAsync<T>(string key)
        {
            key = $"{UniqueCacheKey}_{key}";
            var data = await _cache.GetValueAsync<T>(key);
            return data;
        }


        public async Task<bool> SetObjectAsync<T>(string key, T obj, DateTime? expires = null, bool? sameSiteStrict = null)
        {
            DistributedCacheEntryOptions cacheEntryOptions;
            if (expires.HasValue)
                cacheEntryOptions = new DistributedCacheEntryOptions().SetAbsoluteExpiration(expires.Value);
            else
                cacheEntryOptions = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1));
            return await _cache.SetValueAsync(key, obj,cacheEntryOptions);
        }

        public bool Remove(string key)
        {
            try
            {
                key = $"{UniqueCacheKey}_{key}";
                _cache.Remove(key);
                return true;
            }
            catch
            {
                // ignored
            }

            return false;

        }

        public void RemoveAll()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// expires null olarak bırakıldığında default olarak 1 gün set edilir.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="expires"></param>
        /// <param name="sameSiteStrict"></param>
        /// <returns></returns>
        public bool SetObject<T>(string key, T obj, DateTime? expires = null, bool? sameSiteStrict = null)
        {
            try
            {
                key = $"{UniqueCacheKey}_{key}";
                Remove(key);

                DistributedCacheEntryOptions cacheEntryOptions;
                if (expires.HasValue)
                    cacheEntryOptions = new DistributedCacheEntryOptions().SetAbsoluteExpiration(expires.Value);
                else
                    cacheEntryOptions = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1));
                _cache.SetValue(key, obj,cacheEntryOptions);
                return true;
            }
            catch
            {
                // ignored
            }

            return false;
        }
    }
}

