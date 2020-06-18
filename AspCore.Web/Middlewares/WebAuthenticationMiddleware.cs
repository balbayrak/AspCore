using AspCore.BackendForFrontend.Abstract;
using AspCore.Caching.Abstract;
using AspCore.Dependency.Concrete;
using AspCore.Entities.Authentication;
using AspCore.Entities.Constants;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace AspCore.Web.Middlewares
{
    public class WebAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ICookieService _cookieService;
        private readonly ICacheService _cacheService;
        private string _authenticationControllerName;
        public WebAuthenticationMiddleware(RequestDelegate next, ICacheService cacheService, ICookieService cookieService, string authenticationControllerName)
        {
            _authenticationControllerName = authenticationControllerName;
            _cookieService = cookieService;
            _cacheService = cacheService;
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            string tokenKey = _cookieService.GetObject<string>(ApiConstants.Api_Keys.APP_USER_STORAGE_KEY);

            if (!string.IsNullOrEmpty(tokenKey))
            {
                if (_cacheService.GetObject<AuthenticationToken>(tokenKey) == null)
                {
                    httpContext.Response.Redirect($"/{_authenticationControllerName}/LogOut");
                }
                else
                {
                    await _next(httpContext);
                }
            }
            else
            {
                try
                {
                    await _next(httpContext);
                }
                catch
                {
                    httpContext.Response.Redirect($"/{_authenticationControllerName}/LogOut");
                }
            }
        }
    }
}
