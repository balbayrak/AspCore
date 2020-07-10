using AspCore.Authentication.JWT.Concrete;
using AspCore.Dependency.Abstract;
using Microsoft.IdentityModel.Tokens;

namespace AspCore.Authentication.JWT.Abstract
{
    public interface IJwtHandler
    {
        TokenOption SettingOption { get; }
        TokenValidationParameters Parameters { get; }
        TokenValidationParameters RefreshParameters { get;}
        SigningCredentials SigningCredentials { get; }
        SecurityKey IssuerSigningKey { get; }

    }
}
