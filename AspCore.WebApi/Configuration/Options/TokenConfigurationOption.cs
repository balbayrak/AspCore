using System;
using System.Collections.Generic;
using System.Text;
using AspCore.WebApi.Security.General;

namespace AspCore.WebApi.Configuration.Options
{
    public class TokenConfigurationOption
    {
        public string configurationKey { get; set; }
        public TokenSettingOption tokenSettingOption { get; set; }
    }
}
