using AspCore.WebApi.Authentication.Abstract;
using AspCore.WebApi.Authentication.JWT.Concrete;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace AspCore.DataSearchApi.ElasticSearch.Authentication
{
    public class ElasticSearchApiTokenGenerator : JwtGenerator<ElasticSearchApiJWTInfo>, ITokenGenerator<ElasticSearchApiJWTInfo>
    {
        public ElasticSearchApiTokenGenerator(string configurationKey, TokenSettingOption tokenOption = null) : base(configurationKey, tokenOption)
        {
        }

        public override IEnumerable<Claim> GetJWTClaims(ElasticSearchApiJWTInfo jwtInfo)
        {
            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.UserData, JsonConvert.SerializeObject(jwtInfo)));

            return claims;
        }

        public override ElasticSearchApiJWTInfo GetJWTInfoObject(IEnumerable<Claim> claims)
        {
            string clientData = claims.FirstOrDefault(t => t.Type == ClaimTypes.UserData)?.Value;

            if (!string.IsNullOrEmpty(clientData))
            {
                return JsonConvert.DeserializeObject<ElasticSearchApiJWTInfo>(clientData);
            }

            return null;
        }
    }
}
