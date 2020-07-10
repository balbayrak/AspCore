using AspCore.Authentication.JWT.Abstract;
using AspCore.Authentication.JWT.Concrete;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace AspCore.DataSearchApi.ElasticSearch.Authentication
{
    public class ElasticSearchApiTokenGenerator : JwtGenerator<ElasticSearchApiJWTInfo>, ITokenGenerator<ElasticSearchApiJWTInfo>
    {
        public ElasticSearchApiTokenGenerator(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override IEnumerable<Claim> GetJWTClaims(ElasticSearchApiJWTInfo jwtInfo)
        {
            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.UserData, JsonConvert.SerializeObject(jwtInfo)));

            return claims;
        }
    }
}
