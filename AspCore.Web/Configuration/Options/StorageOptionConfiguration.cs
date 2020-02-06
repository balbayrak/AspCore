using Microsoft.Extensions.DependencyInjection;
using System;
using AspCore.Entities.Configuration;
using AspCore.Storage.Configuration;

namespace AspCore.Web.Configuration.Options
{
    public class StorageOptionConfiguration : ConfigurationOption
    {
        public StorageOptionConfiguration(IServiceCollection services) : base(services)
        {
        }

        public ApiClientConfigurationOption AddStorageService(Action<StorageOptionBuilder> option)
        {
            var cacheStorageOptionBuilder = new StorageOptionBuilder(_services);
            option(cacheStorageOptionBuilder);

            return new ApiClientConfigurationOption(_services);
        }
    }
}
