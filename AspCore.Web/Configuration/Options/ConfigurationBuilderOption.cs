using AspCore.BackendForFrontend.Abstract;
using AspCore.BackendForFrontend.Concrete;
using AspCore.Entities.Configuration;
using AspCore.Entities.DocumentType;
using AspCore.Utilities.DataProtector;
using AspCore.Utilities.MimeMapping;
using Microsoft.Extensions.DependencyInjection;
using System;
using AspCore.WebComponents.ViewComponents.Alert.Configuration;
using AspCore.CacheEntityClient.Configuration;

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
                IDocumentBffLayer<TDocument> implementation = (IDocumentBffLayer<TDocument>)Activator.CreateInstance(typeof(TDocumentHelper), documentHelperOption.uploaderRoute, documentHelperOption.viewerRoute, documentHelperOption.signerRoute);
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

                services.AddTransient(typeof(IDataProtectorHelper), sp =>
                {
                    return new DataProtectorHelper(dataProtectorOption.dataProtectorKey);
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

        public ConfigurationBuilderOption AddCacheEntityAccessLayer(Action<CacheApiClientBuilder> builder)
        {
            CacheApiClientBuilder cacheClientBuilder = new CacheApiClientBuilder(services);
            builder(cacheClientBuilder);
            return this;
        }
    }
}
