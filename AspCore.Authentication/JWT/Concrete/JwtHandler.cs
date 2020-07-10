using AspCore.Authentication.JWT.Abstract;
using AspCore.ConfigurationAccess.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace AspCore.Authentication.JWT.Concrete
{
    public class JwtHandler : IJwtHandler
    {
        public TokenOption SettingOption { get; private set; }
        public SecurityKey IssuerSigningKey { get; private set; }
        public SigningCredentials SigningCredentials { get; private set; }
        public TokenValidationParameters Parameters { get; private set; }

        public TokenValidationParameters RefreshParameters { get; private set; }

        public JwtHandler(IConfigurationAccessor configurationAccessor, TokenConfigurationOption configurationOption)
        {
            if (configurationOption.tokenOption != null)
            {
                SettingOption = configurationOption.tokenOption;
            }

            if (!string.IsNullOrEmpty(configurationOption.configurationKey))
            {
                SettingOption = configurationAccessor.GetValueByKey<TokenOption>(configurationOption.configurationKey);
            }

            if (SettingOption == null)
            {
                throw new Exception(AuthenticationConstants.TOKEN_SETTING_OPTIONS.TOKEN_SETTING_KEY_IS_NULL_EXCEPTION);
            }

            if (SettingOption.UseAsymmetricAlg)
            {
                InitializeRsa();
            }
            else
            {
                InitializeHmac();
            }

            InitializeJwtParameters();
        }

        private void InitializeRsa()
        {
            RSA publicRsa = RSA.Create();

            publicRsa.ImportRSAPublicKey(
            source: Convert.FromBase64String(SettingOption.PublicKey),
            bytesRead: out int _);
            IssuerSigningKey = new RsaSecurityKey(publicRsa);

            if (!string.IsNullOrEmpty(SettingOption.PrivateKey))
            {
                RSA privateRsa = RSA.Create();

                privateRsa.ImportRSAPrivateKey( // Convert the loaded key from base64 to bytes.
                source: Convert.FromBase64String(SettingOption.PrivateKey), // Use the private key to sign tokens
                bytesRead: out int _); // Discard the out variable 
                var privateKey = new RsaSecurityKey(privateRsa);
                SigningCredentials = new SigningCredentials(privateKey, SecurityAlgorithms.RsaSha256);

            }
        }

        private void InitializeHmac()
        {
            if (!string.IsNullOrEmpty(SettingOption.PrivateKey))
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SettingOption.PrivateKey));
                SigningCredentials = new SigningCredentials(IssuerSigningKey, SecurityAlgorithms.HmacSha256);
            }

        }

        private void InitializeJwtParameters()
        {
            Parameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = IssuerSigningKey,
                ValidateLifetime = true,
                ValidAudience = SettingOption.Audience,
                ValidIssuer = SettingOption.Issuer,
                ClockSkew = TimeSpan.Zero
            };


            RefreshParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = IssuerSigningKey,
                ValidateLifetime = false,
                ValidAudience = SettingOption.Audience,
                ValidIssuer = SettingOption.Issuer,
                ClockSkew = TimeSpan.Zero
            };
        }
    }
}
