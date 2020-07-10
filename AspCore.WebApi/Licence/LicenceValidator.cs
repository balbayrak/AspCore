using AspCore.Authentication.JWT.Concrete;
using AspCore.Entities.General;
using AspCore.Entities.Licence;
using AspCore.Extension;
using AspCore.WebApi.Authentication.General;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Security;
using System;
using System.Text;
using System.Xml.Linq;

namespace AspCore.WebApi.Licence
{
    internal sealed class LicenceValidator : ILicenceValidator
    {
        private readonly string signatureAlgorithm = X9ObjectIdentifiers.ECDsaWithSha512.Id;
        private const string Signature_Str = "Signature";

        static LicenceValidator _instance;

        public static LicenceValidator Instance
        {
            get { return _instance ?? (_instance = new LicenceValidator()); }
        }
        private LicenceValidator()
        {

        }

        private LicenceValidator(IServiceProvider serviceProvider)
        {
            isControlled = false;
            _serviceProvider = serviceProvider;
        }
        public static void Init(IServiceProvider services)
        {
            if (_instance == null)
                _instance = new LicenceValidator(services);
        }

        private readonly IServiceProvider _serviceProvider;

        private bool isControlled { get; set; }

        public ServiceResult<bool> CheckLicenceWebApi(string licenceStr)
        {
            ServiceResult<bool> serviceResult = new ServiceResult<bool>();
            if (!isControlled)
            {
                try
                {
                    var base64EncodedBytes = Convert.FromBase64String(licenceStr);
                    string xmlLic = Encoding.UTF8.GetString(base64EncodedBytes);

                    XElement xmlData = XElement.Parse(xmlLic, LoadOptions.None);
                    using (AspCoreLicence license = new AspCoreLicence(xmlData))
                    {
                        bool decLicence = VerifySignature(xmlData);

                        if (license.Expiration > DateTime.Now)
                        {
                            serviceResult.ErrorMessage = SecurityConstants.LICENCE.LICENCE_EXPIRED_ERROR;
                        }
                    }
                }
                catch (Exception ex)
                {
                    serviceResult.ErrorMessage(SecurityConstants.LICENCE.LICENCE_VALIDATOR_ERROR, ex);
                }

                if (string.IsNullOrEmpty(serviceResult.ErrorMessage))
                {
                    serviceResult.IsSucceeded = true;
                    serviceResult.Result = true;
                }
            }
            else
            {
                serviceResult.IsSucceeded = true;
                serviceResult.Result = true;
            }
            return serviceResult;
        }

        public ServiceResult<bool> CheckLicenceWeb(string licenceStr)
        {
            ServiceResult<bool> serviceResult = new ServiceResult<bool>();
            if (!isControlled)
            {
                try
                {
                    string requestUrl = string.Empty;
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var httpContextAccessor = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>();
                        requestUrl = httpContextAccessor.HttpContext.Request.Host.Value;
                    }


                    var base64EncodedBytes = Convert.FromBase64String(licenceStr);
                    string xmlLic = Encoding.UTF8.GetString(base64EncodedBytes);

                    XElement xmlData = XElement.Parse(xmlLic, LoadOptions.None);
                    using (AspCoreLicence license = new AspCoreLicence(xmlData))
                    {
                        bool decLicence = VerifySignature(xmlData);

                        if (license.Expiration > DateTime.Now)
                        {
                            serviceResult.ErrorMessage = SecurityConstants.LICENCE.LICENCE_EXPIRED_ERROR;
                        }

                        if (!string.IsNullOrEmpty(requestUrl))
                        {
                            if (!requestUrl.Contains(license.ProjectUrl))
                                serviceResult.ErrorMessage = SecurityConstants.LICENCE.LICENCE_URL_ERROR;
                        }
                        else
                        {
                            serviceResult.ErrorMessage = SecurityConstants.LICENCE.APPLICATION_URL_GET_ERROR;
                        }
                    }
                }
                catch (Exception ex)
                {
                    serviceResult.ErrorMessage(SecurityConstants.LICENCE.LICENCE_VALIDATOR_ERROR, ex);
                }

                if (string.IsNullOrEmpty(serviceResult.ErrorMessage)) serviceResult.IsSucceeded = true;
            }
            else
            {
                serviceResult.IsSucceeded = true;
                serviceResult.Result = true;
            }
            return serviceResult;
        }

        private bool VerifySignature(XElement xmlData)
        {
            var signTag = xmlData.Element(Signature_Str);

            if (signTag == null)
                return false;

            try
            {
                signTag.Remove();

                var pubKey = KeyFactory.FromPublicKeyString(SecurityConstants.LICENCE.PUBLIC_KEY);

                var documentToSign = Encoding.UTF8.GetBytes(xmlData.ToString(SaveOptions.DisableFormatting));
                var signer = SignerUtilities.GetSigner(signatureAlgorithm);
                signer.Init(false, pubKey);
                signer.BlockUpdate(documentToSign, 0, documentToSign.Length);

                return signer.VerifySignature(Convert.FromBase64String(signTag.Value));
            }
            finally
            {
                xmlData.Add(signTag);
            }
        }
    }
}
