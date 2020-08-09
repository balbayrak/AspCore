using AspCore.Authentication.JWT.Concrete;
using AspCore.ConfigurationAccess.Abstract;
using AspCore.Entities.Authentication;
using AspCore.Entities.EntityType;
using AspCore.Entities.General;
using AspCore.Extension;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;

namespace AspCore.Authentication.JWT.Abstract
{
    public abstract class JwtValidator<TJWTInfo>
        where TJWTInfo : class, IJWTEntity, new()
    {
        private IJwtHandler _jwtHandler;
        protected IServiceProvider ServiceProvider { get; private set; }
        public JwtValidator(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            _jwtHandler = ServiceProvider.GetRequiredService<IJwtHandler>();
        }

       public bool ValidatePublicKey
        {
            get
            {
                return _jwtHandler.SettingOption.UseAsymmetricAlg;
            }
        }

        public ServiceResult<TJWTInfo> Validate(AuthenticationTicketInfo token, bool isExpiredToken)
        {
            ServiceResult<TJWTInfo> serviceResult = new ServiceResult<TJWTInfo>();
            try
            {
                serviceResult = GetClaimsFromExpiredToken(token, isExpiredToken);
            }
            catch (Exception ex)
            {
                serviceResult.ErrorMessage(AuthenticationConstants.TOKEN_SETTING_OPTIONS.TOKEN_REFRESH_ERROR, ex);
            }

            return serviceResult;
        }

        public ServiceResult<List<Claim>> GetClaims(AuthenticationTicketInfo token)
        {
            ServiceResult<List<Claim>> serviceResult = new ServiceResult<List<Claim>>();
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                SecurityToken securityToken;
                ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(token.access_token, _jwtHandler.RefreshParameters, out securityToken);
                serviceResult.Result = claimsPrincipal.Claims.ToList();
                serviceResult.IsSucceeded = true;
                tokenHandler = null;
            }
            catch (Exception ex)
            {
                serviceResult.ErrorMessage(AuthenticationConstants.TOKEN_SETTING_OPTIONS.TOKEN_VALIDATE_EXCEPTION, ex);
            }

            return serviceResult;
        }

        private ServiceResult<TJWTInfo> GetClaimsFromExpiredToken(AuthenticationTicketInfo token, bool isExpiredToken)
        {
            ServiceResult<TJWTInfo> serviceResult = new ServiceResult<TJWTInfo>();
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                SecurityToken securityToken;
                ClaimsPrincipal claimsPrincipal = null;
                if (isExpiredToken)
                    claimsPrincipal = tokenHandler.ValidateToken(token.access_token, _jwtHandler.RefreshParameters, out securityToken);
                else
                    claimsPrincipal = tokenHandler.ValidateToken(token.access_token, _jwtHandler.Parameters, out securityToken);


                serviceResult.IsSucceeded = true;
                serviceResult.Result = GetJWTInfoObject(claimsPrincipal.Claims);

            }
            catch (Exception ex)
            {
                serviceResult.ErrorMessage(AuthenticationConstants.TOKEN_SETTING_OPTIONS.TOKEN_REFRESH_ERROR, ex);
            }

            return serviceResult;
        }

        public abstract TJWTInfo GetJWTInfoObject(IEnumerable<Claim> claims);
    }
}
