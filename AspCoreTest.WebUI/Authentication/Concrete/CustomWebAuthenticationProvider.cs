using AspCore.Entities.Authentication;
using AspCore.Entities.General;
using AspCore.Extension;
using AspCoreTest.Authentication.Abstract;
using System;

namespace AspCoreTest.Authentication.Concrete
{
    public class CustomWebAuthenticationProvider : ICustomWebAuthenticationProvider
    {

        public string loginPageUrl => "/Account/UserLogin";
        public string firstPageUrl => "/Account/Home";

        public string mainPageUrl => "/Home/Index";

        public string apiAuthenticationType => "CustomApiAuthenticationProvider";

        public CustomWebAuthenticationProvider()
        {
        }


        public ServiceResult<AuthenticationInfo> GetAuthenticationFormInfo(AuthenticationInfo input)
        {
            ServiceResult<AuthenticationInfo> serviceResult = new ServiceResult<AuthenticationInfo>();

            try
            {
                if (input == null || (input != null && !string.IsNullOrEmpty(input.UserName) && !string.IsNullOrEmpty(input.Password)))
                {
                    serviceResult.IsSucceeded = true;
                    serviceResult.Result = new AuthenticationInfo
                    {
                        UserName = input.UserName,
                        Password = input.Password,
                        authenticationProvider = apiAuthenticationType
                    };

                }
                else
                {
                    serviceResult.ErrorMessage = "Username ve Password bilgileri alınamadı!";
                }
            }
            catch (Exception ex)
            {
                serviceResult.ErrorMessage("Username ve Password bilgileri alınamadı", ex);
            }

            return serviceResult;
        }

    }
}
