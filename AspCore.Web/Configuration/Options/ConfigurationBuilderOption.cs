using AspCore.BackendForFrontend.Abstract;
using AspCore.BackendForFrontend.Concrete;
using AspCore.DataSearch.Configuration;
using AspCore.ElasticSearchApiClient.Configuration;
using AspCore.Entities.Configuration;
using AspCore.Entities.DocumentType;
using AspCore.Utilities.DataProtector;
using AspCore.Utilities.MimeMapping;
using AspCore.WebComponents.ViewComponents.Alert.Configuration;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AspCore.Web.Configuration.Options
{
    public class ConfigurationBuilderOption : ConfigurationOption
    {
        public ConfigurationBuilderOption(IServiceCollection services) : base(services)
        {
        }

        public ConfigurationBuilderOption AddNotifierSetting(Action<AlertOptionBuilder> option)
        {
            var alertOptionBuilder = new AlertOptionBuilder(services);
            option(alertOptionBuilder);

            return this;
        }

        public ConfigurationBuilderOption AddDocumentAccessLayer<TDocument, TViewRequest, TDocumentHelper>(Action<DocumentHelperOption> option)
            where TDocument : class, IDocument, new()
            where TDocumentHelper : BaseDocumentBffLayer<TDocument, TViewRequest>, IDocumentBffLayer<TDocument>
            where TViewRequest : class, IDocumentApiViewRequest<TDocument, ViewerToolbarSetting>, new()
        {
            var documentHelperOption = new DocumentHelperOption();
            option(documentHelperOption);

            services.AddTransient(typeof(IDocumentBffLayer<TDocument>), sp =>
            {
                IDocumentBffLayer<TDocument> implementation = (IDocumentBffLayer<TDocument>)Activator.CreateInstance(typeof(TDocumentHelper), sp, documentHelperOption.uploaderRoute, documentHelperOption.viewerRoute, documentHelperOption.signerRoute);
                return implementation;
            });

            return this;
        }

        public ConfigurationBuilderOption AddDataProtectorHelper(Action<DataProtectorOption> option)
        {
            var dataProtectorOption = new DataProtectorOption();
            option(dataProtectorOption);

            if (!string.IsNullOrEmpty(dataProtectorOption.dataProtectorKey))
            {
                services.AddDataProtection();

                services.AddSingleton(typeof(IDataProtectorHelper), sp =>
                {
                    var dataProtectionProvider = sp.GetRequiredService<IDataProtectionProvider>();
                    return new DataProtectorHelper(dataProtectionProvider, dataProtectorOption.dataProtectorKey);
                });
            }

            return this;
        }

        public ConfigurationBuilderOption AddMimeTypeService(Action<MimeTypeBuilder> builder)
        {
            var mimeTypeBuilder = new MimeTypeBuilder(services);
            builder(mimeTypeBuilder);

            return this;
        }

        public ConfigurationBuilderOption AddDataSearchLayer(Action<DataSearchApiClientBuilder> builder)
        {
            DataSearchApiClientBuilder dataSearchApiClientBuilder = new DataSearchApiClientBuilder(services);
            builder(dataSearchApiClientBuilder);
            return this;
        }
    }
}
