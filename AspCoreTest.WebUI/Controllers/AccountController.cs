using AspCore.BackendForFrontend.Abstract;
using AspCore.Entities.Authentication;
using AspCore.Entities.User;
using AspCore.Web.Concrete;
using Microsoft.AspNetCore.Mvc;
using System;
using AspCore.WebComponents.ViewComponents.Alert.Concrete;

namespace AspCoreTest.WebUI.Controllers
{
    public class AccountController : BaseAuthenticationController<AuthenticationInfo, ActiveUser, IAuthenticationBffLayer<AuthenticationInfo, ActiveUser>>
    {
        public AccountController(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }
        public override string AuthenticationProviderName => "custom";


       
        public IActionResult Home()
        {
            return View();
        }

        public IActionResult UserLogin()
        {
            var result = StorageManager.CacheService.GetObject<string>(AuthenticationProviderName);
            if (!string.IsNullOrEmpty(result))
            {
                AlertService.Error("", result,AlertType.Toast);
            }
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
    }
}