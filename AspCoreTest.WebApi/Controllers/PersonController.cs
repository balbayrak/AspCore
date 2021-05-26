using AspCore.BusinessApi;
using AspCoreTest.Entities.Models;
using System;
using System.Threading.Tasks;
using AspCore.Extension;
using AspCoreTest.Dtos.Dtos;
using Microsoft.AspNetCore.Mvc;
using testbusiness.Abstract;

namespace AspCoreTest.WebApi.Controllers
{
    public class PersonController : BaseEntityController<Person,PersonDto ,IPersonService>
    {
    
        public PersonController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override async Task<IActionResult> AddAsync(PersonDto[] entities)
        {
            var result =Service.Add(entities[0]);
            return result.ToHttpResponse();
        }
    }
}