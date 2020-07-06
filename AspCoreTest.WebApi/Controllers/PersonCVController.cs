using AspCore.BusinessApi.DocumentEntity;
using AspCore.Entities.DocumentType;
using AspCoreTest.Business.Abstract;
using AspCoreTest.Entities.Models;
using System;

namespace AspCoreTest.WebApi.Controllers
{
    public class PersonCVController : BaseDocumentEntityController<IPersonCVService, PersonCv, Document, DocumentRequest, DocumentEntityRequest<PersonCv>>
    {
        //private readonly IServiceProvider _serviceProvider;
        public PersonCVController(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }
      

       
    }
}