using AspCore.Authentication.JWT.Abstract;
using AspCore.Entities.Authentication;
using AspCore.Entities.Constants;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Entities.User;
using AspCore.Extension;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace AspCore.WebApi.Middlewares
{
    public class ActiveUserHeaderMiddleware<TJWTInfo>
         where TJWTInfo : class, IJWTEntity, new()
    {
        private readonly RequestDelegate _next;

        private readonly ITokenValidator<TJWTInfo> _tokenValidator;
        public ActiveUserHeaderMiddleware(RequestDelegate next, ITokenValidator<TJWTInfo> tokenValidator)
        {
            _tokenValidator = tokenValidator ?? throw new ArgumentNullException(nameof(tokenValidator));
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {

            if (httpContext.Request.Headers.ContainsKey(ApiConstants.Api_Keys.API_AUTHORIZATION) && httpContext.Request.Headers[ApiConstants.Api_Keys.API_AUTHORIZATION][0].StartsWith("Bearer "))
            {
                var token = httpContext.Request.Headers[ApiConstants.Api_Keys.API_AUTHORIZATION][0]
                    .Substring("Bearer ".Length);

                ServiceResult<TJWTInfo> jwtInfoResult = _tokenValidator.Validate(new AuthenticationToken
                {
                    access_token = token
                },false);


                if (jwtInfoResult.IsSucceededAndDataIncluded())
                {
                    if (typeof(IAuthenticatedUser).IsAssignableFrom(typeof(TJWTInfo)))
                    {
                        httpContext.Request.Headers.Add(HttpContextConstant.HEADER_KEY.ACTIVE_USER, JsonConvert.SerializeObject(jwtInfoResult.Result));
                    }


                    if (httpContext.Request.Headers.ContainsKey(HttpContextConstant.HEADER_KEY.ACTIVE_USER_ID))
                    {
                        httpContext.Request.Headers.Remove(HttpContextConstant.HEADER_KEY.ACTIVE_USER_ID);
                    }

                    httpContext.Request.Headers.Add(HttpContextConstant.HEADER_KEY.ACTIVE_USER_ID, jwtInfoResult.Result.authenticatedUserId.ToString());
                }

            }

            await _next(httpContext);

        }

    }
}
