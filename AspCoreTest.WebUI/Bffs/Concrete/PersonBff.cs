using AspCore.Web.Concrete;
using AspCoreTest.Bffs.Abstract;
using AspCoreTest.Entities.Models;
using AspCoreTest.WebUI.Models.ViewModels;
using System;

namespace AspCoreTest.Bffs.Concrete
{
    public class PersonBff : BaseDatatableEntityBffLayer<PersonViewModel, Person>, IPersonBff
    {
        public PersonBff(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}
