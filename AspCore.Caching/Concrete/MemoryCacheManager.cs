using AspCore.Caching.Abstract;
using Microsoft.Extensions.Caching.Memory;
using System;
using AspCore.Caching.Extension;

namespace AspCore.Caching.Concrete
{
    public class MemoryCacheManager : CacheManager, ICacheService
    {
        private IMemoryCache _MemCache;

        public MemoryCacheManager(IMemoryCache MemCache) : base()
        {
            _MemCache = MemCache;
        }

        public bool ExpireEntryIn(string key, TimeSpan timeSpan)
        {
            return true;
        }

        public T GetObject<T>(string key)
        {
            key = $"{uniqueCacheKey}_{key}";
            return _MemCache.GetValue<T>(key);
        }

        public bool Remove(string key)
        {
            try
            {
                key = $"{uniqueCacheKey}_{key}";
                _MemCache.Remove(key);
                return true;
            }
            catch
            {

            }

            return false;

        }

        public void RemoveAll()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// timespan null olarak bırakıldığında default olarak 1 gün set edilir.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        /// <param name="timeSpan"></param>
        /// <returns></returns>

        public bool SetObject<T>(string key, T obj, DateTime? expires = null, bool? sameSiteStrict = null)
        {
            try
            {
                key = $"{uniqueCacheKey}_{key}";
                Remove(key);

                MemoryCacheEntryOptions cacheEntryOptions = null;
                if (expires.HasValue)
                    cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(expires.Value.TimeOfDay);
                else
                    cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromDays(1));

                _MemCache.SetValue<T>(key, obj, cacheEntryOptions);
                return true;
            }
            catch
            {

            }
            return false;
        }
    }
}
