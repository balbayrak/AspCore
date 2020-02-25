using AspCore.Web.Concrete;
using AspCore.WebComponents.ViewComponents.Alert.Concrete;
using AspCore.WebComponents.ViewComponents.Alert.Concrete.Alert;
using AspCoreTest.Bffs.Abstract;
using AspCoreTest.Entities.Models;
using AspCoreTest.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace AspCoreTest.WebUI.Controllers
{
    public class HomeController : BaseWebEntityController<Person, PersonViewModel, IPersonBff>
    {
        private readonly IPersonCVBff _personCVBff;


        public HomeController(IPersonCVBff personCVBff)
        {
            _personCVBff = personCVBff;
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
