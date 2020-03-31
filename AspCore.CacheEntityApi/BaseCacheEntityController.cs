using AspCore.CacheEntityApi.CacheProviders.Abstract;
using AspCore.CacheEntityClient.QueryBuilder.Concrete;
using AspCore.Dependency.Concrete;
using AspCore.Entities.Cache;
using AspCore.Entities.Constants;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Extension;
using AspCore.WebApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace AspCore.CacheEntityApi
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
        protected virtual IActionResult CreateCacheItem(TCacheEntity[] cacheItems)
        {
            ServiceResult<bool> response = null;
            
            if(cacheItems.Length == 1)
            {
                response = _cacheProvider.CreateCacheItem(GetCacheAliasName(), cacheItems[0]);
            }
            else
            {
                response = _cacheProvider.CreateCacheItemList(GetCacheAliasName(), cacheItems.ToList());
            }

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
        protected virtual IActionResult UpdateCacheItem(TCacheEntity[] cacheItems)
        {
            ServiceResult<bool> response = null;

            if (cacheItems.Length == 1)
            {
                response = _cacheProvider.UpdateCacheItem(GetCacheAliasName(), cacheItems[0]);
            }
            else
            {
                response = _cacheProvider.UpdateCacheItemList(GetCacheAliasName(), cacheItems.ToList());
            }

            return response.ToHttpResponse();
        }

        [NonAction]
        protected virtual IActionResult DeleteCacheItem(TCacheEntity[] cacheItems)
        {
            ServiceResult<bool> response = null;

            if (cacheItems.Length == 1)
            {
                response = _cacheProvider.DeleteCacheItem(GetCacheAliasName(), cacheItems[0]);
            }
            else
            {
                response = _cacheProvider.DeleteCacheItemList(GetCacheAliasName(), cacheItems.ToList());
            }

            return response.ToHttpResponse();
        }

        [Authorize]
        [ActionName(ApiConstants.CacheApi_Urls.CREATE_ACTION_NAME)]
        [HttpPost]

        public IActionResult Create(TCacheEntity[] cacheItems)
        {
            return CreateCacheItem(cacheItems);
        }

        [Authorize]
        [ActionName(ApiConstants.CacheApi_Urls.READ_ACTION_NAME)]
        //[IndexNameSetter]
        [HttpPost]
        public IActionResult Read(SearchRequestItem searchItem)
        {
            return ReadCacheItem(searchItem);
        }

        [Authorize]
        [ActionName(ApiConstants.CacheApi_Urls.UPDATE_ACTION_NAME)]
        [HttpPost]

        public IActionResult Update(TCacheEntity[] cacheItems)
        {
            return UpdateCacheItem(cacheItems);
        }

        [Authorize]
        [ActionName(ApiConstants.CacheApi_Urls.DELETE_ACTION_NAME)]
        [HttpPost]

        public IActionResult Delete(TCacheEntity[] cacheItems)
        {
            return DeleteCacheItem(cacheItems);
        }


        [ActionName(ApiConstants.CacheApi_Urls.MIN_MAX_ACTION_NAME)]
        [Authorize]
    
        //[IndexNameSetter]
        [HttpPost]
        public IActionResult MinMax(SearchRequestItem searchItem)
        {
            return MinMaxCacheItem(searchItem);
        }
    }
}
