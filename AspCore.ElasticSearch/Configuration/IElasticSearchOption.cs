using AspCore.Entities.Configuration;

namespace AspCore.ElasticSearch.Configuration
{
    public interface IElasticSearchOption : IConfigurationEntity
    {
        ElasticSearchServer[] Servers { get; set; }
        ElasticSearchIndex[] ElasticSearchIndices { get; set; }
        AuthorizedClient[] AuthorizedClients { get; set; }
    }
}
