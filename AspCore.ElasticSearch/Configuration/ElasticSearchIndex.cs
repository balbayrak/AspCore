namespace AspCore.ElasticSearch.Configuration
{
    public class ElasticSearchIndex
    {
        public string IndexKey { get; set; }

        public string ApiControllerName { get; set; }

        public AuthorizedClient[] AuthorizedClients { get; set; }
    }
}
