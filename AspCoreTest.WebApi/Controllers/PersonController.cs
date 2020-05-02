using AspCore.BusinessApi;
using AspCoreTest.Entities.Models;
using System;
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

       
    }
}