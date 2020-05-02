using AspCore.Web.Concrete;
using AspCore.WebComponents.ViewComponents.Alert.Concrete;
using AspCoreTest.Bffs.Abstract;
using AspCoreTest.DataSearch.Abstract;
using AspCoreTest.Entities.Models;
using AspCoreTest.WebUI.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace AspCoreTest.WebUI.Controllers
{
    public class HomeController : BaseWebEntityController<Person, PersonViewModel, IPersonBff>
    {
        public HomeController(IServiceProvider serviceProvider, IPersonBff personBff) :base(serviceProvider, personBff)
        {
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult PersonCacheData()
        {
            var client = ServiceProvider.GetRequiredService<IPersonDataSearchEngine>();
            var result = client.FindBy(true, 0, 10);
            return View();
        }

        public IActionResult Privacy()
        {
            AlertService.Error("Error!", "Toast!");

            AlertService.Error("Info!", "Alertify!", AlertType.Alertify);

            AlertService.Info("default!", "BootBox!", AlertType.BootBox);

            AlertService.Info("default!", "Sweet!", AlertType.Sweet);

            AlertService.Info("default!", "Default!", AlertType.Default);


            ViewBag.Models = new List<Person>()
            {
                new Person() {Id = Guid.NewGuid(), Name = "Yusuf"},
                new Person() {Id = Guid.NewGuid(), Name = "Bilal"}
            };
            return View();
        }
    }
}
