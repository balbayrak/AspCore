using AspCore.Dependency.Abstract;
using System;
using System.Security.Claims;

namespace AspCore.BackendForFrontend.Concrete.Security.User
{
    public interface ICurrentUser : ITransientType
        
    {
        Guid? Id { get; }

        string UserName { get; }

        TJWTInfo UserInfo<TJWTInfo>();

        bool IsAuthenticated { get; }

        Claim FindClaim(string claimType);

        Claim[] FindClaims(string claimType);

        bool IsInRole(string roleName);
    }
}
