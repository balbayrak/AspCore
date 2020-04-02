using AspCore.ElasticSearchApiClient.QueryBuilder.Abstract;

namespace AspCore.ElasticSearchApiClient.QueryBuilder.Concrete
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
