using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.WebApi.Configuration.Swagger.Concrete
{
    public class SwaggerOption
    {
        public SwaggerDoc swaggerDoc { get; set; }
        public string includeXmlCommentFileName { get; set; }
    }
}
