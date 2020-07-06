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

        public ConfigurationBuilderOption AddBffApiClient(Action<BffClientOption> option)
        {
            BffClientOption bffClientOption = new BffClientOption();
            option(bffClientOption);

            services.AddHttpClient();

            services.AddTransient(typeof(IBffApiClient), sp =>
            {
                var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
                var httpContextAccessor = sp.GetRequiredService<IHttpContextAccessor>();
                var configurationAccessor = sp.GetRequiredService<IConfigurationAccessor>();
                var cacheService = sp.GetRequiredService<ICacheService>();
                var tokenHelper = sp.GetRequiredService<ICancellationTokenHelper>();

                return new BffApiClient(httpClientFactory,httpContextAccessor, configurationAccessor, cacheService, tokenHelper, bffClientOption.apiConfigurationKey);
            });

            return new ConfigurationBuilderOption(services);
        }
    }
}
