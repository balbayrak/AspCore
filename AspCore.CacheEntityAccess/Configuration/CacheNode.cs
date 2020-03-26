namespace AspCore.CacheEntityAccess.Configuration
{
    public class CacheNode
    {
        public string Cachekey { get; set; }

        public AuthorizedClient[] AuthorizedClients { get; set; }
    }
}
