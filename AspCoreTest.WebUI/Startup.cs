using AspCore.ApiClient.Handlers;
using AspCore.Authentication.JWT.Concrete;
using AspCore.BackendForFrontend.Concrete;
using AspCore.ConfigurationAccess.Configuration;
using AspCore.Entities.DocumentType;
using AspCore.Web.Configuration;
using AspCore.WebComponents.HtmlHelpers.ConfirmBuilder;
using AspCore.WebComponents.ViewComponents.Alert.Concrete;
using AspCoreTest.Authentication.Concrete;
using AspCoreTest.Entities.SearchableEntities;
using AspCoreTest.WebUI.DependencyModules;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AspCoreTest.WebUI
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
                    option.AutoBindModules();
                    option.AddDependencyModule<WebDependencyModule2>();
                })
                .AddConfigurationManager(option =>
                {
                    option.AddConfigurationHelper(option =>
                    {
                        option.type = EnumConfigurationAccessorType.AppSettingJson;
                    });
                })
                .AddCookieAuthentication(option =>
                {
                    option.AddCookieSetting("AuthCookieOption")
                    .AddAuthenticationProvider(builder =>
                    {
                        builder.Add("custom", typeof(CustomWebAuthenticationProvider))
                       .Build();
                    })
                    .AddActiveUserTokenValidator<ActiveUserTokenValidator>(option =>
                    {
                        option.configurationKey = "TokenSettingOption";
                    });
                })
                .AddStorageService(option =>
                {
                    option.AddRedisCache("RedisInfo");
                    option.AddCookie();
                })
                .AddBffApiClient(option =>
                {
                    option.apiKey = "Base";
                })
                .AddAutoMapper()
                .AddNotifierSetting(option =>
                {
                    option.AddAlertViewComponent(option =>
                    {
                        option.alertStorage = EnumAlertStorage.TempData;
                        option.alertType = AlertType.Sweet;
                    })
                    .AddConfirmService(option =>
                    {
                        option.confirmType = ConfirmType.Sweet;
                    });
                })
                .AddDocumentAccessLayer<Document, DocumentApiViewRequest, DocumentBffLayer>(option =>
                {
                    option.uploaderRoute = "DocumentUploader";
                    option.viewerRoute = "DocumentViewer";
                    option.signerRoute = "";
                })
                 .AddDataProtectorHelper(option =>
                 {
                     option.dataProtectorKey = "tsesecretkey";
                     option.persistFileSytemPath = @"bin\debug\configuration";
                     option.lifeTime = 10;
                 })
                .AddMimeTypeService(option =>
                {
                    option.Build();
                })
                .AddDataSearchLayer(option =>
                {
                    option.AddApiClients("DataSearchApi", option =>
                    {
                        option.AddAuthenticatedApiClient(option =>
                        {
                            option.apiKey = "DataSearchApi";
                            option.authenticationHandler = EnumAuthenticationHandler.Cache;
                        })
                        .Build();
                    }).AddDataSearchEngine<PersonSearchEntity>("api/PersonCache")
                    .ElasticSearchAdmins(option =>
                    {
                        option.AddElasticSearchAdmin<PersonSearchEntity>("api/PersonCache");
                    });
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();


            app.UseAspCoreWeb(option=>
            {
                option.ControllerName = "Account";
                option.SameDomain = true;
            });
        }
    }
}
