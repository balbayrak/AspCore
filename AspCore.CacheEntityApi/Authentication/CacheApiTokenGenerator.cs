using AspCore.WebApi.Authentication.Abstract;
using AspCore.WebApi.Authentication.JWT.Concrete;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace AspCore.CacheEntityApi.Authentication
{
    public class CacheApiTokenGenerator : JwtGenerator<CacheApiJWTInfo>, ITokenGenerator<CacheApiJWTInfo>
    {
        public CacheApiTokenGenerator(string configurationKey, TokenSettingOption tokenOption = null) : base(configurationKey, tokenOption)
        {
        }

        public override IEnumerable<Claim> GetJWTClaims(CacheApiJWTInfo jwtInfo)
        {
            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.UserData, JsonConvert.SerializeObject(jwtInfo)));

            return claims;
        }

        public override CacheApiJWTInfo GetJWTInfoObject(IEnumerable<Claim> claims)
        {
            string clientData = claims.FirstOrDefault(t => t.Type == ClaimTypes.UserData)?.Value;

            if (!string.IsNullOrEmpty(clientData))
            {
                return JsonConvert.DeserializeObject<CacheApiJWTInfo>(clientData);
            }

            return null;
        }
    }
}
