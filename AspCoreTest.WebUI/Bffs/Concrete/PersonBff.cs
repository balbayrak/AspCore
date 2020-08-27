using AspCore.Dependency.DependencyAttributes;
using AspCore.Entities.General;
using AspCore.Web.Concrete;
using AspCoreTest.Bffs.Abstract;
using AspCoreTest.Dtos.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AspCoreTest.Bffs.Concrete
{
    [ExposedService(typeof(IPersonBff))]
    public class PersonBff : BaseDatatableEntityBffLayer<PersonDto>, IPersonBff
    {
        public PersonBff(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public override Task<ServiceResult<bool>> AddAsync(List<PersonDto> entities)
        {
            entities[0].Id = Guid.NewGuid();
            entities[0].Admin = new AdminDto();
            entities[0].Admin.Description = "test";
         
            return base.AddAsync(entities);
        }
    }
}
