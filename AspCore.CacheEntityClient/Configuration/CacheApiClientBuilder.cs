using AspCore.ApiClient;
using AspCore.ApiClient.Configuration;
using AspCore.ApiClient.Entities;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.CacheEntityClient.Configuration
{
    public class CacheApiClientBuilder
    {
        private readonly IServiceCollection _services;

        public CacheApiClientBuilder(IServiceCollection services)
        {
            _services = services;
        }

        public CacheClientBuilder AddCacheApiClients(string defaultApiClientKey, Action<ApiClientByNameBuilder> builder)
        {
            ApiClientByNameBuilder apiClientByNameBuilder = new ApiClientByNameBuilder(_services);
            builder(apiClientByNameBuilder);

            return new CacheClientBuilder(_services, defaultApiClientKey);
        }
    }
}
