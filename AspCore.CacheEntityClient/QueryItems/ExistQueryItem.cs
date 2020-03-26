using AspCore.CacheEntityClient.QueryBuilder.Concrete;
using System;

namespace AspCore.CacheEntityClient.QueryItems
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
