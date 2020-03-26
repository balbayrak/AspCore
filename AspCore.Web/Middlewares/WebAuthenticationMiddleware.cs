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
        private readonly ICacheService _cache;
        private string _authenticationControllerName;
        public WebAuthenticationMiddleware(RequestDelegate next, string authenticationControllerName)
        {
            _authenticationControllerName = authenticationControllerName;
            _cache = DependencyResolver.Current.GetService<ICacheService>();
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            string tokenKey = _cache.GetObject<string>(ApiConstants.Api_Keys.CUSTOM_TOKEN_STORAGE_KEY);

            if (!string.IsNullOrEmpty(tokenKey))
            {
                if (_cache.GetObject<AuthenticationToken>(tokenKey) == null)
                {
                    httpContext.Response.Redirect($"/{_authenticationControllerName}/LogOut");
                }
            }

            await _next(httpContext);
        }
    }
}
