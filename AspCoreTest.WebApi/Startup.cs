using AspCore.ConfigurationAccess.Configuration;
using AspCore.Entities.User;
using AspCore.Storage.Concrete.Storage;
using AspCore.WebApi.Configuration;
using AspCore.WebApi.Configuration.Swagger.Concrete;
using AspCore.WebApi.Security.Abstract;
using AspCore.WebApi.Security.Concrete;
using AspCoreTest.Authentication.Concrete;
using AspCoreTest.DataAccess.Concrete.EntityFramework;
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
                    option.AddApiClientStorage(option =>
                    {
                        option.tokenStorage = EnumStorage.MemoryCache;
                    })
                    .AddApiClients(option =>
                    {
                        option.AddBearerAuthenticatedClient("YetkiApi")
                        .AddBearerAuthenticatedClient("DocumentApi")
                        .AddBearerAuthenticatedClient("ViewerApi")
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
                    .AddActiveUserTokenGenerator<ActiveUserJwtGenerator>(option =>
                    {
                        option.configurationKey = "TokenSettingOption";
                    });
                })
                .AddSwaggerSetting(option =>
                {
                    option.swaggerDoc = new SwaggerDoc
                    {
                        title = "TSE Service API",
                        version = "1.0",
                        description = "Service API Description",
                        contactName = "Bilal ALBAYRAK",
                        contactUrl = "http.tse.org.tr",
                        apiVersion = "v1",
                        contactEmail = "balbayrak.tse.org.tr"
                    };
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

            app.UseAspCoreServices<IActiveUserTokenGenerator, ActiveUser>(option =>
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
