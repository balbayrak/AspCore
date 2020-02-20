using AspCoreTest.Entities.Models;
using AspCore.Business.Abstract;
using AspCore.Entities.DocumentType;

namespace AspCoreTest.Business.Abstract
{
    public interface IPersonCVService : IDocumentEntityService<Document, PersonCv>, IBusinessService
    {
    }
}
