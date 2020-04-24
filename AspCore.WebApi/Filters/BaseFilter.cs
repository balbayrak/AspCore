using AspCore.Entities.Authentication;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace AspCore.WebApi.Filters
{
    public abstract class BaseFilter<TOption> : IActionFilter
        where TOption : AuthorizationFilterOption, new()
    {
        public abstract void OnCustomActionExecuting(ActionExecutingContext context);
        private TOption _authorizationFilterOption { get; set; }

        public BaseFilter(Action<TOption> option)
        {
            _authorizationFilterOption = new TOption();
            option(_authorizationFilterOption);
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            string controllerName = context.ActionDescriptor.RouteValues["controller"];
            bool skip = false;

            if (_authorizationFilterOption.includeControllerNames != null && _authorizationFilterOption.includeControllerNames.Length > 0)
            {
                skip = !_authorizationFilterOption.includeControllerNames.Contains(controllerName);
            }

            if (_authorizationFilterOption.excludeControllerNames != null && _authorizationFilterOption.excludeControllerNames.Length > 0)
            {
                skip = _authorizationFilterOption.excludeControllerNames.Contains(controllerName);
            }


            if (!skip)
            {
                OnCustomActionExecuting(context);
            }
        }

        public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
        {
            throw new NotImplementedException();
        }
    }
}
