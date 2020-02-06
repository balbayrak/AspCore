using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using AspCore.Dependency.Concrete;
using AspCore.WebApi.Configuration.Swagger.Abstract;

namespace AspCore.WebApi.Extension
{


    public class SwaggerOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var controllerActionDescriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;

            if (controllerActionDescriptor != null)
            {
                ISwaggerOperationDescriptor swaggerOperationDescriptor = null;
                if (controllerActionDescriptor.MethodInfo.DeclaringType.FullName.Contains("BaseDocumentController"))
                {
                    swaggerOperationDescriptor = DependencyResolver.Current.GetServiceByName<ISwaggerOperationDescriptor>("BaseDocumentController");
                }
                else if (controllerActionDescriptor.MethodInfo.DeclaringType.FullName.Contains("BaseEntityController"))
                {
                    swaggerOperationDescriptor = DependencyResolver.Current.GetServiceByName<ISwaggerOperationDescriptor>("BaseEntityController");
                }

                if (swaggerOperationDescriptor != null)
                {
                    swaggerOperationDescriptor.ApplyOperation(operation, context);
                }

            }
        }
    }
}

