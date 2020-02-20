using AspCore.Entities.DocumentType;
using AspCore.WebApi;
using AspCoreTest.Business.Abstract;
using AspCoreTest.Entities.Models;

namespace AspCoreTest.WebApi.Controllers
{
    public class PersonCVController : BaseDocumentEntityController<IPersonCVService, PersonCv, Document, DocumentRequest, DocumentEntityRequest<PersonCv>>
    {
    }
}