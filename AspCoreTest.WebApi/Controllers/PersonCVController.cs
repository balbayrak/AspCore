using System;
using AspCore.AOP.Abstract;
using AspCore.Business.Task.Abstract;
using AspCore.BusinessApi.DocumentEntity;
using AspCore.Dependency.Concrete;
using AspCore.Entities.Constants;
using AspCore.Entities.DocumentType;
using AspCore.Entities.General;
using AspCore.Extension;
using AspCoreTest.Business.Abstract;
using AspCoreTest.Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace AspCoreTest.WebApi.Controllers
{
    public class PersonCVController : BaseDocumentEntityController<IPersonCVService, PersonCv, Document, DocumentRequest, DocumentEntityRequest<PersonCv>>
    {
        //private readonly IServiceProvider _serviceProvider;
        public PersonCVController(IPersonCVService entityService) : base(entityService)
        {

        }
      

       
    }
}