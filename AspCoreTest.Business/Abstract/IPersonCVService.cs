using AspCore.Business.Abstract;
using AspCore.Entities.DocumentType;
using AspCoreTest.Dtos.Dtos;
using AspCoreTest.Entities.Models;

namespace AspCoreTest.Business.Abstract
{
    public interface IPersonCVService : IDocumentEntityService<Document, PersonCv,PersonCvDto>, IBusinessService
    {
    }
}
