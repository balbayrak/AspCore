using AspCore.ApiClient.Abstract;
using AspCore.Dependency.Abstract;
using AspCore.Entities.Authentication;
using System;

namespace AspCore.BackendForFrontend.Abstract
{
    public interface IBffLayer : ITransientType
    {
        IBffApiClient ApiClient { get; }

        void SetApiClientTokenStorageKey(string tokenStorageKey);

        void SetApiClientTokenStorageExpireTime(DateTime expireTime);

        void SetAuthenticationToken(string key, AuthenticationToken authenticationToken);
    }
}
