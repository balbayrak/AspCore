using AspCore.DataSearch.Abstract;
using AspCore.ElasticSearchApiClient;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Entities.Search;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public ServiceResult<bool> Create(params TSearchableEntity[] searchableEntities)
        {
            return ElasticClient.Create(searchableEntities);
        }

        public ServiceResult<bool> Update(params TSearchableEntity[] searchableEntities)
        {
            return ElasticClient.Update(searchableEntities);
        }

        public ServiceResult<bool> Delete(params TSearchableEntity[] searchableEntities)
        {
            return ElasticClient.Delete(searchableEntities);
        }

        public ServiceResult<DataSearchResult<TSearchableEntity>> FindBy(bool isActiveOnly, int startIndex, int takeCount)
        {
            ServiceResult<DataSearchResult<TSearchableEntity>> result = null;

            if (isActiveOnly)
            {
                result = ElasticClient.Read(t => t.From(startIndex)
                                           .Size(takeCount)
                                           //.Sort(tt => tt.Descending(ttt => ttt.id))
                                           .Query(tt => tt.Bool(s => s.Filter(m => m.TermQuery(mm => mm.IsDeleted, false))))
                                           .TotalCountAgg(t => t.searchId));

            }
            else
            {
                result = ElasticClient.Read(t => t.From(startIndex)
                                           .Size(takeCount)
                                           // .Sort(tt => tt.Descending(ttt => ttt.id))
                                           .Query(tt => tt.Bool(s => s.Must(m => m.MatchAllQuery())))

                                           );
            }

            return result;
        }

        public ServiceResult<DataSearchResult<TSearchableEntity>> FindById(Guid Id, bool isActive)
        {
            ServiceResult<DataSearchResult<TSearchableEntity>> result = null;

            result = ElasticClient.Read(t => t.Query(tt => tt.Bool(s => s.Filter(m => m.TermQuery(mm => mm.Id, Id),
                                                                                m => m.TermQuery(mm => mm.IsDeleted, !isActive)))));

            return result;
        }

        public ServiceResult<DataSearchResult<TSearchableEntity>> FindByIdList(List<Guid> idList, bool isActive)
        {
            List<object> objectList = idList.Cast<object>().ToList();
            return ElasticClient.Read(t => t.Query(tt => tt.Bool(s => s.Filter(m => m.TermsQuery(mm => mm.Id, objectList),
                                                                               m => m.TermQuery(mm => mm.IsDeleted, !isActive)))));
        }

    }
}
