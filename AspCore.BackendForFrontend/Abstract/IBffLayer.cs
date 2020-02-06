using System;
using AspCore.ApiClient.Entities.Concrete;
using AspCore.Dependency.Abstract;

namespace AspCore.BackendForFrontend.Abstract
{
    public interface IBFFLayer : ITransientType
    {
        IBffApiClient apiClient { get; }

        void SetApiClientTokenStorageKey(string tokenStorageKey);

        void SetApiClientTokenStorageExpireTime(DateTime expireTime);

        void SetAuthenticationToken(string key, AuthenticationTokenResponse authenticationToken);
    }
}
