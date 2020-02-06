using AspCore.ApiClient.Abstract;
using AspCore.ApiClient.Entities.Concrete;
using AspCore.Entities.DocumentType;
using AspCore.Entities.General;

namespace AspCore.DocumentManagement.Viewer
{
    public abstract class BaseDocumentViewer<TDocument>
        where TDocument : class, IDocument, new()
    {
        protected IAuthenticatedApiClient _apiClient { get; private set; }
        protected string _apiControllerRoute { get; private set; }

        public BaseDocumentViewer(string viewerKey, string apiControllerRoute)
        {
            _apiClient = ApiClientFactory.GetApiClient(viewerKey);

            if (!apiControllerRoute.StartsWith("/"))
            {
                apiControllerRoute = "/" + apiControllerRoute;
            }
            _apiControllerRoute = apiControllerRoute;

        }

        public abstract ServiceResult<string> ViewDocuments(IDocumentApiViewRequest<TDocument, ViewerToolbarSetting> viewRequest);
    }
}
