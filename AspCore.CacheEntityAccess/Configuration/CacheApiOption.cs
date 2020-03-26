using AspCore.Entities.Configuration;

namespace AspCore.CacheEntityAccess.Configuration
{
    public class CacheApiOption : ICacheApiOption
    {
        public CacheNode[] CacheNodes { get; set; }
        public AuthorizedClient[] AuthorizedClients { get; set; }
        public ElasticSearchProvider ElasticSearchProvider { get; set; }
    }
}
