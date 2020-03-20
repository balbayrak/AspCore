using AspCore.ConfigurationAccess.Abstract;
using AspCore.Dependency.Concrete;
using AspCore.Entities.Authentication;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Extension;
using AspCore.Utilities;
using AspCore.WebApi.Authentication.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace AspCore.WebApi.Authentication.JWT.Concrete
{
    public abstract class JwtGenerator<TJWTInfo> : ITokenGenerator<TJWTInfo>
        where TJWTInfo : class, IJWTEntity, new()
    {
        private IHttpContextAccessor _httpContextAccessor;
        private IConfigurationAccessor _configurationHelper { get; }

        protected TokenSettingOption _tokenOption { get; set; }

        private DateTime _accessTokenExpiration => DateTime.Now.AddMinutes(Convert.ToDouble(_tokenOption.AccessTokenExpiration));

        public JwtGenerator(string configurationKey, TokenSettingOption tokenOption = null)
        {
            _httpContextAccessor = DependencyResolver.Current.GetService<IHttpContextAccessor>();
            _configurationHelper = DependencyResolver.Current.GetService<IConfigurationAccessor>(); ;

            if (tokenOption == null && !string.IsNullOrEmpty(configurationKey))
            {
                _tokenOption = _configurationHelper.GetValueByKey<TokenSettingOption>(configurationKey);
                if (_tokenOption == null)
                {
                    throw new Exception(SecurityConstants.TOKEN_SETTING_OPTIONS.TOKEN_SETTING_KEY_IS_NULL_EXCEPTION);
                }
            }
            else if (tokenOption != null && string.IsNullOrEmpty(configurationKey))
            {
                _tokenOption = tokenOption;
            }
        }

        public ServiceResult<AuthenticationToken> CreateToken(TJWTInfo jwtInfo)
        {
            ServiceResult<AuthenticationToken> serviceResult = new ServiceResult<AuthenticationToken>();
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOption.SecurityKey));
                var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

                var jwt = CreateJwtSecurityToken(jwtInfo, signingCredentials);
                var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                var token = jwtSecurityTokenHandler.WriteToken(jwt);

                serviceResult.IsSucceeded = true;
                serviceResult.Result = new AuthenticationToken();
                serviceResult.Result.access_token = token;
                serviceResult.Result.expires = _accessTokenExpiration;
                serviceResult.Result.refresh_token = EncryptionHelper.Encrypt(token, _tokenOption.SecurityKey).Substring(0, 10);

            }
            catch (Exception ex)
            {
                serviceResult.ErrorMessage(SecurityConstants.TOKEN_SETTING_OPTIONS.TOKEN_CREATE_ERROR, ex);
            }

            return serviceResult;
        }

        public ServiceResult<AuthenticationToken> RefreshToken(AuthenticationToken token)
        {
            ServiceResult<AuthenticationToken> serviceResult = new ServiceResult<AuthenticationToken>();
            try
            {
                var cnt = EncryptionHelper.Encrypt(token.access_token, _tokenOption.SecurityKey).Substring(0, 10);
                if (cnt == token.refresh_token)
                {
                    ServiceResult<TJWTInfo> activeUserResult = GetClaimsFromExpiredToken(token);

                    if (activeUserResult.IsSucceededAndDataIncluded())
                    {
                        return CreateToken(activeUserResult.Result);
                    }
                    else
                    {
                        serviceResult.ErrorMessage = SecurityConstants.TOKEN_SETTING_OPTIONS.ACTIVE_USER_INFO_NOT_FOUND;
                    }
                }
                else
                {
                    serviceResult.ErrorMessage = SecurityConstants.TOKEN_SETTING_OPTIONS.REFRESH_TOKEN_IS_INVALID;
                }
            }
            catch (Exception ex)
            {
                serviceResult.ErrorMessage(SecurityConstants.TOKEN_SETTING_OPTIONS.TOKEN_REFRESH_ERROR, ex);
            }

            return serviceResult;
        }

        public ServiceResult<TJWTInfo> GetJWTInfo(AuthenticationToken token)
        {
            ServiceResult<TJWTInfo> serviceResult = new ServiceResult<TJWTInfo>();
            try
            {
                serviceResult = GetClaimsFromExpiredToken(token);
            }
            catch (Exception ex)
            {
                serviceResult.ErrorMessage(SecurityConstants.TOKEN_SETTING_OPTIONS.TOKEN_REFRESH_ERROR, ex);
            }

            return serviceResult;
        }

        private ServiceResult<TJWTInfo> GetClaimsFromExpiredToken(AuthenticationToken token)
        {
            ServiceResult<TJWTInfo> serviceResult = new ServiceResult<TJWTInfo>();
            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOption.SecurityKey)),
                    ValidateLifetime = false
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token.access_token, tokenValidationParameters, out securityToken);
                var jwtSecurityToken = securityToken as JwtSecurityToken;
                if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Contains("Hmac-Sha256", StringComparison.InvariantCultureIgnoreCase))
                    throw new SecurityTokenException("Invalid token");

                serviceResult.IsSucceeded = true;
                serviceResult.Result = GetJWTInfoObject(principal.Claims);

                if (serviceResult.IsSucceeded && serviceResult.Result != null)
                {
                    serviceResult.Result.correlationId = principal.Claims.FirstOrDefault(t => t.Type == ClaimTypes.Hash)?.Value;
                }
            }
            catch (Exception ex)
            {
                serviceResult.ErrorMessage(SecurityConstants.TOKEN_SETTING_OPTIONS.TOKEN_REFRESH_ERROR, ex);
            }

            return serviceResult;
        }

        private JwtSecurityToken CreateJwtSecurityToken(TJWTInfo jwtInfo, SigningCredentials signingCredentials)
        {
            IEnumerable<Claim> claims = GetJWTClaims(jwtInfo);

            if (claims != null)
            {
                string correlationID = _httpContextAccessor.HttpContext.GetHeaderValue(HttpContextConstant.HEADER_KEY.CORRELATION_ID);

                if (string.IsNullOrEmpty(correlationID))
                {
                    correlationID = Guid.NewGuid().ToString();
                }

                claims.ToList().Add(new Claim(ClaimTypes.Hash, correlationID));
            }

            var jwt = new JwtSecurityToken(
                issuer: _tokenOption.Issuer,
                audience: _tokenOption.Audience,
                expires: _accessTokenExpiration,
                notBefore: DateTime.Now,
                claims: claims,
                signingCredentials: signingCredentials
            );
            return jwt;
        }

        public abstract IEnumerable<Claim> GetJWTClaims(TJWTInfo jwtInfo);

        public abstract TJWTInfo GetJWTInfoObject(IEnumerable<Claim> claims);

    }
}
