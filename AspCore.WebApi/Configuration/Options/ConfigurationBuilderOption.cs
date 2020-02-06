using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using AspCore.ApiClient.Configuration;
using AspCore.DataAccess.Configuration;
using AspCore.Dependency.Configuration;
using AspCore.Entities.Configuration;
using AspCore.Extension;
using AspCore.WebApi.Configuration.Swagger.Abstract;
using AspCore.WebApi.Configuration.Swagger.Concrete;
using AspCore.WebApi.Extension;
using AspCore.DocumentAccess.Configuration;

namespace AspCore.WebApi.Configuration.Options
{
    public class ConfigurationBuilderOption : ConfigurationOption
    {
        public ConfigurationBuilderOption(IServiceCollection services) : base(services)
        {
        }

        public ConfigurationBuilderOption AddDataAccessLayer(Action<DataAccessLayerOptionBuilder> option)
        {
            var dataAccessLayerOptionBuilder = new DataAccessLayerOptionBuilder(_services);
            option(dataAccessLayerOptionBuilder);

            return this;
        }

        public ConfigurationBuilderOption AddApiClientSetting(Action<ApiClientStorageBuilder> option)
        {
            var apiClientStorageBuilder = new ApiClientStorageBuilder(_services);
            option(apiClientStorageBuilder);

            return this;
        }

        public ConfigurationBuilderOption AddSwaggerSetting(Action<SwaggerOption> option)
        {
            SwaggerOption swaggerOption = new SwaggerOption();
            option(swaggerOption);


            _services.AddSwaggerGen(g =>
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

            _services.BindTransientByName<ISwaggerOperationDescriptor>()
                .Add<DocumentApiOperationDescriptor>("BaseDocumentController")
                .Add<EntityApiOperationDescriptor>("BaseEntityController")
                .Build();

            return this;
        }

        public ConfigurationBuilderOption AddJWTAuthentication(Action<AuthenticationProviderBuilder> option)
        {
            AuthenticationProviderBuilder authenticationProviderBuilder = new AuthenticationProviderBuilder(_services);
            option(authenticationProviderBuilder);
            return this;
        }

        public ConfigurationBuilderOption AddDocumentAccessLayer(Action<DocumentAccessBuilder> action)
        {
            DocumentAccessBuilder documentHelperBuilder = new DocumentAccessBuilder(_services);
            action(documentHelperBuilder);

            return this;
        }
    }
}
