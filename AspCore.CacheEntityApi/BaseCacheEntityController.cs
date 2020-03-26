using AspCore.CacheEntityApi.CacheProviders.Abstract;
using AspCore.CacheEntityClient.QueryBuilder.Concrete;
using AspCore.Dependency.Concrete;
using AspCore.Entities.Cache;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Extension;
using AspCore.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace AspCore.CacheApi
{
    public class BaseCacheEntityController<T, TCacheEntity> : BaseController
        where T : ICacheEntityProvider<TCacheEntity>
        where TCacheEntity : class, ICacheEntity, new()
    {
        public readonly T _cacheProvider;
        public BaseCacheEntityController()
        {
            _cacheProvider = DependencyResolver.Current.GetService<T>();
        }

        [NonAction]
        protected string GetCacheAliasName()
        {
            //string controllerName = ActionContext.ControllerContext.ControllerDescriptor.ControllerName;
            //CacheItem cacheItem = ConfigurationHelper.configurationManager.GetCacheItemByControllerName(controllerName);

            //if (cacheItem != null)
            //{
            //    return cacheItem.cacheKey;
            //}

            return string.Empty;
        }


        [NonAction]
        protected virtual IActionResult CreateCacheItem(TCacheEntity cacheItem)
        {
            ServiceResult<bool> response = _cacheProvider.CreateCacheItem(GetCacheAliasName(), cacheItem);
            return response.ToHttpResponse();
        }

        [NonAction]
        protected virtual IActionResult ReadCacheItem(SearchRequestItem searchItem)
        {
            ServiceResult<CacheResult<TCacheEntity>> response = _cacheProvider.ReadCacheItem(searchItem);
            return response.ToHttpResponse();
        }

        [NonAction]
        protected virtual IActionResult MinMaxCacheItem(SearchRequestItem searchItem)
        {
            ServiceResult<MinMax> response = _cacheProvider.MinMaxCacheItem(searchItem);
            return response.ToHttpResponse();
        }

        [NonAction]
        protected virtual IActionResult UpdateCacheItem(TCacheEntity cacheItem)
        {
            ServiceResult<bool> response = _cacheProvider.UpdateCacheItem(GetCacheAliasName(), cacheItem);
            return response.ToHttpResponse();
        }

        [NonAction]
        protected virtual IActionResult UpdateCacheItemList(List<TCacheEntity> cacheItemList)
        {
            ServiceResult<bool> response = _cacheProvider.UpdateCacheItemList(GetCacheAliasName(), cacheItemList);
            return response.ToHttpResponse();
        }

        [NonAction]
        protected virtual IActionResult DeleteCacheItemList(List<TCacheEntity> cacheItemList)
        {
            ServiceResult<bool> response = _cacheProvider.DeleteCacheItemList(GetCacheAliasName(), cacheItemList);
            return response.ToHttpResponse();
        }

        [NonAction]
        protected virtual IActionResult CreateCacheItemList(List<TCacheEntity> cacheItemList)
        {
            ServiceResult<bool> response = _cacheProvider.CreateCacheItemList(GetCacheAliasName(), cacheItemList);
            return response.ToHttpResponse();
        }

        [NonAction]
        protected virtual IActionResult DeleteCacheItem(TCacheEntity cacheItem)
        {
            ServiceResult<bool> response = _cacheProvider.DeleteCacheItem(GetCacheAliasName(), cacheItem);
            return response.ToHttpResponse();
        }

        [Authorize]
        [ActionName("CreateCacheData")]
        [HttpPost]

        public IActionResult Create(TCacheEntity cacheItem)
        {
            return CreateCacheItem(cacheItem);
        }

        [Authorize]
        [ActionName("ReadCacheData")]
        //[IndexNameSetter]
        [HttpPost]
        public IActionResult Read(SearchRequestItem searchItem)
        {
            return ReadCacheItem(searchItem);
        }

        [Authorize]
        [ActionName("UpdateCacheData")]
        [HttpPost]

        public IActionResult Update(TCacheEntity updateItem)
        {
            return UpdateCacheItem(updateItem);
        }

        [Authorize]
        [ActionName("DeleteCacheData")]
        [HttpPost]

        public IActionResult Delete(TCacheEntity deleteItem)
        {
            return DeleteCacheItem(deleteItem);
        }

        [Authorize]
        [ActionName("UpdateListCacheData")]
        [HttpPost]

        public IActionResult UpdateList(List<TCacheEntity> updateItemList)
        {
            return UpdateCacheItemList(updateItemList);
        }

        [Authorize]
        [ActionName("CreateListCacheData")]
        [HttpPost]

        public IActionResult CreateList(List<TCacheEntity> updateItemList)
        {
            return CreateCacheItemList(updateItemList);
        }

        [Authorize]
        [ActionName("DeleteListCacheData")]
        [HttpPost]

        public IActionResult DeleteList(List<TCacheEntity> updateItemList)
        {
            return DeleteCacheItemList(updateItemList);
        }

        [Authorize]
        [ActionName("MinMaxCacheData")]
        //[IndexNameSetter]
        [HttpPost]
        public IActionResult MinMax(SearchRequestItem searchItem)
        {
            return MinMaxCacheItem(searchItem);
        }
    }
}
