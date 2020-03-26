namespace AspCore.CacheEntityAccess.Configuration
{
    public class CacheProviderOption
    {
        public string Type { get; set; }

        public CacheServer[] Servers { get; set; }
    }
}
