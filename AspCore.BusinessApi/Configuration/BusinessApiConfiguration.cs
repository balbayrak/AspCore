﻿using AspCore.DataAccess.Configuration;
using AspCore.DocumentAccess.Configuration;
using AspCore.ElasticSearchApiClient.Configuration;
using AspCore.WebApi.Configuration.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.BusinessApi.Configuration
{
    public static class BusinessApiConfiguration
    {
        public static ConfigurationBuilderOption AddDataAccessLayer(this ConfigurationBuilderOption configurationBuilderOption,Action<DataAccessLayerOptionBuilder> option)
        {
            var dataAccessLayerOptionBuilder = new DataAccessLayerOptionBuilder(configurationBuilderOption.services);
            option(dataAccessLayerOptionBuilder);

            return configurationBuilderOption;
        }

        public static ConfigurationBuilderOption AddDocumentAccessLayer(this ConfigurationBuilderOption configurationBuilderOption, Action<DocumentAccessBuilder> action)
        {
            DocumentAccessBuilder documentHelperBuilder = new DocumentAccessBuilder(configurationBuilderOption.services);
            action(documentHelperBuilder);

            return configurationBuilderOption;
        }

        public static ConfigurationBuilderOption AddDataSearchAccessLayer(this ConfigurationBuilderOption configurationBuilderOption,string defaultCAcheApiKey, Action<ESApiClientBuilder> builder)
        {
            ESApiClientBuilder cacheClientBuilder = new ESApiClientBuilder(configurationBuilderOption.services, defaultCAcheApiKey);
            builder(cacheClientBuilder);
            return configurationBuilderOption;
        }
    }
}
