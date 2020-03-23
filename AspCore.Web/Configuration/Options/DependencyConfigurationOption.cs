﻿using AspCore.AOP.Configuration;
using AspCore.Entities.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AspCore.Web.Configuration.Options
{
    public class DependencyConfigurationOption : ConfigurationOption
    {
        public DependencyConfigurationOption(IServiceCollection services) : base(services)
        {

        }

        public ConfigurationHelperOption AddDependencyResolver(Action<InterceptorOptionBuilder> option)
        {
            var dependencyOptionBuilder = new InterceptorOptionBuilder(services);
            option(dependencyOptionBuilder);

            return new ConfigurationHelperOption(services);
        }
    }
}
