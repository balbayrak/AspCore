using AspCore.Web.Concrete;
using AspCoreTest.Bffs.Abstract;
using AspCoreTest.Entities.Models;
using AspCoreTest.Models.ViewModels;

namespace AspCoreTest.Bffs.Concrete
{
    public class PersonBff : BaseDatatableEntityBffLayer<PersonViewModel, Person>, IPersonBff
    {
        public PersonBff() : base()
        {
        }
    }
}
