using AspCore.AOP.Abstract;
using AspCore.Business.Task.Abstract;
using AspCore.BusinessApi;
using AspCore.Entities.EntityFilter;
using AspCore.Entities.General;
using AspCore.Extension;
using AspCoreTest.Business.Abstract;
using AspCoreTest.Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
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