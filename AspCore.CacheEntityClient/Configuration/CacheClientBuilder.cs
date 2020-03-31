using AspCore.Entities.EntityType;
using Microsoft.Extensions.DependencyInjection;

namespace AspCore.CacheEntityClient.Configuration
{
    public class CacheClientBuilder
    {
        private readonly IServiceCollection _services;
        private string _apiClientKey { get; set; }
        public CacheClientBuilder(IServiceCollection services,string apiClientKey)
        {
            _services = services;
            _apiClientKey = apiClientKey;
        }
        public CacheClientBuilder AddCacheClient<TCacheEntiy>(string apiClientKey, string cacheKey, string cacheApiRoute)
     where TCacheEntiy : class, ICacheEntity, new()
        {
            _services.AddTransient(typeof(ICacheClient<TCacheEntiy>), sp =>
            {
                return new CacheClient<TCacheEntiy>(apiClientKey,cacheKey,cacheApiRoute);
            });

            return this;
        }

        public CacheClientBuilder AddCacheClient<TCacheEntiy>(string cacheKey, string cacheApiRoute)
   where TCacheEntiy : class, ICacheEntity, new()
        {
            _services.AddTransient(typeof(IReadOnlyCacheClient<TCacheEntiy>), sp =>
            {
                return new ReadOnlyCacheClient<TCacheEntiy>(_apiClientKey, cacheKey, cacheApiRoute);
            });

            return this;
        }
    }
}
