using System;
using AspCore.AOP.Abstract;
using AspCore.Business.Task.Abstract;
using AspCore.BusinessApi;
using AspCore.Entities.General;
using AspCore.Extension;
using AspCoreTest.Business.Abstract;
using AspCoreTest.Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using testbusiness.Abstract;

namespace AspCoreTest.WebApi.Controllers
{
    public class PersonController : BaseEntityController<Person, IPersonService>
    {
        private readonly IServiceProvider _serviceProvider;

        public PersonController(IPersonService personService,IServiceProvider serviceProvider) : base(personService)
        {
            _serviceProvider = serviceProvider;
        }

        [ActionName("Liveness2")]
        [HttpGet]
        [ProducesResponseType(typeof(ServiceResult<bool>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [AllowAnonymous]

        public IActionResult Liveness2()
        {
            ServiceResult<bool> response = new ServiceResult<bool>();
            _serviceProvider.GetService(typeof(IPersonCVService));
            using (var scope=_serviceProvider.CreateScope())
            {
                var service = scope.ServiceProvider.GetService<IPersonCVService>();
                var service2 = scope.ServiceProvider.GetService<ITaskFlowBuilder>();
                var service23 = scope.ServiceProvider.GetService<IInterceptorContext>();
            }
         

            response.IsSucceeded = true;
            response.Result = true;
            return response.ToHttpResponse();
        }
    }
}