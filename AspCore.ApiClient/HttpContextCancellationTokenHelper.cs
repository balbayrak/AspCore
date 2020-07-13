using AspCore.ApiClient.Abstract;
using Microsoft.AspNetCore.Http;
using System.Threading;

namespace AspCore.ApiClient
{
    public class HttpContextCancellationTokenHelper : ICancellationTokenHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CancellationToken Token => _httpContextAccessor.HttpContext?.RequestAborted ?? CancellationToken.None;

        public HttpContextCancellationTokenHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
