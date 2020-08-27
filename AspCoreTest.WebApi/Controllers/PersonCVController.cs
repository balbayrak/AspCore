using AspCore.BusinessApi.DocumentEntity;
using AspCore.Entities.DocumentType;
using AspCoreTest.Business.Abstract;
using AspCoreTest.Dtos.Dtos;
using AspCoreTest.Entities.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspCore.BusinessApi;
using AspCore.Entities.Constants;
using AspCore.Entities.EntityFilter;
using AspCore.Entities.General;
using AspCore.Extension;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspCoreTest.WebApi.Controllers
{
    public class PersonCVController : BaseEntityController< PersonCv,PersonCvDto,IPersonCVService>
    {
        //private readonly IServiceProvider _serviceProvider;
        public PersonCVController(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }

        [ActionName("GetWithInclude")]
        [HttpPost]
        [Authorize()]
        public async Task<IActionResult> GetWithInclude()
        {
            ServiceResult<List<PersonCvDto>> response = await Service.GetWithInclude();
            return response.ToHttpResponse();
        }

    }
}