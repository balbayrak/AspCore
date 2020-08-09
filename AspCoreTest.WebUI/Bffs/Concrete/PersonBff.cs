using AspCore.Dependency.DependencyAttributes;
using AspCore.Web.Concrete;
using AspCoreTest.Bffs.Abstract;
using AspCoreTest.Dtos.Dtos;
using System;

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
