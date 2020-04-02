using AspCore.ElasticSearchApiClient.QueryBuilder.Concrete;
using System;
using System.Collections.Generic;

namespace AspCore.ElasticSearchApiClient.QueryItems
{
    public class TermsQueryItem : QueryItem
    {
        public IEnumerable<object> values { get; set; }

        public TermsQueryItem()
        { }

        public TermsQueryItem(string fieldDescriptor, IEnumerable<object> values) : base(fieldDescriptor)
        {
            this.values = values;
        }
    }
}
