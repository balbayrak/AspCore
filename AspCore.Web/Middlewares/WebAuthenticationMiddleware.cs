using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using AspCore.ApiClient.Entities.Concrete;
using AspCore.Dependency.Concrete;
using AspCore.Entities.Constants;
using AspCore.Storage.Abstract;
using AspCore.Entities.Authentication;

namespace AspCore.Web.Middlewares
{
    public class WebAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IStorage _storage;
        private string _authenticationControllerName;
        public WebAuthenticationMiddleware(RequestDelegate next, string authenticationControllerName)
        {
            _authenticationControllerName = authenticationControllerName;
            _storage = DependencyResolver.Current.GetService<IStorage>();
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            string tokenKey = _storage.GetObject<string>(ApiConstants.Api_Keys.CUSTOM_TOKEN_STORAGE_KEY);

            if (!string.IsNullOrEmpty(tokenKey))
            {
                if (_storage.GetObject<AuthenticationToken>(tokenKey) == null)
                {
                    httpContext.Response.Redirect($"/{_authenticationControllerName}/LogOut");
                }
            }

            await _next(httpContext);
        }
    }
}
