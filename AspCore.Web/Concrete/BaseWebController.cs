using AspCore.BackendForFrontend.Abstract;
using AspCore.ConfigurationAccess.Abstract;
using AspCore.Entities.Constants;
using AspCore.Entities.DocumentType;
using AspCore.Entities.General;
using AspCore.Storage.Concrete;
using AspCore.Utilities.MimeMapping;
using AspCore.WebComponents.ViewComponents.Alert.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AspCore.Web.Concrete
{
    public abstract class BaseWebController<TDocument, TDocumentRequest> : Controller
        where TDocument : class, IDocument, new()
        where TDocumentRequest : class, IDocumentRequest<TDocument>, new()
    {
        protected IServiceProvider ServiceProvider { get; private set; }
        protected readonly object ServiceProviderLock = new object();

        protected TService LazyGetRequiredService<TService>(ref TService reference)
            => LazyGetRequiredService(typeof(TService), ref reference);

        protected TRef LazyGetRequiredService<TRef>(Type serviceType, ref TRef reference)
        {
            if (reference == null)
            {
                lock (ServiceProviderLock)
                {
                    if (reference == null)
                    {
                        reference = (TRef)ServiceProvider.GetRequiredService(serviceType);
                    }
                }
            }

            return reference;
        }

        public StorageService StorageManager => LazyGetRequiredService(ref _storageService);
        private StorageService _storageService;

        protected IAlertService AlertService => LazyGetRequiredService(ref _alertService);
        private IAlertService _alertService;

        protected IDocumentBffLayer<TDocument> DocumentHelper => LazyGetRequiredService(ref _documentHelper);
        private IDocumentBffLayer<TDocument> _documentHelper;

        protected IConfigurationAccessor ConfigurationAccessor => LazyGetRequiredService(ref _configurationAccessor);
        private IConfigurationAccessor _configurationAccessor;

        protected BaseWebController(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

  
        [HttpGet]
        public IActionResult DownloadDocument(string documentUrl)
        {
            
            if (!string.IsNullOrEmpty(documentUrl))
            {
                ServiceResult<TDocument> documentResult = DocumentHelper.GetDocument(new TDocumentRequest
                {
                    document = new TDocument
                    {
                        url = documentUrl
                    }
                });

                if (documentResult.IsSucceededAndDataIncluded())
                {
                    using (var scope = ServiceProvider.CreateScope())
                    {
                        IMimeMappingService mappingService = scope.ServiceProvider.GetRequiredService<IMimeMappingService>();
                        return File(documentResult.Result.content, mappingService.Map(documentResult.Result.name), documentResult.Result.name);
                    }
                }
            }

            return BadRequest(string.Format(FrontEndConstants.ERROR_MESSAGES.PARAMETER_IS_NULL, nameof(documentUrl)));
        }
    }
}
