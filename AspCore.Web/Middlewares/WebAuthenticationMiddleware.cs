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
        private readonly IApplicationCachedClient _applicationCachedClient;
        private readonly ICacheService _cacheService;
        private string _authenticationControllerName;
        public WebAuthenticationMiddleware(RequestDelegate next,ICacheService cacheService, IApplicationCachedClient applicationCachedClient, string authenticationControllerName)
        {
            _authenticationControllerName = authenticationControllerName;
            _applicationCachedClient = applicationCachedClient;
            _cacheService = cacheService;
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            string tokenKey = _applicationCachedClient.ApplicationUserKey;

            if (!string.IsNullOrEmpty(tokenKey))
            {
                if (_cacheService.GetObject<AuthenticationToken>(tokenKey) == null)
                {
                    httpContext.Response.Redirect($"/{_authenticationControllerName}/LogOut");
                }
            }

            await _next(httpContext);
        }
    }
}
