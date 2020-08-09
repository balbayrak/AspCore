using AspCore.Storage.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.Storage.Concrete
{
    public class StorageService 
    {
        public ICookieService CookieService { get; private set; }

        public ICacheService CacheService { get; private set; }

        public StorageService(ICookieService cookieService, ICacheService cacheService)
        {
            CookieService = cookieService;
            CacheService = cacheService;

        }
    }
}
