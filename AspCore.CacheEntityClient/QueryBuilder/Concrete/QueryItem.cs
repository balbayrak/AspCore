using AspCore.CacheEntityClient.QueryBuilder.Abstract;

namespace AspCore.CacheEntityClient.QueryBuilder.Concrete
{
    public abstract class QueryItem : IQueryItem
    {
        public string FieldName { get; set; }

        public QueryItem()
        {
            this.FieldName = null;
        }

        public QueryItem(string fieldName)
        {
            this.FieldName = fieldName;
        }
    }
}
