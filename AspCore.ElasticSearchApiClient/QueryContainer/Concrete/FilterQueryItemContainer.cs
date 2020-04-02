namespace AspCore.ElasticSearchApiClient.QueryContiner.Concrete
{
    public class FilterQueryItemContainer : QueryItemContainer
    {
        public override BoolQueryContainer container { get; set; }

        public FilterQueryItemContainer()
        {
            this.container = new BoolQueryContainer();
        }
    }
}
