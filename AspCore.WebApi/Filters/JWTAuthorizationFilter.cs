using Microsoft.AspNetCore.Mvc.Filters;
using System;
using AspCore.Authentication.Abstract;
using AspCore.Authentication.Concrete;
using AspCore.Dependency.Concrete;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.WebApi.Configuration.Options;
using AspCore.WebApi.Security.General;

namespace AspCore.WebApi.Filters
{
    public class JWTAuthorizationFilter<TInput, TOutput> : BaseFilter<JWTAuthorizationFilterOption>
          where TInput : AuthenticationInfo
          where TOutput : class, IJWTEntity, new()
    {
        private JWTAuthorizationFilterOption _jWTAuthorizationFilterOption { get; set; }

        private IApiAuthenticationProvider<TInput, TOutput> authenticationProvider { get; set; }

        public JWTAuthorizationFilter(Action<JWTAuthorizationFilterOption> option) : base(option)
        {
            _jWTAuthorizationFilterOption = new JWTAuthorizationFilterOption();
            option(_jWTAuthorizationFilterOption);

            authenticationProvider = DependencyResolver.Current.GetService<IApiAuthenticationProvider<TInput, TOutput>>();
        }

        public override void OnCustomActionExecuting(ActionExecutingContext context)
        {
            ServiceResult<bool> result = authenticationProvider.AuthorizeAction(context.ActionDescriptor.RouteValues["action"], context.ActionArguments);

            if (!result.IsSucceeded)
            {
                context.Result = new CustomUnauthorizedResult(SecurityConstants.AUTHORIZATION.NOT_AUTHORIZE_ACTION);
            }
        }
    }
}
