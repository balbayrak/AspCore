using AspCore.DataSearch.Abstract;
using AspCore.ElasticSearchApiClient;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Entities.Search;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspCore.DataSearch.Concrete.ElasticSearch
{
    public abstract class ESDataSearchEngine<TSearchableEntity>
         where TSearchableEntity : class, ISearchableEntity, new()
    {
        protected readonly IElasticClient<TSearchableEntity> ElasticClient;

        protected readonly IServiceProvider ServiceProvider;
        public ESDataSearchEngine(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            ElasticClient = ServiceProvider.GetRequiredService<IElasticClient<TSearchableEntity>>();
        }

        public async Task<ServiceResult<bool>> CreateAsync(params TSearchableEntity[] searchableEntities)
        {
            return await ElasticClient.Create(searchableEntities);
        }

        public async Task<ServiceResult<bool>> UpdateAsync(params TSearchableEntity[] searchableEntities)
        {
            return await ElasticClient.Update(searchableEntities);
        }

        public async Task<ServiceResult<bool>> DeleteAsync(params TSearchableEntity[] searchableEntities)
        {
            return await ElasticClient.Delete(searchableEntities);
        }

        public async Task<ServiceResult<DataSearchResult<TSearchableEntity>>> FindByAsync(bool isActiveOnly, int startIndex, int takeCount)
        {
            ServiceResult<DataSearchResult<TSearchableEntity>> result = null;

            if (isActiveOnly)
            {
                result = await ElasticClient.Read(t => t.From(startIndex)
                                           .Size(takeCount)
                                           //.Sort(tt => tt.Descending(ttt => ttt.id))
                                           .Query(tt => tt.Bool(s => s.Filter(m => m.TermQuery(mm => mm.IsDeleted, false))))
                                           .TotalCountAgg(t => t.searchId));

            }
            else
            {
                result = await ElasticClient.Read(t => t.From(startIndex)
                                           .Size(takeCount)
                                           // .Sort(tt => tt.Descending(ttt => ttt.id))
                                           .Query(tt => tt.Bool(s => s.Must(m => m.MatchAllQuery())))

                                           );
            }

            return result;
        }

        public async Task<ServiceResult<DataSearchResult<TSearchableEntity>>> FindByIdAsync(Guid Id, bool isActive)
        {
            ServiceResult<DataSearchResult<TSearchableEntity>> result = null;

            result = await ElasticClient.Read(t => t.Query(tt => tt.Bool(s => s.Filter(m => m.TermQuery(mm => mm.Id, Id),
                                                                                m => m.TermQuery(mm => mm.IsDeleted, !isActive)))));

            return result;
        }

        public async Task<ServiceResult<DataSearchResult<TSearchableEntity>>> FindByIdListAsync(List<Guid> idList, bool isActive)
        {
            List<object> objectList = idList.Cast<object>().ToList();
            return await ElasticClient.Read(t => t.Query(tt => tt.Bool(s => s.Filter(m => m.TermsQuery(mm => mm.Id, objectList),
                                                                               m => m.TermQuery(mm => mm.IsDeleted, !isActive)))));
        }

    }
}
