using AspCore.BusinessApi;
using AspCoreTest.Entities.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspCore.Entities.General;
using AspCore.Extension;
using AspCoreTest.Dtos.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using testbusiness.Abstract;

namespace AspCoreTest.WebApi.Controllers
{
    public class AdminController : BaseEntityController<Admin,AdminDto ,IAdminService>
    {
    
        public AdminController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }


        [ActionName("GetWithInclude")]
        [HttpPost]
        public async Task<IActionResult> AddTask()
        {
            var response = await Service.AddAsync(new AdminDto()
            {
                Description = "Admin666",
            });
            return response.ToHttpResponse();
        }
    }
}