﻿using AspCore.Entities.Configuration;

namespace AspCore.WebApi.Security.General
{
    public class TokenSettingOption : IConfigurationEntity
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public long AccessTokenExpiration { get; set; }
        public string SecurityKey { get; set; }
    }
}
