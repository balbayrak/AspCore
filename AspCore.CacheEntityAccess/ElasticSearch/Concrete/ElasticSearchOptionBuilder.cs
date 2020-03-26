using AspCore.CacheEntityAccess.Configuration;
using AspCore.CacheEntityAccess.ElasticSearch.Abstract;
using AspCore.CacheEntityAccess.ElasticSearch.Concrete;
using AspCore.ConfigurationAccess.Abstract;
using AspCore.ConfigurationAccess.Concrete;
using AspCore.Entities.Configuration;
using Elasticsearch.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AspCore.CacheEntityAccess.ElasticSearch.Configuration
{
    public class ElasticSearchOptionBuilder : ConfigurationOption
    {
        public ElasticSearchOptionBuilder(IServiceCollection services) : base(services)
        {
        }

        public void AddElasticsearch<TOption>(string configurationKey)
            where TOption : class, ICacheApiOption, new()
        {
            using (ServiceProvider serviceProvider = services.BuildServiceProvider())
            {
                if (!string.IsNullOrEmpty(configurationKey))
                {
                    var httpContextAccessor = services.FirstOrDefault(d => d.ServiceType == typeof(IHttpContextAccessor));
                    if (httpContextAccessor == null)
                    {
                        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
                    }
                    //configuration helper ile setting

                    IConfigurationAccessor configurationHelper = serviceProvider.GetRequiredService<IConfigurationAccessor>();
                    if (configurationHelper == null)
                    {
                        throw new Exception(ConfigurationHelperConstants.ErrorMessages.CONFIGURATION_HELPER_NOT_FOUND);
                    }

                    TOption elasticOption = configurationHelper.GetValueByKey<TOption>(configurationKey);

                    if (elasticOption != null)
                    {
                        if (elasticOption.ElasticSearchProvider != null)
                        {
                            if (elasticOption.ElasticSearchProvider.Servers != null && elasticOption.ElasticSearchProvider.Servers.Length > 0)
                            {
                                ConnectionSettings _settings = null;
                                if (elasticOption.ElasticSearchProvider.Servers.Length > 1)
                                {
                                    var connectionUris = new List<Uri>();
                                    foreach (var server in elasticOption.ElasticSearchProvider.Servers)
                                    {
                                        connectionUris.Add(new Uri(server.Url));
                                    }

                                    var _connectionPool = new SniffingConnectionPool(connectionUris);
                                    _settings = new ConnectionSettings(_connectionPool)
                                         .PrettyJson(true)
                                        .EnableHttpCompression(true)
                                        .SniffOnConnectionFault(false)
                                        .SniffOnStartup(false)
                                        .SniffLifeSpan(TimeSpan.FromMinutes(1));
                                }
                                else
                                {
                                    _settings = new ConnectionSettings(new Uri(elasticOption.ElasticSearchProvider.Servers[0].Url))
                                                    .PrettyJson(true)
                                                    .EnableHttpCompression(true);
                                }

                                if (_settings != null)
                                {
                                    var client = new ElasticClient(_settings);

                                    services.AddSingleton<IElasticClient>(client);
                                    services.AddSingleton<IESContext,ESContext>();
                                }
                            }
                        }
                    }
                }
            }
        }

        public void AddElasticsearch(ConnectionSettings connectionSettings = null)
        {
            if (connectionSettings != null)
            {
                var client = new ElasticClient(connectionSettings);
                services.AddSingleton<IElasticClient>(client);
                services.AddSingleton<IESContext, ESContext>();
            }
        }
    }
}
