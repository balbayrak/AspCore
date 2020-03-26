using AspCore.CacheEntityAccess.Configuration;
using AspCore.CacheEntityApi.Authentication;
using AspCore.CacheEntityApi.Configuration;
using AspCore.ConfigurationAccess.Configuration;
using AspCore.Entities.Authentication;
using AspCore.WebApi.Configuration;
using AspCore.WebApi.Configuration.Swagger.Concrete;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AspCoreTest.CacheApi
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
                .AddJWTAuthentication(option =>
                     {
                         option.AddAppSettingAuthenticationProvider<AuthenticationInfo, CacheApiJWTInfo, CacheApiOption, CacheApiAppSettingAuthProvider>(option =>
                         {
                             option.configurationKey = "CacheApiInfo";
                         })
                         .AddTokenGenerator<CacheApiJWTInfo, CacheApiTokenGenerator>(option =>
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
                .AddCacheEntityProviders(option=>
                {
                    option.AddElasticsearch<CacheApiOption>("CacheApiInfo");
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
