using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.WebApi.Configuration.Swagger.Abstract
{
    public interface ISwaggerOperationDescriptor
    {
        void ApplyOperation(OpenApiOperation operation, OperationFilterContext context);
    }
}
