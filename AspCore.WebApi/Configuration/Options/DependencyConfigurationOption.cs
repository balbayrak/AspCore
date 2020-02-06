using Microsoft.Extensions.DependencyInjection;
using System;
using AspCore.Dependency.Configuration;
using AspCore.Entities.Configuration;

namespace AspCore.WebApi.Configuration.Options
{
    public class DependencyConfigurationOption : ConfigurationOption
    {
        public DependencyConfigurationOption(IServiceCollection services) : base(services)
        {
        }

        public ConfigurationHelperOption AddDependencyResolver(Action<DependencyOptionBuilder> option)
        {
            var dependencyOptionBuilder = new DependencyOptionBuilder(_services);
            option(dependencyOptionBuilder);

            return new ConfigurationHelperOption(_services);
        }
    }
}
