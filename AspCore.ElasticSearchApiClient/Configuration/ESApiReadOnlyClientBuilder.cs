using AspCore.Entities.EntityType;
using Microsoft.Extensions.DependencyInjection;

namespace AspCore.ElasticSearchApiClient.Configuration
{
    public class ESApiReadOnlyClientBuilder
    {
        private readonly IServiceCollection _services;
        private string _apiClientKey { get; set; }
        public ESApiReadOnlyClientBuilder(IServiceCollection services,string apiClientKey)
        {
            _services = services;
            _apiClientKey = apiClientKey;
        }
        public ESApiReadOnlyClientBuilder AddESApiClient<TSearchableEntity>(string apiClientKey, string elasticApiRoute)
     where TSearchableEntity : class, ISearchableEntity, new()
        {
            _services.AddTransient(typeof(IElasticClient<TSearchableEntity>), sp =>
            {
                return new ReadOnlyElasticClient<TSearchableEntity>(apiClientKey,elasticApiRoute);
            });

            return this;
        }

        public ESApiReadOnlyClientBuilder AddESApiClient<TSearchableEntity>(string elasticApiRoute)
     where TSearchableEntity : class, ISearchableEntity, new()
        {
            _services.AddTransient(typeof(IElasticClient<TSearchableEntity>), sp =>
            {
                return new ReadOnlyElasticClient<TSearchableEntity>(_apiClientKey, elasticApiRoute);
            });

            return this;
        }
    }
}
