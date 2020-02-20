using AspCore.Web.Abstract;
using AspCoreTest.Entities.Models;
using AspCoreTest.Models.ViewModels;

namespace AspCoreTest.Bffs.Abstract
{
    public interface IPersonBff : IDatatableEntityBffLayer<PersonViewModel, Person>
    {
    }
}
