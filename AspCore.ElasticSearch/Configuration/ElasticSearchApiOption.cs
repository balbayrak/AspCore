using AspCore.Entities.Configuration;

namespace AspCore.ElasticSearch.Configuration
{
    public class ElasticSearchApiOption : IElasticSearchOption
    {
        public ElasticSearchServer[] Servers { get; set; }
        public ElasticSearchIndex[] ElasticSearchIndices { get; set; }
        public AuthorizedClient[] AuthorizedClients { get; set; }
    }
}
