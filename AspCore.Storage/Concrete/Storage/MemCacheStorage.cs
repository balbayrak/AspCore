using Microsoft.Extensions.Caching.Memory;
using System;
using AspCore.Storage.Abstract;
using AspCore.Storage.Extension;

namespace AspCore.Storage.Concrete.Storage
{
    public class MemCacheStorage : IStorage
    {
        private IMemoryCache _MemCache;

        public MemCacheStorage(IMemoryCache MemCache)
        {
            _MemCache = MemCache;
        }

        public bool ExpireEntryIn(string key, TimeSpan timeSpan)
        {
            return true;
        }

        public T GetObject<T>(string key)
        {
            return _MemCache.GetValue<T>(key);
        }

        public bool Remove(string key)
        {
            try
            {
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
