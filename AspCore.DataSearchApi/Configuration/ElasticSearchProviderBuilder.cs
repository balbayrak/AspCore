using AspCore.DataSearchApi.ElasticSearch.Abstract;
using AspCore.DataSearchApi.ElasticSearch.Concrete;
using AspCore.Dependency.Concrete;
using AspCore.ElasticSearch.Configuration;
using AspCore.ElasticSearchApiClient.QueryBuilder.Concrete;
using AspCore.Entities.EntityType;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace AspCore.DataSearchApi.Configuration
{
    public static class ElasticSearchProviderBuilder
    {
        public static ElasticSearchProviderOption AddElasticSearchIndex<TSearchableEntity, TElasticSearchProvider>(this ElasticSearchProviderOption elasticSearchProviderOption, string indexKey)
            where TSearchableEntity : class, ISearchableEntity, new()
            where TElasticSearchProvider : BaseElasticSearchProvider<TSearchableEntity>, IElasticSearchProvider<TSearchableEntity>
        {
            elasticSearchProviderOption.services.AddTransient(typeof(IElasticSearchProvider<TSearchableEntity>), sp =>
            {
                return (IElasticSearchProvider<TSearchableEntity>)Activator.CreateInstance(typeof(TElasticSearchProvider), indexKey);
            });

            return elasticSearchProviderOption;
        }
    }
}
