using AspCore.Authentication.Concrete;
using AspCore.Entities.General;
using AspCore.Entities.User;
using AspCoreTest.Authentication.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspCoreTest.Authentication.Concrete
{
    public class CustomApiAuthenticationProvider : ICustomApiAuthenticationProvider
    {
        public ServiceResult<ActiveUser> Authenticate(AuthenticationInfo input)
        {
            ServiceResult<ActiveUser> serviceResult = new ServiceResult<ActiveUser>();

            try
            {
                ActiveUser activeUser = null;

                if (input.UserName == "Admin" && input.Password == "Admin")
                {
                    activeUser = new ActiveUser();
                    activeUser.id = Guid.NewGuid();
                    activeUser.name = "Bilal";
                    activeUser.surname = "Albayrak";
                    activeUser.tckn = "11111111111";
                }
                else if (input.UserName == "Admin2" && input.Password == "Admin2")
                {
                    activeUser = new ActiveUser();
                    activeUser.id = Guid.NewGuid();
                    activeUser.name = "Yusuf";
                    activeUser.surname = "Aykaç";
                    activeUser.tckn = "22222222222";
                }
                if (activeUser != null)
                {
                    serviceResult.IsSucceeded = true;
                    serviceResult.Result = activeUser;
                }
            }
            catch
            {

            }

            return serviceResult;
        }

        public ServiceResult<bool> AuthorizeAction(string actionName, IDictionary<string, object> arguments = null)
        {
            return new ServiceResult<bool> { IsSucceeded = true };
        }
    }
}
