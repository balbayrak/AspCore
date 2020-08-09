using AspCore.Dependency.Abstract;
using AspCore.Entities.EntityType;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace AspCore.Security.User
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
