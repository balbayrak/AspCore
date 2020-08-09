using AspCore.Web.Concrete;
using AspCoreTest.Bffs.Abstract;
using AspCoreTest.Dtos.Dtos;
using System;

namespace AspCoreTest.Bffs.Concrete
{
    public class PersonBff2 : BaseDatatableEntityBffLayer<PersonDto>, IPersonBff
    {
        public PersonBff2(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}
