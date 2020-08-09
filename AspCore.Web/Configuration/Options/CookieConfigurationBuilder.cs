using AspCore.ConfigurationAccess.Abstract;

namespace AspCore.Web.Configuration.Options
{
    public class CookieConfigurationBuilder
    {
        public readonly AuthCookieOption cookieOption;
        private readonly IConfigurationAccessor _configurationAccessor;
        public CookieConfigurationBuilder(IConfigurationAccessor configurationAccessor, string configurationKey)
        {
            _configurationAccessor = configurationAccessor;
            cookieOption = _configurationAccessor.GetValueByKey<AuthCookieOption>(configurationKey);
        }

        public CookieConfigurationBuilder(AuthCookieOption cacheOption)
        {
            this.cookieOption = cacheOption;
        }
    }
}
