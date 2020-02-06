using AspCore.ApiClient.Abstract;

namespace AspCore.BackendForFrontend.Abstract
{
    public interface IBffApiClient : IAuthenticatedApiClient
    {
        string apiClientKey { get; set; }
    }
}
