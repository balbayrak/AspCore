using AspCore.BusinessApi.Configuration;
using AspCore.ConfigurationAccess.Configuration;
using AspCore.DataSearchApi.Configuration;
using AspCore.DataSearchApi.ElasticSearch.Authentication;
using AspCore.ElasticSearch.Configuration;
using AspCore.Entities.Authentication;
using AspCore.WebApi.Authentication.Abstract;
using AspCore.WebApi.Configuration;
using AspCore.WebApi.Configuration.Swagger.Concrete;
using AspCore.WebApi.Filters;
using AspCoreTest.DataAccess.Concrete.EntityFramework;
using AspCoreTest.DataSearchApi.ESProviders;
using AspCoreTest.Entities.Models;
using AspCoreTest.Entities.SearchableEntities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using testbusiness.Abstract;

namespace AspCoreTest.DataSearchApi
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
                .AddJWTAuthentication(option =>
                {
                    option.AddAppSettingAuthenticationProvider<AuthenticationInfo, ElasticSearchApiJWTInfo, ElasticSearchApiOption, ElasticSearchAppSettingAuthProvider>(option =>
                    {
                        option.configurationKey = "SearchApiInfo";
                    })
                    .AddTokenGenerator<ElasticSearchApiJWTInfo, ElasticSearchApiTokenGenerator>(option =>
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
                .AddDataSearchProviders(option =>
                {
                    option.AddElasticSearch<ElasticSearchApiOption>("SearchApiInfo")
                    .AddElasticSearchIndex<PersonSearchEntity, IPersonService, PersonElasticSearchProvider>("person");
                });
            }, mvcOption =>
            {
                mvcOption.Filters.Add(new JWTAuthorizationFilter<AuthenticationInfo, ElasticSearchApiJWTInfo>(option =>
                {
                    option.authenticationProviderType = typeof(ElasticSearchAppSettingAuthProvider);
                    option.excludeControllerNames = new string[] { "Authentication" };
                }));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAspCoreServices<ITokenGenerator<ElasticSearchApiJWTInfo>, ElasticSearchApiJWTInfo>(option =>
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
                    option.SwaggerEndpoint("/swagger/v1/swagger.json", "AspCore Data Search API");
                    option.RoutePrefix = "api";

                })
                .UseDataSearch(app, option =>
                 {
                     option.InitElasticSearchIndex<PersonSearchEntity, IPersonService, PersonElasticSearchProvider>("person", true);
                 });
            });
        }
    }
}
