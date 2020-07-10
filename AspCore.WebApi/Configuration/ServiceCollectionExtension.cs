using AspCore.ApiClient.Entities.Concrete;
using AspCore.Authentication.JWT.Abstract;
using AspCore.Entities.EntityType;
using AspCore.Entities.Json;
using AspCore.Extension;
using AspCore.Utilities.DataProtector;
using AspCore.WebApi.Configuration.Options;
using AspCore.WebApi.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;

namespace AspCore.WebApi.Configuration
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigureAspCoreServices(this IServiceCollection services, Action<DependencyConfigurationOption> option, Action<MvcOptions> mvcOptions = null)
        {
            if (mvcOptions != null)
            {
                services.AddMvcCore(mvcOptions).AddAuthorization();
            }
            else
            {
                services.AddMvcCore().AddAuthorization();
            }

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.Converters.Add(new DecimalJsonConverter());
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.TypeNameHandling = TypeNameHandling.Auto;
                options.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;

            });

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            services.AddHeaderPropagation(options =>
            {
                options.Headers.Add(HttpContextConstant.HEADER_KEY.CORRELATION_ID, context =>
                {
                    return new StringValues(Guid.NewGuid().ToString());
                });
            });

            DependencyConfigurationOption configurationHelperOption = new DependencyConfigurationOption(services);
            option(configurationHelperOption);

            //All configuration completed, resolve initialize all configuration.

           

            return services;
        }

        public static IApplicationBuilder UseAspCoreServices<TTokenValidator, TJWTInfo>(this IApplicationBuilder app, Action<ApplicationBuilderOption> option)
         where TTokenValidator : ITokenValidator<TJWTInfo>
         where TJWTInfo : class, IJWTEntity, new()
        {
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseMiddleware<ActiveUserHeaderMiddleware<TJWTInfo>>();

            app.UseHeaderPropagation();

            ApplicationBuilderOption applicationBuilderOption = new ApplicationBuilderOption();
            option(applicationBuilderOption);

            ApiClientFactory.Init(app.ApplicationServices);
            var dataProtector = app.ApplicationServices.GetService<IDataProtectorHelper>();
            if(dataProtector!=null)
            {
                DataProtectorFactory.Init(dataProtector);
            }
            
            return app;
        }
    }
}
