using AspCore.Extension;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace AspCore.ApiClient.Handlers
{
    public class CorrelationIdHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CorrelationIdHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (_httpContextAccessor != null && _httpContextAccessor.HttpContext != null)
            {
                string correlationID = _httpContextAccessor.HttpContext.GetHeaderValue(HttpContextConstant.HEADER_KEY.CORRELATION_ID);
                if (!string.IsNullOrEmpty(correlationID))
                {
                    request.Headers.Add(HttpContextConstant.HEADER_KEY.CORRELATION_ID, correlationID);
                }
            }
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
