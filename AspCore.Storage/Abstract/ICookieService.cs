using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.Storage.Abstract
{
    public interface ICookieService
    {
        T GetObject<T>(string key);

        void Remove(string key);

        void RemoveAll();

        bool SetObject<T>(string key, T obj, DateTime? expires = null, bool? sameSiteStrict = null);
    }
}
