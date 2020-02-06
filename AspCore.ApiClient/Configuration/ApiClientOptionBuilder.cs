using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using AspCore.ApiClient.Abstract;
using AspCore.Entities.Configuration;

namespace AspCore.ApiClient.Configuration
{
    public class ApiClientOptionBuilder : ConfigurationOption, IDisposable
    {
        public ApiClientOptionBuilder(IServiceCollection services) : base(services)
        {

        }
        public void AddApiClients(Action<ApiClientByNameBuilder> builder)
        {
            ApiClientByNameBuilder apiClientByNameBuilder = new ApiClientByNameBuilder(_services);
            builder(apiClientByNameBuilder);
        }

        public void Dispose()
        {

        }
    }
}
