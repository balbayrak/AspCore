using AspCore.WebApi.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace AspCore.WebApi.Configuration
{
    public static class ApplicationBuilderExtension
    {
        public static void UseAspCoreApi(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
            //app.UseMiddleware<CustomHeaderMiddleware>();
        }
    }
}
