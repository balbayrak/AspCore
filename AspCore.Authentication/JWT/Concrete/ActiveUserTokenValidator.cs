using AspCore.Authentication.JWT.Abstract;
using AspCore.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace AspCore.Authentication.JWT.Concrete
{
    public class ActiveUserTokenValidator : JwtValidator<ActiveUser>, ITokenValidator<ActiveUser>
    {
        public ActiveUserTokenValidator(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
        public override ActiveUser GetJWTInfoObject(IEnumerable<Claim> claims)
        {
            return new ActiveUser
            {
                id = new Guid(claims.FirstOrDefault(t => t.Type == ClaimTypes.NameIdentifier)?.Value),
                authenticatedUserId = new Guid(claims.FirstOrDefault(t => t.Type == ClaimTypes.NameIdentifier)?.Value),
                job = claims.FirstOrDefault(t => t.Type == ClaimTypes.Actor)?.Value,
                email = claims.FirstOrDefault(t => t.Type == ClaimTypes.Email)?.Value,
                jobCompany = claims.FirstOrDefault(t => t.Type == ClaimTypes.Locality)?.Value,
                name = claims.FirstOrDefault(t => t.Type == ClaimTypes.Name)?.Value,
                tckn = claims.FirstOrDefault(t => t.Type == ClaimTypes.SerialNumber)?.Value,
                telephone = claims.FirstOrDefault(t => t.Type == ClaimTypes.HomePhone)?.Value,
                address = claims.FirstOrDefault(t => t.Type == ClaimTypes.StreetAddress)?.Value,
            };
        }
    }
}
