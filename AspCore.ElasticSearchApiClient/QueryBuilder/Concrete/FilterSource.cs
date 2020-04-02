namespace AspCore.ElasticSearchApiClient.QueryBuilder.Concrete
{
    public class FilterSource
    {
        public bool includeAll { get; set; }
        public bool excludeAll { get; set; }
        public string[] includeFields { get; set; }
        public string[] excludeFields { get; set; }

        public FilterSource()
        {
            includeAll = false;
            excludeAll = false;
            includeFields = null;
            excludeFields = null;
        }

        public FilterSource(bool include,bool exclude)
        {
            includeAll = include;
            excludeAll = exclude;
            includeFields = null;
            excludeFields = null;
        }
    }
}
