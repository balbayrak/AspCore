using AspCore.Utilities.DataProtector;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace AspCore.Web.Configuration.Options
{
    public class ConfigureCookieOption : IConfigureNamedOptions<CookieAuthenticationOptions>
    {
        private readonly IDataProtectorHelper _dataProtectorHelper;
        private readonly CookieConfigurationBuilder _cookiConfigurationBuilder;
        public ConfigureCookieOption(IDataProtectorHelper dataProtectorHelper, CookieConfigurationBuilder cookiConfigurationBuilder)
        {
            _dataProtectorHelper = dataProtectorHelper;
            _cookiConfigurationBuilder = cookiConfigurationBuilder;
        }
        public void Configure(string name, CookieAuthenticationOptions options)
        {
            options.Cookie.Name = _cookiConfigurationBuilder.cookieOption.CookieName;
            options.Cookie.HttpOnly = true;
            options.Cookie.SecurePolicy = _cookiConfigurationBuilder.cookieOption.IsSecureCookie ? CookieSecurePolicy.Always : CookieSecurePolicy.None;
            options.Cookie.SameSite = SameSiteMode.Lax;
            options.DataProtectionProvider = DataProtectionProvider.Create(this._dataProtectorHelper.secretKey);
            options.SlidingExpiration = true;
        }

        public void Configure(CookieAuthenticationOptions options)
        {
            Configure(CookieAuthenticationDefaults.AuthenticationScheme, options);
        }
    }
}
