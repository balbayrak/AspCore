using AspCore.CacheEntityClient.QueryBuilder.Concrete;
using System;

namespace AspCore.CacheEntityClient.QueryItems
{
    /// <summary>
    /// Match Phrase Prefix verilen değer ile başlayan ifadeleri getirir. Değer analiz edilir.
    /// </summary>
    /// <typeparam name="T">item sinifi</typeparam>
    public class MatchPhrasePrefixQueryItem : QueryItem
    {
        public string Value { get; set; }

        public MatchPhrasePrefixQueryItem(string fieldDescriptor, string value) : base(fieldDescriptor)
        {
            this.Value = value;
        }
    }
}
