using Microsoft.Extensions.DependencyInjection;
using System;
using AspCore.Dependency.Configuration;
using AspCore.Entities.Configuration;
using AspCore.AOP.Configuration;

namespace AspCore.Web.Configuration.Options
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
