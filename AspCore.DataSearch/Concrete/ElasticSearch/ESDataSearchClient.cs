using AspCore.DataSearch.Abstract;
using AspCore.Dependency.Concrete;
using AspCore.ElasticSearchApiClient;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Entities.Search;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AspCore.DataSearch.Concrete.ElasticSearch
{
    public abstract class ESDataSearchClient<TSearchableEntity> : IDataSearchClient<TSearchableEntity>
         where TSearchableEntity : class, ISearchableEntity, new()
    {
        private readonly IReadOnlyElasticClient<TSearchableEntity> _readOnlyElasticClient;
        public ESDataSearchClient() 
        {
            _readOnlyElasticClient = DependencyResolver.Current.GetService<IReadOnlyElasticClient<TSearchableEntity>>();
        }

        public ServiceResult<DataSearchResult<TSearchableEntity>> FindBy(bool isActiveOnly, int startIndex, int takeCount)
        {
            ServiceResult<DataSearchResult<TSearchableEntity>> result = null;

            if (isActiveOnly)
            {
                result = _readOnlyElasticClient.Read(t => t.From(startIndex)
                                           .Size(takeCount)
                                           //.Sort(tt => tt.Descending(ttt => ttt.id))
                                           .Query(tt => tt.Bool(s => s.Filter(m => m.TermQuery(mm => mm.IsDeleted, false))))
                                           .TotalCountAgg(t=>t.searchId));

            }
            else
            {
                result = _readOnlyElasticClient.Read(t => t.From(startIndex)
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

            result = _readOnlyElasticClient.Read(t => t.Query(tt => tt.Bool(s => s.Filter(m => m.TermQuery(mm => mm.Id, Id),
                                                                                m => m.TermQuery(mm => mm.IsDeleted, !isActive)))));

            return result;
        }

        public ServiceResult<DataSearchResult<TSearchableEntity>> FindByIdList(List<Guid> idList, bool isActive)
        {
            List<object> objectList = idList.Cast<object>().ToList();
            return  _readOnlyElasticClient.Read(t => t.Query(tt => tt.Bool(s => s.Filter(m => m.TermsQuery(mm => mm.Id, objectList),
                                                                                m => m.TermQuery(mm => mm.IsDeleted, !isActive)))));
        }
    }
}
