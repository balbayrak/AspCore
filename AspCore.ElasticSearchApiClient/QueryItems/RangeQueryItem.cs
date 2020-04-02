using AspCore.ElasticSearchApiClient.QueryBuilder.Concrete;
using System;

namespace AspCore.ElasticSearchApiClient.QueryItems
{
    /// <summary>
    /// Range Query aranan ifadeyi verilen aralıkta arar, field tipi numeric olmak zorundadır.
    /// </summary>
    
    public class RangeQueryItem : QueryItem
    {
        public double? LessThan { get; set; }
        public double? GreaterThan { get; set; }

        public RangeQueryItem(string fieldDescriptor, double? lessThan, double? greaterThan) : base(fieldDescriptor)
        {
            this.LessThan = lessThan;
            this.GreaterThan = greaterThan;
        }
    }
}
