using AspCore.ApiClient.Abstract;
using AspCore.ApiClient.Entities.Abstract;
using AspCore.ApiClient.Entities.Concrete;
using AspCore.Caching.Abstract;
using AspCore.ConfigurationAccess.Abstract;
using AspCore.Dependency.Concrete;
using AspCore.Entities.Authentication;
using AspCore.Entities.Constants;
using AspCore.Extension;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace AspCore.ApiClient
{
    public class ApiClient<TOption> : IApiClient, IDisposable
        where TOption : class, IApiClientConfiguration, new()
    {
        protected IHttpContextAccessor HttpContextAccessor { get; private set; }
        protected IConfigurationAccessor ConfigurationHelper { get; private set; }
        protected ICacheService AccessTokenService { get; private set; }
        protected ICancellationTokenHelper CancellationTokenHelper { get; }
        protected TOption ApiConfiguration { get; set; }

        private string _baseAddress;

        public string baseAddress
        {
            get
            {
                return _baseAddress;
            }
            set
            {
                if (!string.IsNullOrEmpty(value)) value = value.TrimStart('/');
                if (!string.IsNullOrEmpty(value) && (!value.EndsWith("/"))) value = value + "/";
                _baseAddress = value;
            }
        }

        private string _apiUrl { get; set; }

        public string apiUrl
        {
            get
            {
                return _apiUrl;
            }
            set
            {
                if (!string.IsNullOrEmpty(value)) value = value.StartsWith("/") ? value.TrimStart('/') : value;
                _apiUrl = value;

            }
        }

        public string apiKey { get; private set; }

        public AuthenticationInfo authenticationInfo { get; set; }

        public ApiClient(IHttpContextAccessor httpContextAccessor, IConfigurationAccessor configurationAccessor, ICacheService cacheService, ICancellationTokenHelper cancellationTokenHelper, string apiKey)
        {
            this.apiKey = apiKey;
            CancellationTokenHelper = cancellationTokenHelper;
            HttpContextAccessor = httpContextAccessor;
            ConfigurationHelper = configurationAccessor;
            AccessTokenService = cacheService;

            _baseAddress = string.Empty;
            
            InitializeBaseAddress(apiKey);
        }

        public virtual void AddAuthenticationRoute(string route) { }

        public virtual async Task<TResult> GetRequest<TPostObject, TResult>(Dictionary<string, string> headerValues = null)
              where TResult : class, new()
              where TPostObject : class
        {
            TResult result = null;
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                using (var client = new CoreHttpClient(handler,TimeSpan.FromMinutes(10)))
                {

                    client.BaseAddress = new Uri(_baseAddress);

                    if (headerValues != null && headerValues.Count > 0)
                    {
                        foreach (var key in headerValues.Keys)
                        {
                            if (!client.DefaultRequestHeaders.Contains(key))
                            {
                                client.DefaultRequestHeaders.Remove(key);
                                client.DefaultRequestHeaders.Add(key, headerValues[key]);
                            }
                        }
                    }

                    string correlationID = HttpContextAccessor.HttpContext.GetHeaderValue(HttpContextConstant.HEADER_KEY.CORRELATION_ID);
                    if (!string.IsNullOrEmpty(correlationID))
                    {
                        client.DefaultRequestHeaders.Add(HttpContextConstant.HEADER_KEY.CORRELATION_ID, correlationID);
                    }
                    var response = await client.GetAsync(_apiUrl,CancellationTokenHelper.Token).ConfigureAwait(false);

                    response.EnsureSuccessStatusCode();

                    await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
                    {
                        if (x.IsFaulted)
                            throw x.Exception;

                        result = JsonConvert.DeserializeObject<TResult>(x.Result);
                    });
                }
            }
            return result;
        }

        public virtual async Task<TResult> PostRequest<TPostObject, TResult>(TPostObject postObject, Dictionary<string, string> headerValues = null, AuthenticationToken authenticationInfo = null)
              where TResult : class, new()
              where TPostObject : class
        {
            TResult result = null;

            using (HttpClientHandler handler = new HttpClientHandler())
            {
                handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                using (var client = new CoreHttpClient(handler,TimeSpan.FromMinutes(10)))
                {
                    client.BaseAddress = new Uri(_baseAddress);

                    if (authenticationInfo == null)
                        Authenticate(client, false, false);
                    else
                    {
                        if (!authenticationInfo.access_token.Contains(ApiConstants.Api_Keys.API_AUTHORIZATION_BEARER))
                            authenticationInfo.access_token = ApiConstants.Api_Keys.API_AUTHORIZATION_BEARER + " " + authenticationInfo.access_token;

                        client.DefaultRequestHeaders.Remove(ApiConstants.Api_Keys.API_AUTHORIZATION);
                        client.DefaultRequestHeaders.Add(ApiConstants.Api_Keys.API_AUTHORIZATION, authenticationInfo.access_token);
                    }
                
                    if (headerValues != null && headerValues.Count > 0)
                    {
                        foreach (var key in headerValues.Keys)
                        {
                            if (!client.DefaultRequestHeaders.Contains(key))
                            {
                                client.DefaultRequestHeaders.Remove(key);
                                client.DefaultRequestHeaders.Add(key, headerValues[key]);
                            }
                        }
                    }

                    string correlationID = HttpContextAccessor.HttpContext.GetHeaderValue(HttpContextConstant.HEADER_KEY.CORRELATION_ID);
                    if (!string.IsNullOrEmpty(correlationID))
                    {
                        client.DefaultRequestHeaders.Add(HttpContextConstant.HEADER_KEY.CORRELATION_ID, correlationID);
                    }

                    JsonContent jsonContent = new JsonContent(postObject);
                    var response = await client.PostAsync(_apiUrl, jsonContent, CancellationTokenHelper.Token);
                    if (response.StatusCode.Equals(HttpStatusCode.Unauthorized))
                    {
                        bool refreshTokenCnt = response.Headers.Contains(ApiConstants.Api_Keys.TOKEN_EXPIRED_HEADER);

                        Authenticate(client, true, refreshTokenCnt);

                        response = await client.PostAsync(_apiUrl, jsonContent);
                    }

                    string responseString = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<TResult>(responseString);


                }
            };

            return result;
        }

        public virtual async Task<TResult> PostRequest<TResult>(object postObject, Dictionary<string, string> headerValues = null, AuthenticationToken authenticationInfo = null)
            where TResult : class, new()
        {
            TResult result = null;

            using (HttpClientHandler handler = new HttpClientHandler())
            {
                handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                using (var client = new CoreHttpClient(handler,TimeSpan.FromMinutes(10)))
                {
                    client.BaseAddress = new Uri(_baseAddress);

                    if (authenticationInfo == null)
                        Authenticate(client, false, false);
                    else
                    {
                        string token = authenticationInfo.access_token;
                        if (!token.Contains(ApiConstants.Api_Keys.API_AUTHORIZATION_BEARER))
                            token = ApiConstants.Api_Keys.API_AUTHORIZATION_BEARER + " " + authenticationInfo.access_token;

                        client.DefaultRequestHeaders.Remove(ApiConstants.Api_Keys.API_AUTHORIZATION);
                        client.DefaultRequestHeaders.Add(ApiConstants.Api_Keys.API_AUTHORIZATION, token);
                    }

                 
                    if (headerValues != null && headerValues.Count > 0)
                    {
                        foreach (var key in headerValues.Keys)
                        {
                            if (!client.DefaultRequestHeaders.Contains(key))
                            {
                                client.DefaultRequestHeaders.Remove(key);
                                client.DefaultRequestHeaders.Add(key, headerValues[key]);
                            }
                        }
                    }

                    string correlationID = HttpContextAccessor.HttpContext.GetHeaderValue(HttpContextConstant.HEADER_KEY.CORRELATION_ID);
                    if (!string.IsNullOrEmpty(correlationID))
                    {
                        client.DefaultRequestHeaders.Add(HttpContextConstant.HEADER_KEY.CORRELATION_ID, correlationID);
                    }

                    JsonContent jsonContent = new JsonContent(postObject);
                    var response = client.PostAsync(_apiUrl, jsonContent,CancellationTokenHelper.Token).Result;
                    if (response.StatusCode.Equals(HttpStatusCode.Unauthorized))
                    {

                        bool refreshTokenCnt = response.Headers.Contains(ApiConstants.Api_Keys.TOKEN_EXPIRED_HEADER);
                        if (!refreshTokenCnt)
                        {
                            string s = response.Headers.WwwAuthenticate.ToString();
                            refreshTokenCnt = s.Contains(ApiConstants.Api_Keys.TOKEN_EXPIRED_HEADER_STR, StringComparison.InvariantCultureIgnoreCase);
                        }

                        Authenticate(client, true, refreshTokenCnt);

                        response = await client.PostAsync(_apiUrl, jsonContent);
                    }
                    if (response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        string responseString = await response.Content.ReadAsStringAsync();

                        Debug.WriteLine(responseString);
                        result = JsonConvert.DeserializeObject<TResult>(responseString);
                    }
                }
            };

            return result;
        }

        public virtual async Task<TResult> PostRequest<TResult>(HttpContent content)
             where TResult : class, new()
        {
            TResult result = null;

            using (var client = new CoreHttpClient(TimeSpan.FromMinutes(10)))
            {
                client.BaseAddress=new Uri(_baseAddress);
                string correlationID = HttpContextAccessor.HttpContext.GetHeaderValue(HttpContextConstant.HEADER_KEY.CORRELATION_ID);
                if (!string.IsNullOrEmpty(correlationID))
                {
                    client.DefaultRequestHeaders.Add(HttpContextConstant.HEADER_KEY.CORRELATION_ID, correlationID);
                }
                var response = client.PostAsync(_apiUrl, content,CancellationTokenHelper.Token).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = await response.Content.ReadAsStringAsync();

                    result = JsonConvert.DeserializeObject<TResult>(responseString);
                }
            }
            return result;
        }

        public virtual AuthenticationToken Authenticate(HttpClient client, bool forceAuthentication, bool refreshToken)
        {
            return null;
        }

        private void InitializeBaseAddress(string apiKey)
        {
            if (!string.IsNullOrEmpty(apiKey))
                ApiConfiguration = ConfigurationHelper.GetValueByKey<TOption>(apiKey);


            if (ApiConfiguration != null)
            {
                if (!string.IsNullOrEmpty(ApiConfiguration.BaseAddress))
                {
                    baseAddress = ApiConfiguration.BaseAddress;
                }
            }
        }

        public virtual void ChangeApiSettingsKey(string apiKey)
        {
            InitializeBaseAddress(apiKey);
        }

        public void Dispose()
        {
            _baseAddress = null;
            _apiUrl = null;
        }


    }
}
