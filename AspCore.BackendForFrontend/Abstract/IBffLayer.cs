using System;
using AspCore.ApiClient.Entities.Concrete;
using AspCore.Dependency.Abstract;
using AspCore.Entities.Authentication;

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
