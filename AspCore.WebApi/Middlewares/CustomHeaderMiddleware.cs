using AspCore.Entities.Authentication;
using AspCore.Entities.Constants;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Entities.User;
using AspCore.Extension;
using AspCore.WebApi.Authentication.Abstract;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace AspCore.WebApi.Middlewares
{
    public class CustomHeaderMiddleware<TTokenGenerator, TJWTInfo>
        where TTokenGenerator : ITokenGenerator<TJWTInfo>
         where TJWTInfo : class, IJWTEntity, new()
    {
        private readonly RequestDelegate _next;

        private readonly TTokenGenerator _tokenGenerator;
        public CustomHeaderMiddleware(RequestDelegate next, TTokenGenerator tokenGenerator)
        {
            _tokenGenerator = tokenGenerator;
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                if (httpContext.Request.Headers.ContainsKey(ApiConstants.Api_Keys.API_AUTHORIZATION) && httpContext.Request.Headers[ApiConstants.Api_Keys.API_AUTHORIZATION][0].StartsWith("Bearer "))
                {
                    if (!httpContext.Request.Headers.ContainsKey(HttpContextConstant.HEADER_KEY.CORRELATION_ID))
                    {
                        var token = httpContext.Request.Headers[ApiConstants.Api_Keys.API_AUTHORIZATION][0]
                            .Substring("Bearer ".Length);

                        ServiceResult<TJWTInfo> jwtInfoResult = _tokenGenerator.GetJWTInfo(new AuthenticationToken
                        {
                            access_token = token
                        });


                        if (jwtInfoResult.IsSucceededAndDataIncluded())
                        {
                            if (typeof(IActiveUser).IsAssignableFrom(typeof(TJWTInfo)))
                            {
                                httpContext.Request.Headers.Add(HttpContextConstant.HEADER_KEY.ACTIVE_USER, JsonConvert.SerializeObject(jwtInfoResult.Result));
                            }

                            httpContext.Request.Headers.Add(HttpContextConstant.HEADER_KEY.CORRELATION_ID, jwtInfoResult.Result.correlationId);


                            if (httpContext.Request.Headers.ContainsKey(HttpContextConstant.HEADER_KEY.ACTIVE_USER_ID))
                            {
                                httpContext.Request.Headers.Remove(HttpContextConstant.HEADER_KEY.ACTIVE_USER_ID);
                            }

                            httpContext.Request.Headers.Add(HttpContextConstant.HEADER_KEY.ACTIVE_USER_ID, jwtInfoResult.Result.activeUserId.ToString());
                        }
                    }
                }

                await _next(httpContext);
            }
            catch
            {

            }
        }

    }
}
