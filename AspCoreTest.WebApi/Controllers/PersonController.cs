using AspCore.BusinessApi;
using AspCoreTest.Entities.Models;
using System;
using testbusiness.Abstract;

namespace AspCoreTest.WebApi.Controllers
{
    public class PersonController : BaseEntityController<Person, IPersonService>
    {
    
        public PersonController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

    }
}