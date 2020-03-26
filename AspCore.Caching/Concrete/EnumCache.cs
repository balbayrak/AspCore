using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.Caching.Concrete
{
    public enum EnumCache
    {
        MemoryCache = 1,
        Cookie = 2,
        Redis = 3,
        Database = 4,
        Session = 5,
    }
}
