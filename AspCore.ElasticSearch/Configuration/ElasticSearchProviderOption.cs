using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.ElasticSearch.Configuration
{
    public class ElasticSearchProviderOption
    {
        public IServiceCollection services { get; }

        public ElasticSearchProviderOption(IServiceCollection services)
        {
            this.services = services;
        }
    }
}
