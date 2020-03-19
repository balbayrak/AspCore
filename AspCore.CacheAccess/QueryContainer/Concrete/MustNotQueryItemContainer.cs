namespace AspCore.CacheAccess.QueryContiner.Concrete
{
    public class MustNotQueryItemContainer : QueryItemContainer
    {
        public override BoolQueryContainer container { get; set; }

        public MustNotQueryItemContainer()
        {
            this.container = new BoolQueryContainer();
        }
    }
}
