using AspCore.Entities.EntityFilter;
using AspCore.Entities.General;
using AspCore.Storage.Concrete;
using AspCore.Web.Concrete;
using AspCore.WebComponents.ViewComponents.Alert.Concrete;
using AspCoreTest.Bffs.Abstract;
using AspCoreTest.DataSearch.Abstract;
using AspCoreTest.Dtos.Dtos;
using AspCoreTest.Entities.ModelFilters;
using AspCoreTest.Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace AspCoreTest.WebUI.Controllers
{
    public class HomeController : BaseWebEntityController<PersonDto, IPersonBff>
    {
        private IAdminBff _adminBff;
        public HomeController(IServiceProvider serviceProvider, IPersonBff personBff) :base(serviceProvider, personBff)
        {
            _adminBff = ServiceProvider.GetRequiredService<IAdminBff>();
        }

        public IActionResult Index()
        {
            //ServiceResult<List<PersonViewModel>> histories =  BffLayer.GetEntityHistoriesAsync(new EntityFilter<Person>
            //{
            //    id = new Guid("fe809d66-1e58-40cc-9050-012daff25a04"),
            //    page=0,
            //    pageSize=5

            //}).Result;

            ServiceResult<List<PersonDto>> histories = BffLayer.GetEntityHistoriesAsync(new EntityFilter
            {
                id = new Guid("fe809d66-1e58-40cc-9050-012daff25a04"),
                page = 0,
                pageSize = 5
            }).Result;
            //StorageManager.CacheService.SetObject("test", "test");
            //StorageManager.CookieService.SetObject("test", "test");

            return View();

        }

        public IActionResult PersonCacheData()
        {
            //var client = ServiceProvider.GetRequiredService<IPersonDataSearchEngine>();
            //var filter = new PersonFilter();
            //filter.name = "bilal";

            _adminBff.AddAsync(null);


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
