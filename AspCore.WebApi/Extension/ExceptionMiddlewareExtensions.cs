using AspCore.Entities.General;
using AspCore.WebApi.General;
using AspCore.WebApi.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace AspCore.WebApi.Extension
{
    internal static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        // logger.Error

                        await context.Response.WriteAsync(new BaseServiceResult()
                        {
                            StatusCode = context.Response.StatusCode,
                            ErrorMessage = WebApiConstants.BaseExceptionMessages.INTERNAL_SERVER_ERROR_OCCURRED,
                            ExceptionMessage = contextFeature.Error.Message + "---> stacktrace:" + contextFeature.Error.StackTrace,

                        }.ToString());
                    }
                });
            });
        }

        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
