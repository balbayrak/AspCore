using AspCore.Dependency.Abstract;
using AspCore.Dependency.Concrete;
using AspCore.ElasticSearchApiClient;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Extension;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AspCore.DataSearch.Concrete.ElasticSearch.Admin
{
    public abstract class ElasticSearchAdmin<TSearchableEntity> : IElasticSearchAdmin<TSearchableEntity>
        where TSearchableEntity : class, ISearchableEntity, new()
    {
        private readonly IElasticClient<TSearchableEntity> _elasticClient;

        public ElasticSearchAdmin(IElasticClient<TSearchableEntity> elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task<ServiceResult<bool>> ResetIndex(bool initWithData)
        {
            return await _elasticClient.ResetIndex(initWithData);
        }
    }
}
