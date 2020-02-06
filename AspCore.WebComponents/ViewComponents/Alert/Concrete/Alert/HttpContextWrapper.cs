using Microsoft.AspNetCore.Http;

namespace AspCore.WebComponents.ViewComponents.Alert.Concrete
{
    public static class HttpContextWrapper
    {
        private static IHttpContextAccessor _httpContextAccessor;

        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public static HttpContext Current => _httpContextAccessor.HttpContext;
    }
}
