using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.WebApi.Configuration.Options
{
    public class ApplicationBuilderOption
    {
        public ApplicationBuilderOption UseAuthentication(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            return this;
        }

        public ApplicationBuilderOption UseSwagger(IApplicationBuilder app, Action<SwaggerUIOptions> option = null)
        {
            app.UseSwagger();
            if (option != null)
                app.UseSwaggerUI(option);

            return this;
        }

        public ApplicationBuilderOption ConfigureRoutes(IApplicationBuilder app, Action<IEndpointRouteBuilder> option = null)
        {
            if (option != null)
                app.UseEndpoints(option);

            return this;
        }
    }
}
