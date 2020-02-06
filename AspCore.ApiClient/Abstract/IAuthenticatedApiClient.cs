using System;
using System.Collections.Generic;
using System.Text;
using AspCore.ApiClient.Entities.Abstract;

namespace AspCore.ApiClient.Abstract
{
    public interface IAuthenticatedApiClient : IApiClient
    {
        string tokenStorageKey { get; set; }

        DateTime? tokenStrorageExpireTime { get; set; }
    }
}
