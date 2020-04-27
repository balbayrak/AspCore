using AspCore.AOP.Abstract;
using AspCore.Business.Task.Abstract;
using AspCore.BusinessApi.DocumentEntity;
using AspCore.Dependency.Concrete;
using AspCore.Entities.Constants;
using AspCore.Entities.DocumentType;
using AspCore.Entities.General;
using AspCore.Extension;
using AspCoreTest.Business.Abstract;
using AspCoreTest.Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspCoreTest.WebApi.Controllers
{
    public class PersonCVController : BaseDocumentEntityController<IPersonCVService, PersonCv, Document, DocumentRequest, DocumentEntityRequest<PersonCv>>
    {


        [ActionName("Liveness2")]
        [HttpGet]
        [ProducesResponseType(typeof(ServiceResult<bool>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [AllowAnonymous]

        public IActionResult Liveness2()
        {
            ServiceResult<bool> response = new ServiceResult<bool>();
            var service = DependencyResolver.Current.GetService<IPersonCVService>();
            var service2 = DependencyResolver.Current.GetService<ITaskFlowBuilder>();
            var service23= DependencyResolver.Current.GetService<IInterceptorContext>();

            response.IsSucceeded = true;
            response.Result = true;
            return response.ToHttpResponse();
        }
    }
}