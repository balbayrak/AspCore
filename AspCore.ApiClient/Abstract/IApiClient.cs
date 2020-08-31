using AspCore.Entities.Authentication;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AspCore.ApiClient.Abstract
{
    public interface IApiClient : IDisposable
    {
        string apiUrl { get; set; }

        string baseAddress { get; set; }

        string apiKey { get; }

        void ChangeApiSettingsKey(string apiKey);

        Task<TResult> GetRequest<TResult>(Dictionary<string, string> headerValues = null)
            where TResult : class, new();

        Task<TResult> PostRequest<TResult>(HttpContent content) where TResult : class, new();

        Task<TResult> PostRequest<TResult>(object postObject, Dictionary<string, string> headerValues = null) where TResult : class, new();


    }
}
