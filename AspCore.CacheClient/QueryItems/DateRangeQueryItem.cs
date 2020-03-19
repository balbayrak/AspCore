using AspCore.CacheClient.QueryBuilder.Concrete;
using System;

namespace AspCore.CacheClient.QueryItems
{
    /// <summary>
    /// Date Range Query verilen tarih aralığında sonuç getirir, seçilen field DateTime olmak zorundadir.
    /// </summary>

    public class DateRangeQueryItem : QueryItem
    {
        public DateTime? LessThan { get; set; }

        public DateTime? GreaterThan { get; set; }

        public DateRangeQueryItem(string fieldDescriptor, DateTime? lessThan, DateTime? greaterThan) : base(fieldDescriptor)
        {
            this.LessThan = lessThan;
            this.GreaterThan = greaterThan;
        }
    }
}
