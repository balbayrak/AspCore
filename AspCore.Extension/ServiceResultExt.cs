using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Entities.Search;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AspCore.Dtos.Dto;
using AspCore.Mapper.Abstract;

namespace AspCore.Extension
{
    public static class ServiceResultExt
    {
        public static void StatusMessage<TResult, TEntity>(this ServiceResult<TResult> result, string message, CoreEntityState entityState)
        {
            if (entityState == CoreEntityState.Added)
            {
                result.StatusMessage = string.Format(message, typeof(TEntity).Name);
            }
            else if (entityState == CoreEntityState.Deleted)
            {
                result.StatusMessage = string.Format(message, typeof(TEntity).Name);
            }
            else
            {
                result.StatusMessage = string.Format(message, typeof(TEntity).Name);
            }
        }
        public static void ErrorMessage<TResult>(this ServiceResult<TResult> result, string errorMessage, Exception ex)
        {
            result.IsSucceeded = false;
            result.ErrorMessage = errorMessage;
            if (ex != null)
                result.ExceptionMessage = ex.Message + "---> stacktrace:" + ex.StackTrace;
        }
        public static void ErrorMessage<TResult>(this ServiceResult<TResult> result, string errorMessage, string exceptionMessage, string customExceptionMessage = null)
        {
            result.IsSucceeded = false;
            result.ErrorMessage = errorMessage;
            if (!string.IsNullOrEmpty(exceptionMessage))
                result.ExceptionMessage = exceptionMessage;
            else
            {
                if (!string.IsNullOrEmpty(customExceptionMessage))
                    result.ExceptionMessage = customExceptionMessage;
            }
        }
        public static ActionResult ToHttpResponse<TModel>(this ServiceResult<TModel> response)
        {
            var status = HttpStatusCode.OK;

            if (!response.IsSucceeded && !string.IsNullOrEmpty(response.ExceptionMessage))
                status = HttpStatusCode.InternalServerError;
            else if (response.IsSucceeded && response.Result == null)
                status = HttpStatusCode.NotFound;

            return new ObjectResult(response)
            {
                StatusCode = (int)status
            };
        }

        public static ServiceResult<TDestionation> ChangeResult<TSource, TDestionation>(this ServiceResult<TSource> result, TDestionation entity)
        {
            ServiceResult<TDestionation> resultDestionation = new ServiceResult<TDestionation>()
            {
                Result = entity,
                ErrorMessage = result.ErrorMessage,
                ExceptionMessage = result.ExceptionMessage,
                IsSucceeded = result.IsSucceeded,
                SearchResultCount = result.SearchResultCount,
                StatusCode = result.StatusCode,
                StatusMessage = result.StatusMessage,
                TotalResultCount = result.TotalResultCount,
                WarningMessage = result.WarningMessage
            };
            return resultDestionation;
        }
        static Task<TBase> FromDerived<TBase, TDerived>(this Task<TDerived> task) where TDerived : TBase
        {
            var tcs = new TaskCompletionSource<TBase>();
            task.ContinueWith(t => tcs.SetResult(t.Result), TaskContinuationOptions.OnlyOnRanToCompletion);
            task.ContinueWith(t => tcs.SetException(t.Exception.InnerExceptions), TaskContinuationOptions.OnlyOnFaulted);
            task.ContinueWith(t => tcs.SetCanceled(), TaskContinuationOptions.OnlyOnCanceled);
            return tcs.Task;
        }

        public static void ToViewModelResult<TViewModel, TEntity>(this ServiceResult<List<TViewModel>> serviceResult, ServiceResult<List<TEntity>> result)
             where TViewModel : BaseViewModel<TEntity>, new()
             where TEntity : class, IEntity, new()
        {
            serviceResult.IsSucceeded = result.IsSucceeded;
            serviceResult.SearchResultCount = result.SearchResultCount;
            serviceResult.TotalResultCount = result.TotalResultCount;
            result.Result.ForEach(x => x.ProtectEntity());
            var entityView = result.Result.Select(t => new TViewModel
            {
                dataEntity = t
            }).ToList();
            serviceResult.Result = entityView;
        }
        public static void ToViewModelResult<TViewModel, TEntity>(this ServiceResult<TViewModel> serviceResult, ServiceResult<TEntity> result)
            where TViewModel : BaseViewModel<TEntity>, new()
            where TEntity : class, IEntity, new()
        {

            serviceResult.IsSucceeded = result.IsSucceeded;
            serviceResult.SearchResultCount = result.SearchResultCount;
            serviceResult.TotalResultCount = result.TotalResultCount;

            result.Result.ProtectEntity();

            var entityView = new TViewModel()
            {
                dataEntity = result.Result
            };
            serviceResult.Result = entityView;
        }

        public static ServiceResult<List<TEntity>> ProtectEntity<TEntity>(this ServiceResult<List<TEntity>> serviceResult)
            where TEntity : class, IEntityDto, new()
        {
            if (serviceResult.IsSucceededAndDataIncluded())
            {
                serviceResult.Result.ForEach(x => x.ProtectEntity());
            }

            return serviceResult;
        }
        public static ServiceResult<TEntity> ProtectEntity<TEntity>(this ServiceResult<TEntity> serviceResult)
            where TEntity : class, IEntityDto, new()
        {
            if (serviceResult.IsSucceededAndDataIncluded())
            {
                serviceResult.Result.ProtectEntity();
            }

            return serviceResult;
        }

        public static ServiceResult<List<TEntity>> ToServiceResultFromDataSearchResult<TEntity>(this ServiceResult<DataSearchResult<TEntity>> result)
where TEntity : class, ISearchableEntity, new()
        {
            ServiceResult<List<TEntity>> serviceResult = new ServiceResult<List<TEntity>>();

            serviceResult.IsSucceeded = result.IsSucceeded;
            serviceResult.SearchResultCount = result.SearchResultCount;
            serviceResult.TotalResultCount = result.TotalResultCount;
            if (result.IsSucceededAndDataIncluded())
            {
                result.Result.items.ForEach(x => x.ProtectEntity());
                serviceResult.Result = result.Result.items;
            }

            return serviceResult;
        }
        public static void ToViewModelResultFromSearchableEntityList<TViewModel, TEntity>(this ServiceResult<List<TViewModel>> serviceResult, ServiceResult<DataSearchResult<TEntity>> result)
where TViewModel : BaseViewModel<TEntity>, new()
where TEntity : class, ISearchableEntity, new()
        {
            serviceResult.IsSucceeded = result.IsSucceeded;
            serviceResult.SearchResultCount = result.SearchResultCount;
            serviceResult.TotalResultCount = result.TotalResultCount;
            if (serviceResult.IsSucceededAndDataIncluded())
            {
                result.Result.items.ForEach(x => x.ProtectEntity());
                var entityView = result.Result.items.Select(t => new TViewModel
                {
                    dataEntity = t
                }).ToList();
                serviceResult.Result = entityView;
            }
        }
        public static void ToViewModelResultFromSearchableEntity<TViewModel, TEntity>(this ServiceResult<TViewModel> serviceResult, ServiceResult<DataSearchResult<TEntity>> result)
            where TViewModel : BaseViewModel<TEntity>, new()
            where TEntity : class, ISearchableEntity, new()
        {

            serviceResult.IsSucceeded = result.IsSucceeded;
            serviceResult.SearchResultCount = result.SearchResultCount;
            serviceResult.TotalResultCount = result.TotalResultCount;

            if (result.IsSucceededAndDataIncluded())
            {
                result.Result.items[0].ProtectEntity();
            }

            var entityView = new TViewModel()
            {
                dataEntity = result.Result.items[0]
            };
            serviceResult.Result = entityView;
        }

        public static void ToViewModelResultFromSearchableEntityList<TEntity, TSearchableEntity>(this ServiceResult<List<TEntity>> serviceResult, IAutoObjectMapper autoObjectMapper, ServiceResult<DataSearchResult<TSearchableEntity>> result)
            where TSearchableEntity : class, ISearchableEntity, new()
            where TEntity : class, IEntityDto, new()

        {
            serviceResult.IsSucceeded = result.IsSucceeded;
            serviceResult.SearchResultCount = result.SearchResultCount;
            serviceResult.TotalResultCount = result.TotalResultCount;
            if (serviceResult.IsSucceededAndDataIncluded())
            {
                result.Result.items.ForEach(x => x.ProtectEntity());
                var entityView = autoObjectMapper.Mapper.Map<List<TEntity>>(result.Result.items);
                serviceResult.Result = entityView;
            }
        }

        public static void ToViewModelResultFromSearchableEntity<TEntityDto, TSearchableEntity>(this ServiceResult<TEntityDto> serviceResult, IAutoObjectMapper autoObjectMapper, ServiceResult<DataSearchResult<TSearchableEntity>> result)
            where TEntityDto : IEntityDto, new()
            where TSearchableEntity : class, ISearchableEntity, new()
        {

            serviceResult.IsSucceeded = result.IsSucceeded;
            serviceResult.SearchResultCount = result.SearchResultCount;
            serviceResult.TotalResultCount = result.TotalResultCount;

            if (result.IsSucceededAndDataIncluded())
            {
                result.Result.items[0].ProtectEntity();
            }
            var entityView = autoObjectMapper.Mapper.Map<TEntityDto>(result.Result.items[0]);

            serviceResult.Result = entityView;
        }
    }
}
