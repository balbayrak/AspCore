using AspCore.CacheAccess.QueryBuilder.Concrete;
using System;

namespace AspCore.CacheAccess.QueryItems
{
    /// <summary>
    /// Exist Query verilen field'in index içerisinde bulunup bulunmadığını return eder.
    /// </summary>

    public class ExistQueryItem : QueryItem
    {
        public ExistQueryItem(string field) : base(field)
        {

        }
    }
}
