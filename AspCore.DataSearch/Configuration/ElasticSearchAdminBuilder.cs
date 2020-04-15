using AspCore.ElasticSearchApiClient;
using AspCore.Entities.EntityType;
using Microsoft.Extensions.DependencyInjection;

namespace AspCore.DataSearch.Configuration
{
    public class ElasticSearchAdminBuilder
    {
        private readonly IServiceCollection _services;
        private string _apiClientKey { get; set; }
        public ElasticSearchAdminBuilder(IServiceCollection services, string apiClientKey)
        {
            _services = services;
            _apiClientKey = apiClientKey;
        }
        public ElasticSearchAdminBuilder AddElasticSearchAdmin<TSearchableEntity>(string apiClientKey, string elasticApiRoute)
                   where TSearchableEntity : class, ISearchableEntity, new()
        {
            _services.AddTransient(typeof(IElasticClient<TSearchableEntity>), sp =>
            {
                return new ElasticClient<TSearchableEntity>(apiClientKey, elasticApiRoute);
            });

            return this;
        }
        public ElasticSearchAdminBuilder AddElasticSearchAdmin<TSearchableEntity>(string elasticApiRoute)
             where TSearchableEntity : class, ISearchableEntity, new()
        {
            _services.AddTransient(typeof(IElasticClient<TSearchableEntity>), sp =>
            {
                return new ElasticClient<TSearchableEntity>(_apiClientKey, elasticApiRoute);
            });

            return this;
        }
    }
}
