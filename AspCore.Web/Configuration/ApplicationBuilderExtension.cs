using AspCore.ApiClient.Entities;
using AspCore.Utilities.DataProtector;
using AspCore.Web.Configuration.Options;
using AspCore.Web.Middlewares;
using AspCore.WebComponents.HtmlHelpers.ConfirmBuilder;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;

namespace AspCore.Web.Configuration
{
    public static class ApplicationBuilderExtension
    {
        public static void UseAspCoreWeb(this IApplicationBuilder app, [NotNull] Action<AuthenticationControllerOption> option)
        {
            AuthenticationControllerOption authenticationControllerOption = new AuthenticationControllerOption();
            option(authenticationControllerOption);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<WebAuthenticationMiddleware>(authenticationControllerOption.ControllerName,authenticationControllerOption.SameDomain);

            app.UseMiddleware<CorrelationIdMiddleware>();

            app.UseHeaderPropagation();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=" + authenticationControllerOption.ControllerName + "}/{action=Login}/{id?}");
            });


            ApiClientFactory.Init(app.ApplicationServices);
            DataProtectorFactory.Init(app.ApplicationServices.GetRequiredService<IDataProtectorHelper>());
            ConfirmManagerFactory.Init(app.ApplicationServices.GetRequiredService<IConfirmService>());
        }
    }
}
