using AspCore.CacheAccess.QueryBuilder.Concrete;
using System;

namespace AspCore.CacheAccess.QueryItems
{
    /// <summary>
    /// Match Query aranan kelime yada kelimeleri analiz eder, Büyük küçük harf farketmeden sonuç getirir. Kelimeler aynı sırada arama yapılır.
    /// </summary>
 
    public class MatchPhraseQueryItem: QueryItem
    {
        public string Value { get; set; }
        public MatchPhraseQueryItem(string fieldDescriptor, string value) : base(fieldDescriptor)
        {
            this.Value = value;
        }
    }
}
