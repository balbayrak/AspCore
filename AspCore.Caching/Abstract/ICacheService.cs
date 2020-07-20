using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AspCore.Caching.Abstract
{
    public interface ICacheService
    {
        T GetObject<T>(string key);
        Task<T> GetObjectAsync<T>(string key);

        bool SetObject<T>(string key, T obj, DateTime? expires = null, bool? sameSiteStrict = null);
        Task<bool> SetObjectAsync<T>(string key, T obj, DateTime? expires = null, bool? sameSiteStrict = null);

        bool Remove(string key);

        bool ExpireEntryIn(string key, TimeSpan timeSpan);

        void RemoveAll();
    }
}
