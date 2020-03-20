using Microsoft.Extensions.DependencyInjection;
using System;
using AspCore.ApiClient.Configuration;
using AspCore.ApiClient.Entities.Abstract;
using AspCore.BackendForFrontend.Abstract;
using AspCore.BackendForFrontend.Concrete;
using AspCore.Entities.Configuration;

namespace AspCore.Web.Configuration.Options
{
    public class ApiClientConfigurationOption : ConfigurationOption
    {
        public ApiClientConfigurationOption(IServiceCollection services) : base(services)
        {
        }

        public ConfigurationBuilderOption AddBffApiClient(Action<BffClientOption> option)
        {
            BffClientOption bffClientOption = new BffClientOption();
            option(bffClientOption);

            services.AddTransient(typeof(IBffApiClient), sp =>
            {
                return new BffApiClient(bffClientOption.apiConfigurationKey);
            });

            return new ConfigurationBuilderOption(services);
        }
    }
}
