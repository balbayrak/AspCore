using AspCore.Business.Abstract;
using AspCore.DataSearchApi.ElasticSearch.Abstract;
using AspCore.DataSearchApi.ElasticSearch.Concrete;
using AspCore.ElasticSearchApiClient.QueryBuilder.Concrete;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using Microsoft.AspNetCore.Builder;
using System;

namespace AspCore.DataSearchApi.Configuration
{
    public class ElasticSearchInitializer
    {
        private readonly IApplicationBuilder _app;

        public ElasticSearchInitializer(IApplicationBuilder app)
        {
            _app = app;
        }
        public ElasticSearchInitializer InitElasticSearchIndex<TSearchableEntity, TSearchableEntityService, TElasticSearchProvider>(string indexKey, bool initWithData)
            where TSearchableEntity : class, ISearchableEntity, new()
            where TSearchableEntityService : ISearchableEntityService<TSearchableEntity>
            where TElasticSearchProvider : BaseElasticSearchProvider<TSearchableEntity, TSearchableEntityService>, IElasticSearchProvider<TSearchableEntity>
        {

            TElasticSearchProvider elasticSearchProvider = (TElasticSearchProvider)_app.ApplicationServices.GetService(typeof(IElasticSearchProvider<TSearchableEntity>));
             ServiceResult<bool> result =  elasticSearchProvider.InitIndex(new InitIndexRequest
            {
                initializeWithData = initWithData
            });

            if(!result.IsSucceeded)
            {
                throw new Exception(result.ErrorMessage + " " + result.ExceptionMessage);
            }


            return this;
        }
    }
}
