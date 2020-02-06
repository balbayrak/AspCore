using AspCore.ConfigurationAccess.Configuration;
using AspCore.Entities.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AspCore.Web.Configuration.Options
{
    public class ConfigurationHelperOption : ConfigurationOption
    {
        public ConfigurationHelperOption(IServiceCollection services) : base(services)
        {

        }
        public AuthenticationProviderBuilder AddConfigurationManager(Action<ConfigurationManagerOptionBuilder> option)
        {
            ConfigurationManagerOptionBuilder configurationManagerOption = new ConfigurationManagerOptionBuilder(_services);
            option(configurationManagerOption);

            return new AuthenticationProviderBuilder(_services);
        }
    }
}
