using AspCore.CacheAccess.QueryBuilder.Concrete;
using System;

namespace AspCore.CacheAccess.QueryItems
{
    /// <summary>
    /// Term Query exact value çalışır,ifadenin tam olarak aynısı arar, büyük harf ile yazılmış bir ifadeyi küçük olarak bulamaz.
    /// </summary>
    public class TermQueryItem : QueryItem
    {
        public object Value { get; set; }

        public TermQueryItem()
        { }

        public TermQueryItem(string fieldDescriptor, object value) : base(fieldDescriptor)
        {
            this.Value = value;
        }
    }
}
