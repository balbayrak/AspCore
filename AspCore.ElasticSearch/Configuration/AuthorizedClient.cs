namespace AspCore.ElasticSearch.Configuration
{
    public class AuthorizedClient
    {
        public AuthenticationOption ClientAuthenticationInfo { get; set; }

        public bool AuthorizedAllActions { get; set; }

        public string[] AuthorizedActions { get; set; }
    }
}
