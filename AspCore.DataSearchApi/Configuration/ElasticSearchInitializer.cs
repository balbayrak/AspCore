using AspCore.Business.Abstract;
using AspCore.DataSearchApi.ElasticSearch.Abstract;
using AspCore.DataSearchApi.ElasticSearch.Concrete;
using AspCore.ElasticSearchApiClient.QueryBuilder.Concrete;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using Microsoft.AspNetCore.Builder;
using System;
using System.Threading.Tasks;

namespace AspCore.DataSearchApi.Configuration
{
    public class ElasticSearchInitializer
    {
        private readonly IApplicationBuilder _app;

        public ElasticSearchInitializer(IApplicationBuilder app)
        {
            _app = app;
        }
        public async Task<ElasticSearchInitializer> InitElasticSearchIndex<TSearchableEntity, TSearchableEntityService, TElasticSearchProvider>(string indexKey, bool initWithData)
            where TSearchableEntity : class, ISearchableEntity, new()
            where TSearchableEntityService : ISearchableEntityService<TSearchableEntity>
            where TElasticSearchProvider : BaseElasticSearchProvider<TSearchableEntity, TSearchableEntityService>, IElasticSearchProvider<TSearchableEntity>
        {

            TElasticSearchProvider elasticSearchProvider = (TElasticSearchProvider)_app.ApplicationServices.GetService(typeof(IElasticSearchProvider<TSearchableEntity>));
            ServiceResult<bool> result = await elasticSearchProvider.InitIndex(new InitIndexRequest
            {
                initializeWithData = initWithData
            });


            return this;
        }
    }
}
