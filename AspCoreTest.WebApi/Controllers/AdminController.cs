using AspCore.BusinessApi;
using AspCoreTest.Entities.Models;
using System;
using AspCoreTest.Dtos.Dtos;
using testbusiness.Abstract;

namespace AspCoreTest.WebApi.Controllers
{
    public class AdminController : BaseEntityController<Admin,AdminDto ,IAdminService>
    {
    
        public AdminController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

    }
}