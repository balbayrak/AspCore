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
    }
}
