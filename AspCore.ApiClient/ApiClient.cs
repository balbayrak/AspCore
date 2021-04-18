﻿using AspCore.ApiClient.Abstract;
using AspCore.ApiClient.Entities;
using AspCore.ConfigurationAccess.Abstract;
using AspCore.Entities.Authentication;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;

namespace AspCore.ApiClient
{
    public class ApiClient<TOption> : IApiClient
        where TOption : class, IApiClientConfiguration, new()
    {
        protected IConfigurationAccessor ConfigurationHelper { get; private set; }
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
                if (!string.IsNullOrEmpty(value) && (!value.EndsWith("/"))) value = $"{value}/";
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

        private HttpClient _client { get; set; }

        public ApiClient(IHttpClientFactory httpClientFactory, IConfigurationAccessor configurationAccessor, string apiKey)
        {
            this.apiKey = apiKey;
            ConfigurationHelper = configurationAccessor;

            InitializeBaseAddress(apiKey);
            _client = httpClientFactory.CreateClient(this.apiKey);

            _client.BaseAddress = new Uri(_baseAddress);
        }

        public virtual async Task<TResult> GetRequest<TResult>(Dictionary<string, string> headerValues = null)
              where TResult : class, new()
        {
            TResult result = null;

            if (headerValues != null && headerValues.Count > 0)
            {
                foreach (var key in headerValues.Keys)
                {
                    if (!_client.DefaultRequestHeaders.Contains(key))
                    {
                        _client.DefaultRequestHeaders.Remove(key);
                        _client.DefaultRequestHeaders.Add(key, headerValues[key]);
                    }
                }
            }

            var response = await _client.GetAsync(_apiUrl);

            response.EnsureSuccessStatusCode();

            await response.Content.ReadAsStringAsync().ContinueWith((Task<string> x) =>
            {
                if (x.IsFaulted)
                    throw x.Exception;

                result = JsonConvert.DeserializeObject<TResult>(x.Result);
            });


            return result;
        }

        public virtual async Task<TResult> PostRequest<TPostObject, TResult>(TPostObject postObject, Dictionary<string, string> headerValues = null)
              where TResult : class, new()
              where TPostObject : class
        {
            TResult result;

            if (headerValues != null && headerValues.Count > 0)
            {
                foreach (var key in headerValues.Keys)
                {
                    if (!_client.DefaultRequestHeaders.Contains(key))
                    {
                        _client.DefaultRequestHeaders.Remove(key);
                        _client.DefaultRequestHeaders.Add(key, headerValues[key]);
                    }
                }
            }

            JsonContent jsonContent = new JsonContent(postObject);
            var response = await _client.PostAsync(_apiUrl, jsonContent);

            string responseString = await response.Content.ReadAsStringAsync();
            result = JsonConvert.DeserializeObject<TResult>(responseString);

            return result;
        }

        public async Task<TResult> PostAsync<TResult>(object postObject, Dictionary<string, string> headerValues = null) where TResult : class, new()
        {
            if (headerValues != null && headerValues.Count > 0)
            {
                foreach (var key in headerValues.Keys)
                {
                    if (!_client.DefaultRequestHeaders.Contains(key))
                    {
                        _client.DefaultRequestHeaders.Remove(key);
                        _client.DefaultRequestHeaders.Add(key, headerValues[key]);
                    }
                }
            }
            JsonContent jsonContent = new JsonContent(postObject);
            var response = await _client.PostAsync(apiUrl, jsonContent);
            string responseString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TResult>(responseString);
            return result;
        }

        public virtual async Task<TResult> PostRequest<TResult>(object postObject, Dictionary<string, string> headerValues = null)
            where TResult : class, new()
        {

            TResult result = null;

            if (headerValues != null && headerValues.Count > 0)
            {
                foreach (var key in headerValues.Keys)
                {
                    if (!_client.DefaultRequestHeaders.Contains(key))
                    {
                        _client.DefaultRequestHeaders.Remove(key);
                        _client.DefaultRequestHeaders.Add(key, headerValues[key]);
                    }
                }
            }
            var formatter = new JsonMediaTypeFormatter() { SerializerSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto } };
            formatter.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            var response = await _client.PostAsync(_apiUrl, postObject, formatter);

            if (response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.BadRequest)
            {
                string responseString = await response.Content.ReadAsStringAsync();

                result = JsonConvert.DeserializeObject<TResult>(responseString);
            }


            return result;
        }



        public async Task<bool> DeleteAsync(string uri, Dictionary<string, string> headerValues = null)
        {
            bool isSuccess = false;
            if (headerValues != null && headerValues.Count > 0)
            {
                foreach (var key in headerValues.Keys)
                {
                    if (!_client.DefaultRequestHeaders.Contains(key))
                    {
                        _client.DefaultRequestHeaders.Remove(key);
                        _client.DefaultRequestHeaders.Add(key, headerValues[key]);
                    }
                }
            }
            var response = await _client.DeleteAsync(uri);
            if (response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.BadRequest)
            {
                isSuccess = true;
            }
            return isSuccess;
        }

        public virtual async Task<TResult> PostRequest<TResult>(HttpContent content)
             where TResult : class, new()
        {
            TResult result = null;

            var response = await _client.PostAsync(_apiUrl, content);
            if (response.IsSuccessStatusCode)
            {
                string responseString = await response.Content.ReadAsStringAsync();

                result = JsonConvert.DeserializeObject<TResult>(responseString);
            }

            return result;
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
            _client.Dispose();
        }
    }
}
