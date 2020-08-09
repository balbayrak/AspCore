using AspCore.BusinessApi;
using AspCoreTest.Entities.Models;
using System;
using AspCoreTest.Dtos.Dtos;
using testbusiness.Abstract;

namespace AspCoreTest.WebApi.Controllers
{
    public class PersonController : BaseEntityController<Person,PersonDto ,IPersonService>
    {
    
        public PersonController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

    }
}