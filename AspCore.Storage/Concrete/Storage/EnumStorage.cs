using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.Storage.Concrete.Storage
{
    public enum EnumStorage
    {
        MemoryCache = 1,
        Cookie = 2,
        Redis = 3,
        Database = 4,
        Session = 5,
    }
}
