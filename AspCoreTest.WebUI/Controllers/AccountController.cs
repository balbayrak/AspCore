using AspCore.Entities.Authentication;
using AspCore.Web.Concrete;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AspCoreTest.WebUI.Controllers
{
    public class AccountController : BaseAuthenticationController<AuthenticationInfo>
    {
        public AccountController(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }
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