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
        protected readonly TOption _configurationOption;
        protected readonly HttpClient _tokenClient;
        private IHttpClientFactory _httpClientFactory;
        public AspCoreAuthenticationHandler(IServiceProvider serviceProvider, string apikey)
        {
            ServiceProvider = serviceProvider;
            _configurationAccessor = ServiceProvider.GetRequiredService<IConfigurationAccessor>();
            _configurationOption = _configurationAccessor.GetValueByKey<TOption>(apikey);
            _httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

            if (_configurationOption == null)
                throw new Exception("Girilen apikey ile configuration bilgileri alınamadı");

            _tokenClient = _httpClientFactory.CreateClient($"{apikey}_tokenClient");
            _tokenClient.BaseAddress = new Uri(_configurationOption.Authentication.BaseAddress);
        }
        public abstract Task<AuthenticationTicketInfo> GetToken();
        public abstract Task<AuthenticationTicketInfo> RefreshToken(AuthenticationTicketInfo authenticationTicketInfo);
        public abstract Task AddorEditTokenStorage(AuthenticationTicketInfo authenticationTicketInfo);
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await GetToken();

            if (token != null)
            {
                request.Headers.Authorization = new AuthenticationHeaderValue(ApiConstants.Api_Keys.API_AUTHORIZATION_BEARER, token.access_token);
            }

            var response = await base.SendAsync(request, cancellationToken);

            if (token != null && response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.Forbidden)
            {
                token = await RefreshToken(token);

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
