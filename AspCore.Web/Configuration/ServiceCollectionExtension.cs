using AspCore.Dependency.Concrete;
using AspCore.Entities.Json;
using AspCore.Extension;
using AspCore.Utilities.MimeMapping;
using AspCore.Web.Configuration.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace AspCore.Web.Configuration
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection ConfigureAspCoreServices(this IServiceCollection services, Action<DependencyConfigurationOption> option)
        {

            var provider = new FileExtensionContentTypeProvider();
            services.AddSingleton<IMimeMappingService>(new MimeMappingManager(provider));

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

            var httpContextAccessor = services.FirstOrDefault(d => d.ServiceType == typeof(IHttpContextAccessor));
            if (httpContextAccessor == null)
            {
                services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            }

            services.AddHeaderPropagation(options =>
            {
                options.Headers.Add(HttpContextConstant.HEADER_KEY.CORRELATION_ID, context =>
                {
                    return new StringValues(context.HttpContext.TraceIdentifier.ToString());
                });
            });


            DependencyConfigurationOption configurationHelperOption = new DependencyConfigurationOption(services);
            option(configurationHelperOption);

         

            return services;
        }
    }
}
