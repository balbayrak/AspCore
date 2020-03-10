using System;
using AspCore.Entities.EntityFilter;
using AspCore.Web.Concrete;
using AspCore.WebComponents.ViewComponents.Alert.Concrete;
using AspCoreTest.Bffs.Abstract;
using AspCoreTest.Entities.Models;
using AspCoreTest.WebUI.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AspCoreTest.WebUI.Controllers
{
    public class HomeController : BaseWebEntityController<Person, PersonViewModel, IPersonBff>
    {
        public HomeController()
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            AlertService.Error("Error!", "Toast!");

            AlertService.Error("Info!", "Alertify!", AlertType.Alertify);

            AlertService.Info("default!", "BootBox!", AlertType.BootBox);

            AlertService.Info("default!", "Sweet!", AlertType.Sweet);

            AlertService.Info("default!", "Default!", AlertType.Default);

            return View();
        }
    }
}
