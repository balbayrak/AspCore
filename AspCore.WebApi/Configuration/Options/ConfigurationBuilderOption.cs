using AspCore.ApiClient.Configuration;
using AspCore.Dependency.Configuration;
using AspCore.Entities.Configuration;
using AspCore.Extension;
using AspCore.WebApi.Configuration.Swagger.Abstract;
using AspCore.WebApi.Configuration.Swagger.Concrete;
using AspCore.WebApi.Extension;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AspCore.WebApi.Configuration.Options
{
    public class ConfigurationBuilderOption : ConfigurationOption
    {
        public ConfigurationBuilderOption(IServiceCollection services) : base(services)
        {
        }

        public ConfigurationBuilderOption AddApiClientSetting(Action<ApiClientCacheBuilder> option)
        {
            
            var apiClientStorageBuilder = new ApiClientCacheBuilder(services);
            option(apiClientStorageBuilder);

            return this;
        }

        public ConfigurationBuilderOption AddSwaggerSetting(Action<SwaggerOption> option)
        {
            SwaggerOption swaggerOption = new SwaggerOption();
            option(swaggerOption);


            services.AddSwaggerGen(g =>
            {
                g.IgnoreObsoleteActions();
                g.DescribeAllParametersInCamelCase();
                g.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());


                if (swaggerOption.swaggerDoc != null)
                {
                    OpenApiContact openApiContact = new OpenApiContact();
                    openApiContact.Name = swaggerOption.swaggerDoc.contactName ?? string.Empty;
                    openApiContact.Email = swaggerOption.swaggerDoc.contactEmail ?? string.Empty;
                    if (swaggerOption.swaggerDoc.contactUrl.IsValidURI())
                    {
                        openApiContact.Url = new Uri(swaggerOption.swaggerDoc.contactUrl);
                    }

                    g.SwaggerDoc(swaggerOption.swaggerDoc.apiVersion, new OpenApiInfo
                    {
                        Title = swaggerOption.swaggerDoc.title ?? string.Empty,
                        Version = swaggerOption.swaggerDoc.version ?? string.Empty,
                        Description = swaggerOption.swaggerDoc.description ?? string.Empty,
                        Contact = openApiContact
                    });
                }

                g.OperationFilter<SwaggerOperationFilter>();
                //  g.OperationFilter<ApplySummariesOperationFilter>();


                g.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    In = ParameterLocation.Header,
                    Description = "Token bilginizi 'Bearer' yazıp boşluk bırakarak ekleyiniz.",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey

                });
                var req = new OpenApiSecurityRequirement();

                var lst = new OpenApiSecurityScheme()
                {
                    BearerFormat = "Bearer",
                };
                req.Add(lst, new List<string>());
                g.AddSecurityRequirement(req);
                if (swaggerOption.includeXmlCommentFileName != null)
                {
                    g.IncludeXmlComments(string.Format(@"{0}\" + swaggerOption.includeXmlCommentFileName,
                     System.AppDomain.CurrentDomain.BaseDirectory));
                }

            });

            services.BindTransientByName<ISwaggerOperationDescriptor>()
                .Add<DocumentApiOperationDescriptor>("BaseDocumentController")
                .Add<EntityApiOperationDescriptor>("BaseEntityController")
                .Build();

            return this;
        }

        public ConfigurationBuilderOption AddJWTAuthentication(Action<AuthenticationProviderBuilder> option)
        {
            AuthenticationProviderBuilder authenticationProviderBuilder = new AuthenticationProviderBuilder(services);
            option(authenticationProviderBuilder);
            return this;
        }


    }
}
