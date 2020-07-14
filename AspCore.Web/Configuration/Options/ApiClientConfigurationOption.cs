using Microsoft.Extensions.DependencyInjection;
using System;
using AspCore.ApiClient.Abstract;
using AspCore.ApiClient.Configuration;
using AspCore.ApiClient.Entities.Abstract;
using AspCore.BackendForFrontend.Abstract;
using AspCore.BackendForFrontend.Concrete;
using AspCore.Entities.Configuration;
using Microsoft.AspNetCore.Http;
using AspCore.ConfigurationAccess.Abstract;
using AspCore.Caching.Abstract;
using System.Net.Http;

namespace AspCore.Web.Configuration.Options
{
    public class ApiClientConfigurationOption : ConfigurationOption
    {
        public ApiClientConfigurationOption(IServiceCollection services) : base(services)
        {
        }

        public ConfigurationBuilderOption AddBffApiClient(Action<ApiClientOption> option)
        {
            services.AddClient<IBffApiClient, BffApiClient>(option);

            return new ConfigurationBuilderOption(services);
        }
    }
}
