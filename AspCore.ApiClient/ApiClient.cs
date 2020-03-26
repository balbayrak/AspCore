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
using System.Threading.Tasks;

namespace AspCore.ApiClient
{
    public class ApiClient<TOption> : IApiClient, IDisposable
        where TOption : class, IApiClientConfiguration, new()
    {
        protected IHttpContextAccessor _httpContextAccessor;
        private readonly IConfigurationAccessor _configurationHelper;
        protected ICacheService _accessTokenService;

        protected TOption apiConfiguration { get; set; }

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

        public ApiClient(string apiKey)
        {
            this.apiKey = apiKey;

            _httpContextAccessor = DependencyResolver.Current.GetService<IHttpContextAccessor>();
            _configurationHelper = DependencyResolver.Current.GetService<IConfigurationAccessor>();
            _accessTokenService = DependencyResolver.Current.GetService<ICacheService>();

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
                using (var client = new HttpClient(handler))
                {

                    client.BaseAddress = new Uri(_baseAddress);

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ApiConstants.Api_Keys.JSON_MEDIA_TYPE_QUALITY_HEADER));
                    client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue(ApiConstants.Api_Keys.GZIP_COMPRESSION_STRING_WITH_QUALITY_HEADER));


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

                    string correlationID = _httpContextAccessor.HttpContext.GetHeaderValue(HttpContextConstant.HEADER_KEY.CORRELATION_ID);
                    if (!string.IsNullOrEmpty(correlationID))
                    {
                        client.DefaultRequestHeaders.Add(HttpContextConstant.HEADER_KEY.CORRELATION_ID, correlationID);
                    }


                    var response = await client.GetAsync(_apiUrl).ConfigureAwait(false);

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

                using (var client = new HttpClient(handler))
                {
                    client.BaseAddress = new Uri(_baseAddress);

                    if(authenticationInfo == null)
                        Authenticate(client, false, false);
                    else
                    {
                        if (!authenticationInfo.access_token.Contains(ApiConstants.Api_Keys.API_AUTHORIZATION_BEARER))
                            authenticationInfo.access_token = ApiConstants.Api_Keys.API_AUTHORIZATION_BEARER + " " + authenticationInfo.access_token;

                        client.DefaultRequestHeaders.Remove(ApiConstants.Api_Keys.API_AUTHORIZATION);
                        client.DefaultRequestHeaders.Add(ApiConstants.Api_Keys.API_AUTHORIZATION, authenticationInfo.access_token);
                    }

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ApiConstants.Api_Keys.JSON_MEDIA_TYPE_QUALITY_HEADER));
                    client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue(ApiConstants.Api_Keys.GZIP_COMPRESSION_STRING_WITH_QUALITY_HEADER));

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

                    string correlationID = _httpContextAccessor.HttpContext.GetHeaderValue(HttpContextConstant.HEADER_KEY.CORRELATION_ID);
                    if (!string.IsNullOrEmpty(correlationID))
                    {
                        client.DefaultRequestHeaders.Add(HttpContextConstant.HEADER_KEY.CORRELATION_ID, correlationID);
                    }

                    JsonContent jsonContent = new JsonContent(postObject);
                    var response = await client.PostAsync(_apiUrl, jsonContent);

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

                using (var client = new HttpClient(handler))
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

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ApiConstants.Api_Keys.JSON_MEDIA_TYPE_QUALITY_HEADER));
                    client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue(ApiConstants.Api_Keys.GZIP_COMPRESSION_STRING_WITH_QUALITY_HEADER));

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

                    string correlationID = _httpContextAccessor.HttpContext.GetHeaderValue(HttpContextConstant.HEADER_KEY.CORRELATION_ID);
                    if (!string.IsNullOrEmpty(correlationID))
                    {
                        client.DefaultRequestHeaders.Add(HttpContextConstant.HEADER_KEY.CORRELATION_ID, correlationID);
                    }

                    JsonContent jsonContent = new JsonContent(postObject);
                    var response = client.PostAsync(_apiUrl, jsonContent).Result;

                    if (response.StatusCode.Equals(HttpStatusCode.Unauthorized))
                    {
                        Authenticate(client, true, response.Headers.Contains(ApiConstants.Api_Keys.TOKEN_EXPIRED_HEADER));

                        response = client.PostAsync(_apiUrl, jsonContent).Result;
                    }


                    //response.EnsureSuccessStatusCode();

                    //if (response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.BadRequest)
                    //{
                    string responseString = await response.Content.ReadAsStringAsync();
                 
                    Debug.WriteLine(responseString);
                    result = JsonConvert.DeserializeObject<TResult>(responseString);
                    //}
                }
            };

            return result;
        }

        public virtual async Task<TResult> PostRequest<TResult>(HttpContent content)
             where TResult : class, new()
        {
            TResult result = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ApiConstants.Api_Keys.JSON_MEDIA_TYPE_QUALITY_HEADER));

                string correlationID = _httpContextAccessor.HttpContext.GetHeaderValue(HttpContextConstant.HEADER_KEY.CORRELATION_ID);
                if (!string.IsNullOrEmpty(correlationID))
                {
                    client.DefaultRequestHeaders.Add(HttpContextConstant.HEADER_KEY.CORRELATION_ID, correlationID);
                }


                var response = client.PostAsync(_apiUrl, content).Result;

                //response.EnsureSuccessStatusCode();

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
                apiConfiguration = _configurationHelper.GetValueByKey<TOption>(apiKey);


            if (apiConfiguration != null)
            {
                if (!string.IsNullOrEmpty(apiConfiguration.BaseAddress))
                {
                    baseAddress = apiConfiguration.BaseAddress;
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
