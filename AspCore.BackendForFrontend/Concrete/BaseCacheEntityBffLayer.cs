using AspCore.BackendForFrontend.Abstract;
using AspCore.CacheEntityClient;
using AspCore.Dependency.Concrete;
using AspCore.Entities.Cache;
using AspCore.Entities.EntityFilter;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspCore.BackendForFrontend.Concrete
{
    public class BaseCacheEntityBffLayer<TViewModel, TCacheEntity> : BaseEntityBffLayer<TViewModel, TCacheEntity>, ICacheEntityBffLayer<TViewModel, TCacheEntity>
        where TViewModel : BaseViewModel<TCacheEntity>, new()
        where TCacheEntity : class, ICacheEntity, new()
    {
        private readonly ICacheClient<TCacheEntity> _cacheClient;
        public BaseCacheEntityBffLayer() : base()
        {
            _cacheClient = DependencyResolver.Current.GetService<ICacheClient<TCacheEntity>>();
        }

        public ServiceResult<List<TViewModel>> FindBy(bool isActiveOnly, int startIndex, int takeCount)
        {
            var viewResult = new ServiceResult<List<TViewModel>>();
            ServiceResult<CacheResult<TCacheEntity>> result = null;

            if (isActiveOnly)
            {
                result = _cacheClient.Read(t => t.CacheName(_cacheClient.cacheKey)
                                           .From(startIndex)
                                           .Size(takeCount)
                                           //.Sort(tt => tt.Descending(ttt => ttt.id))
                                           .Query(tt => tt.Bool(s => s.Filter(m => m.TermQuery(mm => mm.IsDeleted, false)))));

            }
            else
            {
                result = _cacheClient.Read(t => t.CacheName(_cacheClient.cacheKey)
                                           .From(startIndex)
                                           .Size(takeCount)
                                           // .Sort(tt => tt.Descending(ttt => ttt.id))
                                           .Query(tt => tt.Bool(s => s.Must(m => m.MatchAllQuery())))

                                           );
            }
            if (result.IsSucceededAndDataIncluded())
            {
                viewResult.ToViewModelResultFromCacheEntityList(result);
            }


            return viewResult;
        }

        public ServiceResult<TViewModel> FindById(Guid Id, bool isActive)
        {
            var viewResult = new ServiceResult<TViewModel>();
            ServiceResult<CacheResult<TCacheEntity>> result = null;

            result = _cacheClient.Read(t => t.CacheName(_cacheClient.cacheKey)
                                             .Query(tt => tt.Bool(s => s.Filter(m => m.TermQuery(mm => mm.Id, Id),
                                                                                m => m.TermQuery(mm => mm.IsDeleted, !isActive)))));

            if (result.IsSucceededAndDataIncluded())
            {
                viewResult.ToViewModelResultFromCacheEntity(result);
            }

            return viewResult;
        }

        public ServiceResult<List<TViewModel>> FindByIdList(List<Guid> idList, bool isActive)
        {
            var viewResult = new ServiceResult<List<TViewModel>>();
            ServiceResult<CacheResult<TCacheEntity>> result = null;

            List<object> objectList = idList.Cast<object>().ToList();
            result = _cacheClient.Read(t => t.CacheName(_cacheClient.cacheKey)
                                             .Query(tt => tt.Bool(s => s.Filter(m => m.TermsQuery(mm => mm.Id, objectList),
                                                                                m => m.TermQuery(mm => mm.IsDeleted, !isActive)))));

            if (result.IsSucceededAndDataIncluded())
            {
                viewResult.ToViewModelResultFromCacheEntityList(result);
            }

            return viewResult;
        }

    }
}
