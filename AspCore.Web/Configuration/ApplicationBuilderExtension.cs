using AspCore.Web.Middlewares;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.Web.Configuration
{
    public static class ApplicationBuilderExtension
    {
        public static void UseAspCoreWeb(this IApplicationBuilder app, string authenticationControllerName)
        {
            app.UseMiddleware<WebAuthenticationMiddleware>(authenticationControllerName);
           
            //app.UseMiddleware<CustomHeaderMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=" + authenticationControllerName + "}/{action=Login}/{id?}");
            });
        }
    }
}
