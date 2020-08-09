using AspCore.ApiClient.Configuration;
using AspCore.ApiClient.Entities;
using AspCore.ApiClient.Handlers;
using AspCore.BackendForFrontend.Abstract;
using AspCore.BackendForFrontend.Concrete;
using AspCore.Entities.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AspCore.Web.Configuration.Options
{
    public class ApiClientConfigurationOption : ConfigurationOption
    {
        public ApiClientConfigurationOption(IServiceCollection services) : base(services)
        {
        }

        public ConfigurationBuilderOption AddBffApiClient(Action<AuthenticatedApiClientOption> option)
        {
            services.AddAuthenticatedApiClient<IBffApiClient, BffApiClient, ApiClientConfiguration,AuthServiceBasedAuthenticationHandler<ApiClientConfiguration>>(option);

            return new ConfigurationBuilderOption(services);
        }
    }
}
