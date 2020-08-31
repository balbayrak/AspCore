using AspCore.BackendForFrontend.Abstract;
using AspCore.BackendForFrontend.Concrete.Security.User;
using AspCore.Entities.Constants;
using AspCore.Entities.DocumentType;
using AspCore.Entities.General;
using AspCore.Storage.Abstract;
using AspCore.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace AspCore.BackendForFrontend.Concrete
{
    public abstract class BaseDocumentBffLayer<TDocument, TViewRequest>
        where TDocument : class, IDocument, new()
        where TViewRequest : class, IDocumentApiViewRequest<TDocument,ViewerToolbarSetting>, new()
    {
        protected readonly ICurrentUser CurrentUser;
        protected readonly IBffApiClient ApiClient;
        protected readonly ICacheService Cache;
        private string _uploaderRoute { get; set; }
        private string _viewerRoute { get; set; }
        private string _signerRoute { get; set; }

        protected IServiceProvider ServiceProvider { get; private set; }
        public BaseDocumentBffLayer(IServiceProvider serviceProvider, string uploaderRoute, string viewerRoute, string signerRoute) : base()
        {
            ServiceProvider = serviceProvider;

            ApiClient = ServiceProvider.GetRequiredService<IBffApiClient>();

            Cache = ServiceProvider.GetRequiredService<ICacheService>();

            CurrentUser = ServiceProvider.GetRequiredService<ICurrentUser>();

            _uploaderRoute = uploaderRoute;
            _viewerRoute = viewerRoute;
            _signerRoute = signerRoute;
        }
        public ServiceResult<TDocument> GetDocument(IDocumentRequest<TDocument> documentRequest)
        {
            ApiClient.apiUrl = $"{_uploaderRoute}/{ApiConstants.Urls.READDOCUMENT}";
            return ApiClient.PostRequest<ServiceResult<TDocument>>(documentRequest).Result;
        }
        public ServiceResult<string> ViewDocuments(IDocumentViewRequest<TDocument,ViewerToolbarSetting> viewRequest)
        {
            TViewRequest documentApiViewRequest = new TViewRequest();

            using (var scope = ServiceProvider.CreateScope())
            {
                IHttpContextAccessor httpContextAccessor = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>();
                documentApiViewRequest.clientIp = IpAddressHelper.GetUserIP(httpContextAccessor);
            }

            documentApiViewRequest.documents = viewRequest.documents;
            documentApiViewRequest.validateFiles = viewRequest.validateFiles;
            documentApiViewRequest.viewerToolbarSetting = viewRequest.viewerToolbarSetting;

            ApiClient.apiUrl = $"{_viewerRoute}/{ApiConstants.Urls.VIEWDOCUMENTS}";
         
            return ApiClient.PostRequest<ServiceResult<string>>(documentApiViewRequest).Result;
        }
    }
}
