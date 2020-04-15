﻿using AspCore.ElasticSearchApiClient;
using AspCore.Entities.EntityType;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AspCore.DataSearch.Configuration
{
    public class DataSearchClientBuilder
    {
        private readonly IServiceCollection _services;
        private string _apiClientKey { get; set; }
        public DataSearchClientBuilder(IServiceCollection services, string apiClientKey)
        {
            _services = services;
            _apiClientKey = apiClientKey;
        }
        public DataSearchClientBuilder AddDataSearchClient<TSearchableEntity>(string apiClientKey, string elasticApiRoute)
                   where TSearchableEntity : class, ISearchableEntity, new()
        {
            _services.AddTransient(typeof(IReadOnlyElasticClient<TSearchableEntity>), sp =>
            {
                return new ReadOnlyElasticClient<TSearchableEntity>(apiClientKey, elasticApiRoute);
            });

            return this;
        }
        public DataSearchClientBuilder AddDataSearchClient<TSearchableEntity>(string elasticApiRoute)
             where TSearchableEntity : class, ISearchableEntity, new()
        {
            _services.AddTransient(typeof(IReadOnlyElasticClient<TSearchableEntity>), sp =>
            {
                return new ReadOnlyElasticClient<TSearchableEntity>(_apiClientKey, elasticApiRoute);
            });

            return this;
        }
        public void ElasticSearchAdmins(Action<ElasticSearchAdminBuilder> builder)
        {
            ElasticSearchAdminBuilder elasticSearchAdminBuilder = new ElasticSearchAdminBuilder(_services, _apiClientKey);
            builder(elasticSearchAdminBuilder);
        }
        public void ElasticSearchAdmins(string apiClientKey, Action<ElasticSearchAdminBuilder> builder)
        {
            ElasticSearchAdminBuilder elasticSearchAdminBuilder = new ElasticSearchAdminBuilder(_services, apiClientKey);
            builder(elasticSearchAdminBuilder);
        }

    }
}
