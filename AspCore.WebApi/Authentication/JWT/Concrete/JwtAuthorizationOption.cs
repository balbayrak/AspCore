using AspCore.ConfigurationAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.WebApi.Authentication.JWT.Concrete
{
    public class JwtAuthorizationOption
    {
        private readonly IConfigurationAccessor configurationAccessor;
        public TokenSettingOption _tokenSettingOption { get; private set; }

        public JwtAuthorizationOption(IConfigurationAccessor config, string configurationKey)
        {
            this.configurationAccessor = config;
            _tokenSettingOption = configurationAccessor.GetValueByKey<TokenSettingOption>(configurationKey);
        }
    }
}
