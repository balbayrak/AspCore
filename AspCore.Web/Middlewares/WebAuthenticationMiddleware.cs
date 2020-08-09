using AspCore.Entities.Constants;
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
        public WebAuthenticationMiddleware(RequestDelegate next, IHttpContextAccessor httpContextAccessor, string authenticationControllerName)
        {
            _authenticationControllerName = authenticationControllerName;
            _httpContextAccessor = httpContextAccessor;
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            var token = await _httpContextAccessor.HttpContext.GetTokenAsync(ApiConstants.Api_Keys.ACCESS_TOKEN);

            if (!_httpContextAccessor.HttpContext.Request.Path.Value.Contains($"/{_authenticationControllerName}") && string.IsNullOrEmpty(token))
            {
                httpContext.Response.Redirect($"/{_authenticationControllerName}/Logout");
            }
            else
            {
                await _next(httpContext);
            }
        }
    }
}
