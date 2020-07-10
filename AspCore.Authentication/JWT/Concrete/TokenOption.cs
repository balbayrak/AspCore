using AspCore.Entities.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.Authentication.JWT.Concrete
{
    public class TokenOption : IConfigurationEntity
    {
        public string PublicKey { get; set; }
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public bool UseAsymmetricAlg { get; set; }
        public long AccessTokenExpiration { get; set; }
        public string PrivateKey { get; set; }
    }
}
