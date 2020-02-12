using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;
using AspCore.Middlewares;
using AspCore.Middlewares.WebApi;

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
