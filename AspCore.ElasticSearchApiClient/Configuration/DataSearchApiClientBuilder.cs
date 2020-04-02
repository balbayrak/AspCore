using AspCore.ApiClient;
using AspCore.ApiClient.Configuration;
using AspCore.ApiClient.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.ElasticSearchApiClient.Configuration
{
    public class DataSearchApiClientBuilder
    {
        private readonly IServiceCollection _services;

        public DataSearchApiClientBuilder(IServiceCollection services)
        {
            _services = services;
        }

        public ESApiReadOnlyClientBuilder AddApiClients(string defaultApiClientKey, Action<ApiClientByNameBuilder> builder)
        {
            ApiClientByNameBuilder apiClientByNameBuilder = new ApiClientByNameBuilder(_services);
            builder(apiClientByNameBuilder);

            return new ESApiReadOnlyClientBuilder(_services, defaultApiClientKey);
        }
    }
}
