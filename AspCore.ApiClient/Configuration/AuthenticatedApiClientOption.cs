using AspCore.ApiClient.Handlers;

namespace AspCore.ApiClient.Configuration
{
    public class AuthenticatedApiClientOption : ApiClientOption
    {
        public EnumAuthenticationHandler authenticationHandler { get; set; } = EnumAuthenticationHandler.None;
    }
}
