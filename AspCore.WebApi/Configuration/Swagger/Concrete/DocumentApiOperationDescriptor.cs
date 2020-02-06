using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Net;
using AspCore.Entities.Constants;
using AspCore.Entities.DocumentType;
using AspCore.Entities.General;
using AspCore.WebApi.Configuration.Swagger.Abstract;

namespace AspCore.WebApi.Configuration.Swagger.Concrete
{
    public class DocumentApiOperationDescriptor : ISwaggerOperationDescriptor
    {
        public void ApplyOperation(OpenApiOperation operation, OperationFilterContext context)
        {
            var controllerActionDescriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;

            if (controllerActionDescriptor != null)
            {
                Type entityType = null;
                var genericType = controllerActionDescriptor.MethodInfo.DeclaringType;

                if (genericType.GenericTypeArguments != null)
                {
                    entityType = genericType.GenericTypeArguments[0];
                }

                if (entityType != null)
                {
                    var actionName = controllerActionDescriptor.ActionName;

                    if (actionName.Equals(ApiConstants.DocumentApi_Urls.CREATE) ||
                       actionName.Equals(ApiConstants.DocumentApi_Urls.READ) ||
                       actionName.Equals(ApiConstants.Urls.READINESS))
                    {
                        ApplyReturnType(operation, context, entityType, false);
                    }
                }
            }
        }

        private void ApplyParameter(OpenApiOperation operation, OperationFilterContext context, Type requestType)
        {
            var controllerActionDescriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;


            Type documentType = requestType.GetInterfaces()[0].GenericTypeArguments[0];
            Type documentRequestType = typeof(IDocumentRequest<>);

            Type documentRequest = documentRequestType.MakeGenericType(documentType);

            OpenApiParameter parameter = new OpenApiParameter();
            parameter.Required = true;

            var myObjectSchema = context.SchemaGenerator.GenerateSchema(requestType, context.SchemaRepository);
            parameter.Schema = myObjectSchema;

            var resourceName = documentType.Name;
            parameter.Description = $"A request of {resourceName}";
            operation.Parameters.Add(parameter);

        }

        private void ApplyReturnType(OpenApiOperation operation, OperationFilterContext context,Type requestType, bool returnCollection)
        {
            if (operation.Responses.ContainsKey(HttpStatusCode.OK.GetHashCode().ToString()))
                operation.Responses.Remove(HttpStatusCode.OK.GetHashCode().ToString());

            var controllerActionDescriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;

       

            if (!operation.Responses.ContainsKey(HttpStatusCode.OK.GetHashCode().ToString()))
            {
                Type serviceType = typeof(ServiceResult<>);

                if (returnCollection)
                {
                    Type mytype = requestType;
                    Type listGenericType = typeof(IList<>);
                    requestType = listGenericType.MakeGenericType(requestType);
                    requestType = serviceType.MakeGenericType(requestType);
                }
                else
                {
                    requestType = serviceType.MakeGenericType(requestType);
                }

                var myObjectSchema = context.SchemaGenerator.GenerateSchema(requestType, context.SchemaRepository);

                var mediaType = new OpenApiMediaType();
                mediaType.Schema = myObjectSchema;

                operation.Responses.Add(HttpStatusCode.OK.GetHashCode().ToString(), new OpenApiResponse
                {
                    Description = "Success",
                    Content = new Dictionary<string, OpenApiMediaType>() { { "document", mediaType } },
                });
            }

        }
    }
}
