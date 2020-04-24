using AspCore.ApiClient.Entities.Concrete;
using AspCore.Dependency.Concrete;
using AspCore.Utilities.DataProtector;
using AspCore.Web.Middlewares;
using AspCore.WebComponents.HtmlHelpers.ConfirmBuilder;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
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

            ApiClientFactory.Init(app.ApplicationServices);
            DataProtectorFactory.Init(app.ApplicationServices.GetRequiredService<IDataProtectorHelper>());
            ConfirmManagerFactory.Init(app.ApplicationServices.GetRequiredService<IConfirmService>());
        }
    }
}
