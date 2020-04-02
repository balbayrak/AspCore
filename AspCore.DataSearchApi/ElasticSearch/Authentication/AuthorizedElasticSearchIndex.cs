namespace AspCore.DataSearchApi.ElasticSearch.Authentication
{
    public class AuthorizedElasticSearchIndex
    {
        public string indexKey { get; set; }
        public string[] actions { get; set; }
    }
}
