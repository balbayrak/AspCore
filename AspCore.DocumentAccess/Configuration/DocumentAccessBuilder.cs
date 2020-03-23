using Microsoft.Extensions.DependencyInjection;
using System;
using AspCore.ConfigurationAccess.Abstract;
using AspCore.ConfigurationAccess.Concrete;
using AspCore.DocumentManagement.Uploader;
using AspCore.DocumentManagement.Validator;
using AspCore.DocumentManagement.Viewer;
using AspCore.Entities.Configuration;
using AspCore.Entities.DocumentType;

namespace AspCore.DocumentAccess.Configuration
{
    public class DocumentAccessBuilder : ConfigurationOption
    {
        public DocumentAccessBuilder(IServiceCollection services) : base(services)
        {
        }

        public DocumentAccessBuilder AddDocumentUploader<TDocument, TOption, TUploder, TValidator>(Action<DocumentUploaderOption> option)
           where TDocument : class, IDocument, new()
           where TOption : class, IUploaderOption, new()
           where TUploder : BaseDocumentUploader<TDocument, TOption>, IDocumentUploader<TDocument>
           where TValidator : BaseDocumentValidator<TOption>, IDocumentValidator<TDocument, TOption>
        {
            DocumentUploaderOption documentHelperOption = new DocumentUploaderOption();
            option(documentHelperOption);

            TOption uploaderOption = null;
            using (ServiceProvider serviceProvider = services.BuildServiceProvider())
            {
                //configuration helper ile setting

                IConfigurationAccessor configurationHelper = serviceProvider.GetRequiredService<IConfigurationAccessor>();
                if (configurationHelper == null)
                {
                    throw new Exception(ConfigurationHelperConstants.ErrorMessages.CONFIGURATION_HELPER_NOT_FOUND);
                }

                uploaderOption = configurationHelper.GetValueByKey<TOption>(documentHelperOption.uploaderConfigurationKey);
            }

            if (uploaderOption != null)
            {
                services.AddTransient(typeof(IDocumentValidator<TDocument, TOption>), sp =>
                {
                    IDocumentValidator<TDocument, TOption> implementation = (IDocumentValidator<TDocument, TOption>)Activator.CreateInstance(typeof(TValidator), uploaderOption);
                    return implementation;
                });

                services.AddTransient(typeof(IDocumentUploader<TDocument>), sp =>
                {
                    IDocumentUploader<TDocument> implementation = (IDocumentUploader<TDocument>)Activator.CreateInstance(typeof(TUploder), documentHelperOption.apiKey, documentHelperOption.apiControllerRoute);
                    return implementation;
                });
            }

            return this;
        }

        public DocumentAccessBuilder AddDocumentViewer<TDocument, TViewer>(Action<DocumentAccessOption> option)
            where TDocument : class, IDocument, new()
            where TViewer : BaseDocumentViewer<TDocument>, IDocumentViewer<TDocument>
        {
            DocumentAccessOption documentHelperOption = new DocumentAccessOption();
            option(documentHelperOption);

            services.AddTransient(typeof(IDocumentViewer<TDocument>), sp =>
            {
                IDocumentViewer<TDocument> implementation = (IDocumentViewer<TDocument>)Activator.CreateInstance(typeof(TViewer), documentHelperOption.apiKey, documentHelperOption.apiControllerRoute);
                return implementation;
            });

            return this;
        }
    }
}
