using AspCore.ElasticSearchApiClient.QueryBuilder.Concrete;
using System;

namespace AspCore.ElasticSearchApiClient.QueryItems
{
    /// <summary>
    /// WilcardQueryItem kelime araması yapar. * karakteri birden fazla gelebilecek karakter olduğunu ifade eder. ? karakteri ise herhangi bir karakter gelebileceğini ifade eder.
    /// </summary>
    public class WildcardQueryItem : QueryItem
    {
        public object Value { get; set; }

        public WildcardQueryItem(string fieldDescriptor, object value) : base(fieldDescriptor)
        {
            this.Value = value;
        }
    }
}
