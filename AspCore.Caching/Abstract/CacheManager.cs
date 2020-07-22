using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AspCore.Caching.Abstract
{
    public abstract class CacheManager
    {
        protected string uniqueCacheKey { get; private set; }

        protected CacheManager()
        {
            uniqueCacheKey = Guid.NewGuid().ToString("N");
        }

        public abstract T GetObject<T>(string key);
        public abstract bool SetObject<T>(string key, T obj, DateTime? expires = null, bool? sameSiteStrict = null);
        public abstract bool Remove(string key);
        public abstract bool RemoveAll();

        public async virtual Task<bool> RemoveAsync(string key)
        {
            var data = await Task.Run((() => Remove(key)));
            return data;
        }

        public async virtual Task<bool> RemoveAllAsync()
        {
            var data = await Task.Run((() => RemoveAll()));
            return data;
        }

        public async virtual Task<T> GetObjectAsync<T>(string key)
        {
            var data = await Task.Run((() => GetObject<T>(key)));
            return data;
        }

        public async virtual Task<bool> SetObjectAsync<T>(string key, T obj, DateTime? expires = null, bool? sameSiteStrict = null)
        {
            var data = await Task.Run((() => SetObject(key, obj, expires, sameSiteStrict)));
            return data;

        }
    }
}
