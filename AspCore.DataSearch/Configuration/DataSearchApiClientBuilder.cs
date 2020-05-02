using AspCore.ApiClient.Configuration;
using AspCore.ElasticSearchApiClient;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AspCore.DataSearch.Configuration
{
    public class DataSearchApiClientBuilder
    {
        private readonly IServiceCollection _services;

        public DataSearchApiClientBuilder(IServiceCollection services)
        {
            _services = services;
        }

        public DataSearchEngineBuilder AddApiClients(string defaultApiClientKey, Action<ApiClientByNameBuilder> builder)
        {
            ApiClientByNameBuilder apiClientByNameBuilder = new ApiClientByNameBuilder(_services);
            builder(apiClientByNameBuilder);

            return new DataSearchEngineBuilder(_services, defaultApiClientKey);
        }
    }
}
