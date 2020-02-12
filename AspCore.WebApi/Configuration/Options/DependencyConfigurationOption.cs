using AspCore.AOP.Configuration;
using AspCore.Entities.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AspCore.WebApi.Configuration.Options
{
    public class DependencyConfigurationOption : ConfigurationOption
    {
        public DependencyConfigurationOption(IServiceCollection services) : base(services)
        {
        }
        public ConfigurationHelperOption AddDependencyResolver(Action<InterceptorOptionBuilder> option)
        {
            var dependencyOptionBuilder = new InterceptorOptionBuilder(_services);
            option(dependencyOptionBuilder);

            return new ConfigurationHelperOption(_services);
        }
    }
}
