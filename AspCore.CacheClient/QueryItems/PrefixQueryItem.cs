using AspCore.CacheClient.QueryBuilder.Concrete;
using System;

namespace AspCore.CacheClient.QueryItems
{
    /// <summary>
    /// Prefix Query verilen değer ile başlayan ifadeleri getirir. Analiz edilmez doğrudan girilen değeri arama yapar.
    /// </summary>
   
    public class PrefixQueryItem :  QueryItem
    {
        public string Value { get; set; }

        public PrefixQueryItem(string fieldDescriptor, string value) : base(fieldDescriptor)
        {
            this.Value = value;
        }
    }
}
