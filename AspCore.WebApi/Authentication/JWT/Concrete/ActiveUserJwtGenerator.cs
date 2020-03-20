using AspCore.Entities.User;
using AspCore.WebApi.Security.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace AspCore.WebApi.Authentication.JWT.Concrete
{
    public class ActiveUserJwtGenerator : JwtGenerator<ActiveUser>, IActiveUserTokenGenerator
    {
        public ActiveUserJwtGenerator(string configurationKey, TokenSettingOption tokenOption = null) : base(configurationKey, tokenOption)
        {
        }


        public override IEnumerable<Claim> GetJWTClaims(ActiveUser jwtInfo)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, jwtInfo.id.ToString()),
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

        public override ActiveUser GetJWTInfoObject(IEnumerable<Claim> claims)
        {
            return new ActiveUser
            {
                id = new Guid(claims.FirstOrDefault(t => t.Type == ClaimTypes.NameIdentifier)?.Value),
                activeUserId = new Guid(claims.FirstOrDefault(t => t.Type == ClaimTypes.NameIdentifier)?.Value),
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
