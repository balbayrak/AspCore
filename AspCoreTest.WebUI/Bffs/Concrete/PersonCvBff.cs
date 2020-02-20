using AspCore.BackendForFrontend.Concrete;
using AspCore.Entities.DocumentType;
using AspCoreTest.Bffs.Abstract;
using AspCoreTest.Entities.Models;
using AspCoreTest.Models.ViewModels;

namespace AspCoreTest.Bffs.Concrete
{
    public class PersonCvBff : BaseDocumentEntityBffLayer<PersonCvViewModel, PersonCv, Document>, IPersonCVBff
    {
        public PersonCvBff() : base()
        {
        }
    }
}
