using AspCore.ApiClient.Abstract;
using AspCore.ApiClient.Entities;
using AspCore.Entities.DocumentType;
using AspCore.Entities.General;

namespace AspCore.DocumentManagement.Viewer
{
    public abstract class BaseDocumentViewer<TDocument>
        where TDocument : class, IDocument, new()
    {
        protected IApiClient _apiClient { get; private set; }
        protected string _apiControllerRoute { get; private set; }

        public BaseDocumentViewer(string viewerKey, string apiControllerRoute)
        {
            _apiClient = ApiClientFactory.Instance.GetApiClient(viewerKey);

            if (!apiControllerRoute.StartsWith("/"))
            {
                apiControllerRoute = "/" + apiControllerRoute;
            }
            _apiControllerRoute = apiControllerRoute;

        }

        public abstract ServiceResult<string> ViewDocuments(IDocumentApiViewRequest<TDocument, ViewerToolbarSetting> viewRequest);
    }
}
