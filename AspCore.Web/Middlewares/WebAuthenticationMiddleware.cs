using AspCore.Entities.Constants;
using AspCore.Web.Configuration.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace AspCore.Web.Middlewares
{
    public class WebAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string _authenticationControllerName;
        private bool _sameDomain;
        private CookieConfigurationBuilder _cookieConfigurationBuilder;
        public WebAuthenticationMiddleware(RequestDelegate next, IHttpContextAccessor httpContextAccessor, CookieConfigurationBuilder cookieConfigurationBuilder, string authenticationControllerName, bool sameDomain)
        {
            _authenticationControllerName = authenticationControllerName;
            _httpContextAccessor = httpContextAccessor;
            _next = next;
            _sameDomain = sameDomain;
            _cookieConfigurationBuilder = cookieConfigurationBuilder;

        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            var token = await _httpContextAccessor.HttpContext.GetTokenAsync(ApiConstants.Api_Keys.ACCESS_TOKEN);

            if (_sameDomain && !_httpContextAccessor.HttpContext.Request.Path.Value.Contains($"/{_authenticationControllerName}") && string.IsNullOrEmpty(token))
            {
                httpContext.Response.Redirect($"/{_authenticationControllerName}/Login");
            }
            else
            {
                if (_sameDomain)
                {
                    if (_httpContextAccessor.HttpContext.Request.Path.Value.Contains($"/{_authenticationControllerName}"))
                    {
                        await _next(httpContext);
                    }
                    else
                    {
                        if (_httpContextAccessor.HttpContext.Request.Cookies[_cookieConfigurationBuilder.cookieOption.CookieName] != null)
                            await _next(httpContext);
                        else
                        {
                            httpContext.Response.Redirect($"/{_authenticationControllerName}/Login");
                        }
                    }
                }
                else
                {

                    await _next(httpContext);

                }
            }
        }
    }
}
