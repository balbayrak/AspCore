using AspCore.ApiClient.Entities;
using AspCore.ConfigurationAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

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
