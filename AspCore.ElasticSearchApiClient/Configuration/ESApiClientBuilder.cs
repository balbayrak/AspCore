using AspCore.Entities.EntityType;
using Microsoft.Extensions.DependencyInjection;

namespace AspCore.ElasticSearchApiClient.Configuration
{
    public class ESApiClientBuilder
    {
        private readonly IServiceCollection _services;
        private string _apiClientKey { get; set; }
        public ESApiClientBuilder(IServiceCollection services,string apiClientKey)
        {
            _services = services;
            _apiClientKey = apiClientKey;
        }
        public ESApiClientBuilder AddESApiClient<TSearchableEntity>(string apiClientKey, string elasticApiRoute)
     where TSearchableEntity : class, ISearchableEntity, new()
        {
            _services.AddTransient(typeof(IElasticClient<TSearchableEntity>), sp =>
            {
                return new ElasticClient<TSearchableEntity>(apiClientKey,elasticApiRoute);
            });

            return this;
        }

        public ESApiClientBuilder AddESApiClient<TSearchableEntity>(string elasticApiRoute)
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
