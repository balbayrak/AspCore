using AspCore.Authentication.JWT.Abstract;
using AspCore.Authentication.JWT.Concrete;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace AspCore.DataSearchApi.ElasticSearch.Authentication
{
    public class ElasticSearchApiTokenValidator : JwtValidator<ElasticSearchApiJWTInfo>, ITokenValidator<ElasticSearchApiJWTInfo>
    {
        public ElasticSearchApiTokenValidator(IServiceProvider serviceProvider) : base(serviceProvider)
        {
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
