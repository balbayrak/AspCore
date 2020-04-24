using AspCore.BusinessApi.DocumentEntity;
using AspCore.Entities.DocumentType;
using AspCoreTest.Business.Abstract;
using AspCoreTest.Entities.Models;

namespace AspCoreTest.WebApi.Controllers
{
    public class PersonCVController : BaseDocumentEntityController<IPersonCVService, PersonCv, Document, DocumentRequest, DocumentEntityRequest<PersonCv>>
    {
        public PersonCVController(IPersonCVService personCVService) : base(personCVService)
        {

        }
    }
}