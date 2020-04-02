using AspCore.ElasticSearch.Configuration;
using AspCore.WebApi.Configuration.Options;
using System;

namespace AspCore.DataSearchApi.Configuration
{
    public static class DataSearchApiConfiguration
    {
        public static ConfigurationBuilderOption AddDataSearchProviders(this ConfigurationBuilderOption configurationBuilderOption, Action<ElasticSearchOptionBuilder> option)
        {
            var elasticSearchOptionBuilder = new ElasticSearchOptionBuilder(configurationBuilderOption.services);
            option(elasticSearchOptionBuilder);

            return configurationBuilderOption;
        }
    }
}
