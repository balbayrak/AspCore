using AspCore.Entities.Configuration;

namespace AspCore.Web.Configuration.Options
{
    public class AuthCookieOption : IConfigurationEntity
    {
        public string CookieName { get; set; }

        /// <summary>
        /// must be true in production
        /// </summary>
        public bool IsSecureCookie { get; set; }

        /// <summary>
        /// as minutes
        /// </summary>
        public int Expire { get; set; }
    }
}
