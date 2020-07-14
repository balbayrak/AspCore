using AspCore.Authentication.JWT.Abstract;
using AspCore.Authentication.JWT.Concrete;
using AspCore.BusinessApi.Configuration;
using AspCore.ConfigurationAccess.Configuration;
using AspCore.Entities.User;
using AspCore.WebApi.Configuration;
using AspCore.WebApi.Configuration.Swagger.Concrete;
using AspCoreTest.Authentication.Concrete;
using AspCoreTest.DataAccess.Concrete.EntityFramework;
using AspCoreTest.Entities.SearchableEntities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AspCoreTest.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureAspCoreServices(option =>
            {
                option.AddDependencyResolver(option =>
                {
                    option.AutoBind();
                })
                .AddConfigurationManager(option =>
                {
                    option.AddConfigurationHelper(option =>
                    {
                        option.type = EnumConfigurationAccessorType.AppSettingJson;
                    });
                })
                .AddDataAccessLayer(option =>
                {
                    option.ConfigureDataAccessLayer<AspCoreTestDbContext>("DataAccessLayerInfo");
                })
                .AddApiClientSetting(option =>
                {
                    option.AddMemoryCache()
                    .AddApiClients(option =>
                    {
                        option.AddBearerAuthenticatedClient(option => { option.apiKey = "YetkiApi"; })
                        .AddBearerAuthenticatedClient(option => { option.apiKey = "DocumentApi"; })
                        .AddBearerAuthenticatedClient(option => { option.apiKey = "ViewerApi"; })
                        .AddJWTAuthenticatedClient(option => { option.apiKey = "DataSearchApi"; })
                        .Build();
                    });
                })
                .AddJWTAuthentication(option =>
                {
                    option.AddAuthenticationProvider(builder =>
                    {
                        builder.Add(typeof(CustomApiAuthenticationProvider))
                         .Build();
                    })
                    .AddActiveUserTokenGenerator<ActiveUserJwtGenerator, ActiveUserTokenValidator>(option =>
                     {
                         option.configurationKey = "TokenSettingOption";
                     });
                })
                .AddSwaggerSetting(option =>
                {
                    option.swaggerDoc = new SwaggerDoc
                    {
                        title = "Service API",
                        version = "1.0",
                        description = "Service API Description",
                        contactName = "Bilal ALBAYRAK",
                        contactUrl = "http.google.com.tr",
                        apiVersion = "v1",
                        contactEmail = "balbayrak87@gmail.com"
                    };
                })
                .AddDataProtectorHelper(option =>
                {
                    option.dataProtectorKey = "tsesecretkey";
                    option.persistFileSytemPath = @"bin\debug\configuration";
                    option.lifeTime = 10;
                })
                .AddDataSearchAccessLayer("DataSearchApi", option =>
                 {
                     option.AddDataSearchEngine<PersonSearchEntity>("api/PersonCache");
                 });
                //.AddDocumentAccessLayer(option =>
                //{
                //    option.AddDocumentUploader<Document, TseDocumentUploaderOption, TseDocumentUploader, TseDocumentValidator>(option =>
                //    {
                //        option.apiKey = "DocumentApi";
                //        option.apiControllerRoute = "document";
                //        option.uploaderConfigurationKey = "TseDocumentApiSetting";
                //    })
                //    .AddDocumentViewer<Document, TseDocumentViewer>(option =>
                //    {
                //        option.apiKey = "ViewerApi";
                //        option.apiControllerRoute = "viewer";
                //    });
                //});
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAspCoreServices<ITokenValidator<ActiveUser>, ActiveUser>(option =>
            {
                option.UseAuthentication(app).
                ConfigureRoutes(app, endpoints =>
                {
                    endpoints.MapControllerRoute(
                    name: "api",
                    pattern: "api/{controller}/{action}",
                    defaults: new { action = "Index" });
                })
                .UseSwagger(app, option =>
                {
                    option.SwaggerEndpoint("/swagger/v1/swagger.json", "AspCore Service API");
                    option.RoutePrefix = "api";

                });
            });
        }
    }
}
