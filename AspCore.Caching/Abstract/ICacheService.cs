using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.Caching.Abstract
{
    public interface ICacheService
    {
        T GetObject<T>(string key);

        bool SetObject<T>(string key, T obj, DateTime? expires = null, bool? sameSiteStrict = null);

        bool Remove(string key);

        bool ExpireEntryIn(string key, TimeSpan timeSpan);

        void RemoveAll();
    }
}
