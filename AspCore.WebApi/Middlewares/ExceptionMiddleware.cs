using AspCore.Entities.General;
using AspCore.WebApi.General;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AspCore.WebApi.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private  readonly JsonSerializerSettings _jsonSettings;
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
            _jsonSettings = new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                NullValueHandling = NullValueHandling.Ignore
            };
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
              
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private  Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var data = new BaseServiceResult()
            {
                StatusCode = context.Response.StatusCode,
                ErrorMessage = WebApiConstants.BaseExceptionMessages.INTERNAL_SERVER_ERROR_OCCURRED,
                ExceptionMessage = exception.Message + "---> stacktrace:" + exception.StackTrace,
            };
            var jsonContent = JsonConvert.SerializeObject(data, _jsonSettings);
            return context.Response.WriteAsync(jsonContent);
        }
    }
}
