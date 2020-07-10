using AspCore.BackendForFrontend.Abstract;
using AspCore.Entities.Authentication;
using AspCore.Entities.User;
using AspCore.Web.Concrete;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AspCoreTest.WebUI.Controllers
{
    public class AccountController : BaseAuthenticationController<AuthenticationInfo, ActiveUser, IAuthenticationBffLayer<AuthenticationInfo, ActiveUser>>
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