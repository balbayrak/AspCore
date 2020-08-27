using AspCore.Dependency.DependencyAttributes;
using AspCore.Entities.General;
using AspCore.Web.Concrete;
using AspCoreTest.Bffs.Abstract;
using AspCoreTest.Dtos.Dtos;
using AspCoreTest.Entities.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspCoreTest.Bffs.Concrete
{
    public class AdminBff : BaseDatatableEntityBffLayer<AdminDto>, IAdminBff
    {
        public AdminBff(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}
