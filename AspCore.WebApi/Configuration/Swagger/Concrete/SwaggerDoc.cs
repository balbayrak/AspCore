using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.WebApi.Configuration.Swagger.Concrete
{
    public class SwaggerDoc
    {
        public string apiVersion { get; set; }
        public string title { get; set; }

        public string version { get; set; }

        public string description { get; set; }

        public string contactName { get; set; }

        public string contactEmail { get; set; }

        public string contactUrl { get; set; }
    }
}
