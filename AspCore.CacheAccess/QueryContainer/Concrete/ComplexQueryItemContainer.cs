using AspCore.CacheAccess.QueryContiner.Abstract;

namespace AspCore.CacheAccess.QueryContiner.Concrete
{
    public class ComplexQueryItemContainer : IComplexQueryItemContainer
    {
        public ComplexQueryItemContainer()
        {
        }

        public ShouldQueryItemContainer shouldQueryContainer { get; set; }
        public MustQueryItemContainer mustQueryContainer { get; set; }
        public MustNotQueryItemContainer mustNotQueryContainer { get; set; }
        public FilterQueryItemContainer filterQueryContainer { get; set; }
    }
}
