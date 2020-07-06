using AspCore.Extension;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Threading.Tasks;

namespace AspCore.Web.Middlewares
{
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Headers.TryGetValue(HttpContextConstant.HEADER_KEY.CORRELATION_ID, out StringValues correlationId))
            {
                context.TraceIdentifier = correlationId;
            }

            context.Request.Headers.Add(HttpContextConstant.HEADER_KEY.CORRELATION_ID, new[] { context.TraceIdentifier });


            // apply the correlation ID to the response header for client side tracking
            context.Response.OnStarting(() =>
            {
                context.Response.Headers.Add(HttpContextConstant.HEADER_KEY.CORRELATION_ID, new[] { context.TraceIdentifier });
                return Task.CompletedTask;
            });

            await _next(context);
        }
    }
}
