using AspCore.Authentication.JWT.Concrete;
using AspCore.Entities.Authentication;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.WebApi.Authentication.General;
using AspCore.WebApi.Authentication.Providers.Abstract;
using AspCore.WebApi.Configuration.Options;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;

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
        }

        public override void OnCustomActionExecuting(ActionExecutingContext context)
        {
            authenticationProvider = context.HttpContext.RequestServices.GetRequiredService<IApiAuthenticationProvider<TInput, TOutput>>();
            ServiceResult<bool> result = authenticationProvider.AuthorizeAction(context.ActionDescriptor.RouteValues["action"], context.ActionArguments);

            if (!result.IsSucceeded)
            {
                context.Result = new CustomUnauthorizedResult(AuthenticationConstants.AUTHORIZATION.NOT_AUTHORIZE_ACTION);
            }
        }
    }
}
