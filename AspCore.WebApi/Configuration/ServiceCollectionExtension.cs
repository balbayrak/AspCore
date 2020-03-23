using AspCore.Dependency.Concrete;
using AspCore.Entities.EntityType;
using AspCore.Entities.Json;
using AspCore.WebApi.Authentication.Abstract;
using AspCore.WebApi.Configuration.Options;
using AspCore.WebApi.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
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

            DependencyConfigurationOption configurationHelperOption = new DependencyConfigurationOption(services);
            option(configurationHelperOption);

            //All configuration completed, resolve initialize all configuration.

            DependencyResolver.Init(services.BuildServiceProvider());

            return services;
        }

        public static IApplicationBuilder UseAspCoreServices<TTokenGenerator, TJWTInfo>(this IApplicationBuilder app, Action<ApplicationBuilderOption> option)
         where TTokenGenerator : ITokenGenerator<TJWTInfo>
         where TJWTInfo : class, IJWTEntity, new()
        {
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseMiddleware<CustomHeaderMiddleware<TTokenGenerator, TJWTInfo>>();

            ApplicationBuilderOption applicationBuilderOption = new ApplicationBuilderOption();
            option(applicationBuilderOption);

            return app;
        }
    }
}
