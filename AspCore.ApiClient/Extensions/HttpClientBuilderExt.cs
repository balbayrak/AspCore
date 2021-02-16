using System;
using System.Net.Http;

namespace AspCore.ApiClient.Extensions
{
    public static class HttpClientBuilderExt
    {
        public static HttpClient CreateHttpClient(this IHttpClientFactory httpClientFactory, string key, string baseAddress)
        {
            var client = httpClientFactory.CreateClient(key);
            client.BaseAddress = new Uri(baseAddress);
            return client;
        }
    }
}
