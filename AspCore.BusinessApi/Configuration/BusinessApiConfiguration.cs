using AspCore.DataAccess.Configuration;
using AspCore.DataSearch.Configuration;
using AspCore.DocumentAccess.Configuration;
using AspCore.WebApi.Configuration.Options;
using System;
using AspCore.Mapper.Configuration;

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

        public static ConfigurationBuilderOption AddAutoMapper(this ConfigurationBuilderOption configurationBuilderOption)
        {
            MapperConfigurationBuilder mapperConfigurationBuilder = new MapperConfigurationBuilder(configurationBuilderOption.services);
            return configurationBuilderOption;
        }

        public static ConfigurationBuilderOption AddDataSearchAccessLayer(this ConfigurationBuilderOption configurationBuilderOption,string defaultDataSearchApiKey, Action<DataSearchEngineBuilder> builder)
        {
            DataSearchEngineBuilder dataSearchEngineBuilder = new DataSearchEngineBuilder(configurationBuilderOption.services, defaultDataSearchApiKey);
            builder(dataSearchEngineBuilder);
            return configurationBuilderOption;
        }
    }
}
