using AspCore.Entities.General;
using AspCore.WebApi.General;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;

namespace AspCore.WebApi.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        //private readonly ILoggerManager _logger;

        // public ExceptionMiddleware(RequestDelegate next, ILoggerManager logger)
        public ExceptionMiddleware(RequestDelegate next)
        {
            //_logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                //_logger.LogError($"Something went wrong: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return context.Response.WriteAsync(new BaseServiceResult()
            {
                StatusCode = context.Response.StatusCode,
                ErrorMessage = WebApiConstants.BaseExceptionMessages.INTERNAL_SERVER_ERROR_OCCURRED,
                ExceptionMessage = exception.Message + "---> stacktrace:" + exception.StackTrace,

            }.ToString());
        }
    }
}
