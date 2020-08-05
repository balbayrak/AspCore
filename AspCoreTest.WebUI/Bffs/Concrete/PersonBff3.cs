using AspCore.Web.Concrete;
using AspCoreTest.Bffs.Abstract;
using AspCoreTest.Dtos.Dtos;
using System;

namespace AspCoreTest.Bffs.Concrete
{
    public class PersonBff3 : BaseDatatableEntityBffLayer<PersonDto>, IPersonBff
    {
        public PersonBff3(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}
