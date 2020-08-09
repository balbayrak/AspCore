using AspCore.ApiClient.Abstract;
using AspCore.Dependency.Abstract;
using AspCore.Entities.Authentication;
using System;

namespace AspCore.BackendForFrontend.Abstract
{
    public interface IBffLayer : ITransientType
    {
        IBffApiClient ApiClient { get; }

    }
}
