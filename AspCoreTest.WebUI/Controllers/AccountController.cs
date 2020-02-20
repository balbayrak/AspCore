using AspCore.Authentication.Concrete;
using AspCore.Web.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace AspCoreTest.WebUI.Controllers
{
    public class AccountController : BaseAuthenticationController<AuthenticationInfo>
    {
        public override string AuthenticationProviderName => "custom";

        public IActionResult UserLogin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UserLogin(string username, string password)
        {
            Login(new AuthenticationInfo
            {
                UserName = username,
                Password = password
            });

            return View();
        }

        public void AuthenticationTicket(string ticket)
        {

        }
    }
}