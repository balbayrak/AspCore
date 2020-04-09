using AspCore.DataSearch.Abstract;
using AspCore.DataSearch.Concrete.ElasticSearch;
using AspCore.Entities.EntityType;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.DataSearch.Configuration
{
    public class DataSearchEngineBuilder
    {
        private readonly IServiceCollection _services;
        private string _apiClientKey { get; set; }
        public DataSearchEngineBuilder(IServiceCollection services, string apiClientKey)
        {
            _services = services;
            _apiClientKey = apiClientKey;
        }
        public DataSearchEngineBuilder AddDataSearchEngine<TSearchableEntity>(string apiClientKey, string elasticApiRoute)
     where TSearchableEntity : class, ISearchableEntity, new()
        {
            _services.AddTransient(typeof(IDataSearchEngine<TSearchableEntity>), sp =>
            {
                return new ESDataSearchEngine<TSearchableEntity>(apiClientKey, elasticApiRoute);
            });

            return this;
        }
        public DataSearchEngineBuilder AddDataSearchEngine<TSearchableEntity>(string elasticApiRoute)
   where TSearchableEntity : class, ISearchableEntity, new()
        {
            _services.AddTransient(typeof(IDataSearchEngine<TSearchableEntity>), sp =>
            {
                return new ESDataSearchEngine<TSearchableEntity>(_apiClientKey, elasticApiRoute);
            });

            return this;
        }
    }
}
