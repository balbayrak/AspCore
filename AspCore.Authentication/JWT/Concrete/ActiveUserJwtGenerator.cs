using AspCore.Authentication.JWT.Abstract;
using AspCore.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace AspCore.Authentication.JWT.Concrete
{
    public class ActiveUserJwtGenerator : JwtGenerator<ActiveUser>, ITokenGenerator<ActiveUser>
    {
        public ActiveUserJwtGenerator(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override IEnumerable<Claim> GetJWTClaims(ActiveUser jwtInfo)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, jwtInfo.authenticatedUserId.ToString()),
                new Claim(ClaimTypes.Email, jwtInfo.email ?? string.Empty),
                new Claim(ClaimTypes.Name, (jwtInfo.name + " " + jwtInfo.surname) ?? string.Empty),
                new Claim(ClaimTypes.HomePhone, jwtInfo.telephone ?? string.Empty),
                new Claim(ClaimTypes.StreetAddress, jwtInfo.address ?? string.Empty),
                new Claim(ClaimTypes.SerialNumber, jwtInfo.tckn ?? string.Empty),
                new Claim(ClaimTypes.Actor, jwtInfo.job ?? string.Empty),
                new Claim(ClaimTypes.Locality, jwtInfo.jobCompany ?? string.Empty)
            };
            if (jwtInfo.roles != null)
                jwtInfo.roles.ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role ?? string.Empty)));

            return claims;
        }
    }
}
