using AspCore.Entities.Configuration;

namespace AspCore.CacheEntityAccess.Configuration
{
    public interface ICacheApiOption : IConfigurationEntity
    {
        ElasticSearchProvider ElasticSearchProvider { get; set; }
        CacheNode[] CacheNodes { get; set; }
        AuthorizedClient[] AuthorizedClients { get; set; }
    }
}
