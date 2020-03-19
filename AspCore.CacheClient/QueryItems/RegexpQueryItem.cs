using AspCore.CacheClient.QueryBuilder.Concrete;
using System;

namespace AspCore.CacheClient.QueryItems
{
    public class RegexpQueryItem : QueryItem
    {
        public string Value { get; set; }

        public RegexpQueryItem(string fieldDescriptor, string value) : base(fieldDescriptor)
        {
            this.Value = value;
        }
    }
}
