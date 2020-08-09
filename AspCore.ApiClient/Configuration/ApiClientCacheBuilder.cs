using AspCore.Entities.Configuration;
using AspCore.Storage.Abstract;
using AspCore.Storage.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AspCore.ApiClient.Configuration
{
    public class ApiClientCacheBuilder : ConfigurationOption, IDisposable
    {
        public ApiClientCacheBuilder(IServiceCollection services) : base(services)
        {}


        public ApiClientOptionBuilder AddCacheService(Action<CacheOptionBuilder> builder)
        {
            CacheOptionBuilder cacheOptionBuilder = new CacheOptionBuilder(services);
            builder(cacheOptionBuilder);

            return new ApiClientOptionBuilder(services);
        }

        public void Dispose()
        {}
    }
}
