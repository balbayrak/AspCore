using AspCore.BackendForFrontend.Abstract;
using AspCore.Entities.DocumentType;
using AspCoreTest.Dtos.Dtos;
using AspCoreTest.Entities.Models;

namespace AspCoreTest.Bffs.Abstract
{
    public interface IPersonCVBff : IDocumentEntityBffLayer<PersonCvDto, PersonCv, Document>
    {
    }
}
