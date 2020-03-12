using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using AspCore.Dependency.Concrete;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Utilities.DataProtector;

namespace AspCore.Extension
{
    public static class ServiceResultExt
    {
        public static void StatusMessage<TResult, TEntity>(this ServiceResult<TResult> result,string message, CoreEntityState entityState)
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

            if (!response.IsSucceeded)
                status = HttpStatusCode.InternalServerError;
            else if (response.Result == null)
                status = HttpStatusCode.NotFound;

            return new ObjectResult(response)
            {
                StatusCode = (int)status
            };
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
    }
}
