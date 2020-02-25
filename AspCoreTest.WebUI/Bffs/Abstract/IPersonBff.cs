using AspCore.Web.Abstract;
using AspCoreTest.Entities.Models;
using AspCoreTest.WebUI.Models.ViewModels;

namespace AspCoreTest.Bffs.Abstract
{
    public interface IPersonBff : IDatatableEntityBffLayer<PersonViewModel, Person>
    {
    }
}
