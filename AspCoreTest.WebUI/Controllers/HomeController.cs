using AspCore.Entities.EntityFilter;
using AspCore.Entities.General;
using AspCore.Web.Concrete;
using AspCore.WebComponents.ViewComponents.Alert.Concrete;
using AspCoreTest.Bffs.Abstract;
using AspCoreTest.Dtos.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace AspCoreTest.WebUI.Controllers
{
    public class HomeController : BaseWebEntityController<PersonDto, IPersonBff>
    {
        private readonly IPersonCVBff _personCvBff;

        public HomeController(IServiceProvider serviceProvider, IPersonBff personBff,IPersonCVBff personCvBff) :base(serviceProvider, personBff)
        {
            _personCvBff = personCvBff;
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

           
            return View();
        }

        public IActionResult Privacy()
        {
            AlertService.Error("Error!", "Toast!");

            AlertService.Error("Info!", "Alertify!", AlertType.Alertify);

            AlertService.Info("default!", "BootBox!", AlertType.BootBox);

            AlertService.Info("default!", "Sweet!", AlertType.Sweet);

            AlertService.Info("default!", "Default!", AlertType.Default);

            //var models = new List<PersonCvDto>()
            //{
            //    new PersonCvDto() {Id = Guid.NewGuid(), Name = "Yusuf", DocumentUrl = "sadsadsada",Person = new PersonDto(){Name = "yusuf",Surname = "aykaç"}},
            //    new PersonCvDto() {Id = Guid.NewGuid(), Name = "Bilal", DocumentUrl = "sadsadsada",Person = new PersonDto(){Name = "Bilal",Surname = "Aykaç"}}
            //};
            //_personCvBff.AddAsync(models);
            //var personcv = _personCvBff.GetByIdAsync(new EntityFilter()
            //{
            //    id = Guid.NewGuid()
            //}).Result;
            //var personCvList = _personCvBff.GetByIdAsync(Guid.NewGuid()).Result;
            //foreach (var personCvDto in personCvList)
            //{
            //    personCvDto.Name = "Yusuf3";
            //    personCvDto.Person.Surname = "Aykaç3";
            //    personCvDto.Person.Admin.Description = "23223423432";
            //}
            //_personCvBff.UpdateAsync(personCvList);
            //ViewBag.Models = new List<Person>()
            //{
            //    new Person() {Id = Guid.NewGuid(), Name = "Yusuf"},
            //    new Person() {Id = Guid.NewGuid(), Name = "Bilal"}
            //};
            return View();
        }
    }
}
