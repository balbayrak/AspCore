using AspCore.ApiClient.Entities;
using AspCore.ConfigurationAccess.Abstract;
using AspCore.Entities.Authentication;
using AspCore.Entities.Constants;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace AspCore.ApiClient.Handlers
{
    public abstract class AspCoreAuthenticationHandler<TOption> : DelegatingHandler
        where TOption : class, IApiClientConfiguration, new()
    {
        protected readonly IServiceProvider ServiceProvider;
        private readonly IConfigurationAccessor _configurationAccessor;
        protected readonly TOption ConfigurationOption;
        protected readonly HttpClient TokenClient;
        protected readonly string ApiKey;
        private IHttpClientFactory _httpClientFactory;
        public AspCoreAuthenticationHandler(IServiceProvider serviceProvider, string apikey)
        {
            ServiceProvider = serviceProvider;
            ApiKey = apikey;
            _configurationAccessor = ServiceProvider.GetRequiredService<IConfigurationAccessor>();
            ConfigurationOption = _configurationAccessor.GetValueByKey<TOption>(apikey);
            _httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

            if (ConfigurationOption == null)
                throw new Exception("Girilen apikey ile configuration bilgileri alınamadı");

            TokenClient = _httpClientFactory.CreateClient($"{apikey}_tokenClient");
            TokenClient.BaseAddress = new Uri(ConfigurationOption.Authentication.BaseAddress);
        }
        public abstract Task<AuthenticationTicketInfo> GetToken(HttpRequestMessage request = null, bool forceNewToken = false);
        public abstract Task<AuthenticationTicketInfo> RefreshToken(AuthenticationTicketInfo authenticationTicketInfo);
        public abstract Task AddorEditTokenStorage(AuthenticationTicketInfo authenticationTicketInfo);
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await GetToken(request);

            if (token != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue(ApiConstants.Api_Keys.API_AUTHORIZATION_BEARER, token.access_token);
            }

            var response = await base.SendAsync(request, cancellationToken);

            if (token != null && response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
            {
                if (string.IsNullOrEmpty(token.refresh_token))
                {
                    token = await GetToken(null, true);
                }
                else
                {
                    token = await RefreshToken(token);
                }

                if (token != null)
                {
                    request.Headers.Authorization = new AuthenticationHeaderValue(ApiConstants.Api_Keys.API_AUTHORIZATION_BEARER, token.access_token);

                    await AddorEditTokenStorage(token);
                }

                response = await base.SendAsync(request, cancellationToken);
            }

            return response;
        }
    }
}
