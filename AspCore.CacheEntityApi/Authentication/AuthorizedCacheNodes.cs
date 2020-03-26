namespace AspCore.CacheEntityApi.Authentication
{
    public class AuthorizedCacheNodes
    {
        public string cacheKey { get; set; }
        public string[] actions { get; set; }
    }
}
