using AspCore.ApiClient.Abstract;

namespace AspCore.BackendForFrontend.Abstract
{
    public interface IBffApiClient : IApiClient
    {
        string apiClientKey { get; set; }
    }
}
