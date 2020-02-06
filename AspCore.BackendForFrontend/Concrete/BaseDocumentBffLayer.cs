using Microsoft.AspNetCore.Http;
using AspCore.ApiClient.Abstract;
using AspCore.ApiClient.Entities.Concrete;
using AspCore.BackendForFrontend.Abstract;
using AspCore.Dependency.Concrete;
using AspCore.Entities.Constants;
using AspCore.Entities.DocumentType;
using AspCore.Entities.General;
using AspCore.Storage.Abstract;
using AspCore.Utilities;

namespace AspCore.BackendForFrontend.Concrete
{
    public abstract class BaseDocumentBffLayer<TDocument, TViewRequest>
        where TDocument : class, IDocument, new()
        where TViewRequest : class, IDocumentApiViewRequest<TDocument,ViewerToolbarSetting>, new()
    {
        private IBffApiClient apiClient { get; set; }
        private IStorage _storage { get; set; }
        private string _uploaderRoute { get; set; }
        private string _viewerRoute { get; set; }
        private string _signerRoute { get; set; }

        public BaseDocumentBffLayer(string uploaderRoute, string viewerRoute, string signerRoute) : base()
        {
            apiClient = DependencyResolver.Current.GetService<IBffApiClient>();

            _storage = DependencyResolver.Current.GetService<IStorage>();

            string tokenStorageKey = _storage.GetObject<string>(ApiConstants.Api_Keys.CUSTOM_TOKEN_STORAGE_KEY);
            apiClient.tokenStorageKey = tokenStorageKey;

            _uploaderRoute = uploaderRoute;
            _viewerRoute = viewerRoute;
            _signerRoute = signerRoute;
        }
        public ServiceResult<TDocument> GetDocument(IDocumentRequest<TDocument> documentRequest)
        {
            apiClient.apiUrl = _uploaderRoute + "/" + ApiConstants.Urls.READDOCUMENT;
            return apiClient.PostRequest<ServiceResult<TDocument>>(documentRequest).Result;
        }
        public ServiceResult<string> ViewDocuments(IDocumentViewRequest<TDocument,ViewerToolbarSetting> viewRequest)
        {
            IHttpContextAccessor httpContextAccessor = DependencyResolver.Current.GetService<IHttpContextAccessor>();

            TViewRequest documentApiViewRequest = new TViewRequest();
            documentApiViewRequest.documents = viewRequest.documents;
            documentApiViewRequest.validateFiles = viewRequest.validateFiles;
            documentApiViewRequest.viewerToolbarSetting = viewRequest.viewerToolbarSetting;
            documentApiViewRequest.clientIp = IpAddressHelper.GetUserIP(httpContextAccessor);



            apiClient.apiUrl = _viewerRoute + "/" + ApiConstants.Urls.VIEWDOCUMENTS;
            return apiClient.PostRequest<ServiceResult<string>>(documentApiViewRequest).Result;
        }
    }
}
