using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using AspCore.ApiClient.Abstract;
using AspCore.Entities.Configuration;
using AspCore.Extension;

namespace AspCore.ApiClient.Configuration
{
    public class ApiClientOptionBuilder : ConfigurationOption, IDisposable
    {
        public ApiClientOptionBuilder(IServiceCollection services) : base(services)
        {

        }
        public void AddApiClients(Action<ApiClientByNameBuilder> builder)
        {
            services.AddHttpClient();

            ApiClientByNameBuilder apiClientByNameBuilder = new ApiClientByNameBuilder(services);
            builder(apiClientByNameBuilder);
        }

        public void Dispose()
        {

        }
    }
}
