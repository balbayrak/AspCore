namespace AspCore.ElasticSearchApiClient.QueryContiner.Concrete
{
    public abstract class QueryItemContainer
    {
        public abstract BoolQueryContainer container { get; set; }
    }
}
