using Microsoft.Extensions.DependencyInjection;
using System;
using AspCore.ConfigurationAccess.Configuration;
using AspCore.Entities.Configuration;

namespace AspCore.WebApi.Configuration.Options
{
    public class ConfigurationHelperOption : ConfigurationOption
    {
        public ConfigurationHelperOption(IServiceCollection services) : base(services)
        {
        }

        public ConfigurationBuilderOption AddConfigurationManager(Action<ConfigurationManagerOptionBuilder> option)
        {
            ConfigurationManagerOptionBuilder configurationManagerOptionBuilder = new ConfigurationManagerOptionBuilder(_services);
            option(configurationManagerOptionBuilder);

            return new ConfigurationBuilderOption(_services);
        }
    }
}
