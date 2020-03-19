using AspCore.CacheClient.QueryBuilder.Concrete;
using System;

namespace AspCore.CacheClient.QueryItems
{
    /// <summary>
    /// Match Query aranan kelime yada kelimeleri analiz eder, Büyük küçük harf farketmeden sonuç getirir.
    /// Kelimeler aynı sırada olmasa da arama sonucunda bulunur. Operation AND olarak seçilirse kelimelerin tümü aranır, OR olarak seçilirse herhangi biri aranır.
    /// </summary>
    public class QueryStringQueryItem: QueryItem
    {
        public string[] fields { get; set; }

        public object Value { get; set; }

        public string MinimumShouldMatchPercentage { get; set; }

        public string MinimumShouldMatchFixed { get; set; }

        public EnumMultiQueryOperation? operation { get; set; }


        public QueryStringQueryItem(string[] fields, object value, string minimumShouldMatchPercentage, string minimumShouldMatchFixed, EnumMultiQueryOperation? operation) : base()
        {
            this.fields = fields;
            this.Value = value;
            this.MinimumShouldMatchPercentage = minimumShouldMatchPercentage;
            this.MinimumShouldMatchFixed = minimumShouldMatchFixed;
            this.operation = operation;
        }
    }
}
