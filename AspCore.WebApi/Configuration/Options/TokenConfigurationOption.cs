using AspCore.WebApi.Authentication.JWT.Concrete;

namespace AspCore.WebApi.Configuration.Options
{
    public class TokenConfigurationOption
    {
        public string configurationKey { get; set; }
        public TokenSettingOption tokenSettingOption { get; set; }
    }
}
