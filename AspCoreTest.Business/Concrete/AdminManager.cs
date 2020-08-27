using AspCore.Business.Concrete;
using AspCore.Business.General;
using AspCore.Business.Task.Abstract;
using AspCore.Entities.General;
using AspCoreTest.Business.Concrete.Tasks;
using AspCoreTest.DataAccess.Abstract;
using AspCoreTest.DataSearch.Abstract;
using AspCoreTest.Entities.Models;
using AspCoreTest.Entities.SearchableEntities;
using Microsoft.Extensions.DependencyInjection;
using System;
using AspCoreTest.Dtos.Dtos;
using testbusiness.Abstract;
using System.Threading.Tasks;

namespace testbusiness.Concrete
{
    public class AdminManager : BaseEntityManager<IAdminDAL, Admin, AdminDto>, IAdminService
    {
        public AdminManager(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }

    }
}
