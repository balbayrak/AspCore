using AspCore.BackendForFrontend.Abstract;
using AspCore.Entities.DocumentType;
using AspCoreTest.Entities.Models;
using AspCoreTest.WebUI.Models.ViewModels;

namespace AspCoreTest.Bffs.Abstract
{
    public interface IPersonCVBff : IDocumentEntityBffLayer<PersonCvViewModel, PersonCv, Document>
    {
    }
}
