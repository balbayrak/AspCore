using AspCore.ApiClient.Entities;
using AspCore.Utilities.DataProtector;
using AspCore.Web.Middlewares;
using AspCore.WebComponents.HtmlHelpers.ConfirmBuilder;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace AspCore.Web.Configuration
{
    public static class ApplicationBuilderExtension
    {
        public static void UseAspCoreWeb(this IApplicationBuilder app, string authenticationControllerName)
        {
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<WebAuthenticationMiddleware>(authenticationControllerName);
           
            app.UseMiddleware<CorrelationIdMiddleware>();

            app.UseHeaderPropagation();

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
