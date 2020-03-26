using AspCore.CacheEntityAccess.ElasticSearch.Configuration;
using AspCore.WebApi.Configuration.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.CacheEntityApi.Configuration
{
    public static class CacheApiConfiguration
    {
        public static ConfigurationBuilderOption AddCacheEntityProviders(this ConfigurationBuilderOption configurationBuilderOption, Action<ElasticSearchOptionBuilder> option)
        {
            var elasticSearchOptionBuilder = new ElasticSearchOptionBuilder(configurationBuilderOption.services);
            option(elasticSearchOptionBuilder);

            return configurationBuilderOption;
        }
    }
}
