using Microsoft.Extensions.DependencyInjection;
using System;
using AspCore.Entities.Configuration;
using AspCore.Caching.Configuration;

namespace AspCore.Web.Configuration.Options
{
    public class CacheOptionConfiguration : ConfigurationOption
    {
        public CacheOptionConfiguration(IServiceCollection services) : base(services)
        {
        }

        public ApiClientConfigurationOption AddCacheService(Action<CacheOptionBuilder> option)
        {
            var cacheStorageOptionBuilder = new CacheOptionBuilder(services);
            option(cacheStorageOptionBuilder);

            return new ApiClientConfigurationOption(services);
        }
    }
}
