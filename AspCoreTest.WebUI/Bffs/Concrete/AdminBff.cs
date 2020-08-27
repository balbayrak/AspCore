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

        public override Task<ServiceResult<bool>> AddAsync(List<AdminDto> entities)
        {
            entities = new List<AdminDto>();
            AdminDto adminDto = new AdminDto();
            adminDto.Description = "test";
            adminDto.Person = new PersonDto();
            adminDto.Person.Name = "aaaaa";
            adminDto.Person.Surname = "aaaaa";
            entities.Add(adminDto);

            return base.AddAsync(entities);
        }
    }
}
