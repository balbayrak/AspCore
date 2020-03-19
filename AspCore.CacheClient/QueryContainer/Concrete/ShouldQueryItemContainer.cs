namespace AspCore.CacheClient.QueryContiner.Concrete
{
    public class ShouldQueryItemContainer : QueryItemContainer
    {
        public override BoolQueryContainer container { get; set; }

        public ShouldQueryItemContainer()
        {
            this.container = new BoolQueryContainer();
        }
    }
}
