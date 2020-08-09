using AspCore.Entities.EntityType;
using AspCore.Security.Claims;
using AspCore.Utilities;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace AspCore.Security.User
{
    public class CurrentUser : ICurrentUser
    {
        private static readonly Claim[] EmptyClaimsArray = new Claim[0];

        public TJWTInfo UserInfo<TJWTInfo>()
        {

            var jsonValue = this.FindClaimValue(AspCoreSecurityType.UserInfo)?.UnCompressString();
            if (!string.IsNullOrEmpty(jsonValue))
            {
                return JsonConvert.DeserializeObject<TJWTInfo>(jsonValue);
            }

            return default(TJWTInfo);
        }

        public virtual bool IsAuthenticated => Id.HasValue;

        public virtual Guid? Id => _principals?.FindUserId();

        public virtual string UserName => this.FindClaimValue(AspCoreSecurityType.UserName);

        private readonly ClaimsPrincipal _principals;

        public CurrentUser(IPrincipal principal)
        {
            _principals = principal as ClaimsPrincipal;
        }

        public virtual Claim FindClaim(string claimType)
        {
            return _principals?.FindFirst(c => c.Type == claimType);
        }

        public virtual Claim[] FindClaims(string claimType)
        {
            return _principals?.Claims.Where(c => c.Type == claimType).ToArray() ?? EmptyClaimsArray;
        }

        public virtual bool IsInRole(string roleName)
        {
            return FindClaims(AspCoreSecurityType.Role).Any(c => c.Value == roleName);
        }
    }
}
