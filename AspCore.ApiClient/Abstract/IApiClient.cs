using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AspCore.ApiClient.Entities.Concrete;
using AspCore.Entities.Authentication;

namespace AspCore.ApiClient.Abstract
{
    public interface IApiClient
    {
        AuthenticationInfo authenticationInfo { get; set; }
        string apiUrl { get; set; }

        string baseAddress { get; set; }

        string apiKey { get; }

        void ChangeApiSettingsKey(string apiKey);

        void AddAuthenticationRoute(string apiKey);

        Task<TResult> GetRequest<TPostObject, TResult>(Dictionary<string, string> headerValues = null)
            where TResult : class, new()
              where TPostObject : class;

        Task<TResult> PostRequest<TResult>(HttpContent content) where TResult : class, new();

        Task<TResult> PostRequest<TResult>(object postObject, Dictionary<string, string> headerValues = null, AuthenticationToken authenticationInfo = null) where TResult : class, new();


    }
}
