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

        bool SetObject<T>(string key, T obj, bool? sameSiteStrict = null, DateTime? expires = null);
    }
}
