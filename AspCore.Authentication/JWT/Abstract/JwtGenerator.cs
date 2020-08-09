using AspCore.Authentication.JWT.Concrete;
using AspCore.ConfigurationAccess.Abstract;
using AspCore.Entities.Authentication;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Extension;
using AspCore.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AspCore.Authentication.JWT.Abstract
{
    public abstract class JwtGenerator<TJWTInfo>
        where TJWTInfo : class, IJWTEntity, new()
    {

        private DateTime _accessTokenExpiration => DateTime.Now.AddMinutes(Convert.ToDouble(_jwtHandler.SettingOption.AccessTokenExpiration));

        private ITokenValidator<TJWTInfo> _tokenValidator;
        private IJwtHandler _jwtHandler;
        protected IServiceProvider ServiceProvider { get; private set; }
        public JwtGenerator(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            _jwtHandler = ServiceProvider.GetRequiredService<IJwtHandler>();
            _tokenValidator = ServiceProvider.GetRequiredService<ITokenValidator<TJWTInfo>>();
        }

        public ServiceResult<AuthenticationTicketInfo> CreateToken(TJWTInfo jwtInfo)
        {
            ServiceResult<AuthenticationTicketInfo> serviceResult = new ServiceResult<AuthenticationTicketInfo>();
            try
            {

                var jwt = CreateJwtSecurityToken(jwtInfo);
                var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                var token = jwtSecurityTokenHandler.WriteToken(jwt);


                serviceResult.IsSucceeded = true;
                serviceResult.Result = new AuthenticationTicketInfo();
                serviceResult.Result.access_token = token;
                serviceResult.Result.expires = _accessTokenExpiration;
                serviceResult.Result.refresh_token = EncryptionHelper.Encrypt(token, _jwtHandler.SettingOption.PrivateKey).Substring(0, 10);

            }
            catch (Exception ex)
            {
                serviceResult.ErrorMessage(AuthenticationConstants.TOKEN_SETTING_OPTIONS.TOKEN_CREATE_ERROR, ex);
            }

            return serviceResult;
        }

        public ServiceResult<AuthenticationTicketInfo> RefreshToken(AuthenticationTicketInfo token)
        {
            ServiceResult<AuthenticationTicketInfo> serviceResult = new ServiceResult<AuthenticationTicketInfo>();
            try
            {
                var cnt = EncryptionHelper.Encrypt(token.access_token, _jwtHandler.SettingOption.PrivateKey).Substring(0, 10);
                if (cnt == token.refresh_token)
                {
                    ServiceResult<TJWTInfo> activeUserResult = _tokenValidator.Validate(token, true);

                    if (activeUserResult.IsSucceededAndDataIncluded())
                    {
                        return CreateToken(activeUserResult.Result);
                    }
                    else
                    {
                        serviceResult.ErrorMessage = AuthenticationConstants.TOKEN_SETTING_OPTIONS.ACTIVE_USER_INFO_NOT_FOUND;
                    }
                }
                else
                {
                    serviceResult.ErrorMessage = AuthenticationConstants.TOKEN_SETTING_OPTIONS.REFRESH_TOKEN_IS_INVALID;
                }
            }
            catch (Exception ex)
            {
                serviceResult.ErrorMessage(AuthenticationConstants.TOKEN_SETTING_OPTIONS.TOKEN_REFRESH_ERROR, ex);
            }

            return serviceResult;
        }

        private JwtSecurityToken CreateJwtSecurityToken(TJWTInfo jwtInfo)
        {
            IEnumerable<Claim> claims = GetJWTClaims(jwtInfo);

            var jwt = new JwtSecurityToken(
                issuer: _jwtHandler.SettingOption.Issuer,
                audience: _jwtHandler.SettingOption.Audience,
                expires: _accessTokenExpiration,
                notBefore: DateTime.Now,
                claims: claims,
                signingCredentials: _jwtHandler.SigningCredentials
            );
            return jwt;
        }

        public abstract IEnumerable<Claim> GetJWTClaims(TJWTInfo jwtInfo);

    }
}
