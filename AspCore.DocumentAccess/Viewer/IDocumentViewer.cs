using AspCore.Entities.DocumentType;
using AspCore.Entities.General;

namespace AspCore.DocumentManagement.Viewer
{
    public interface IDocumentViewer<TDocument>
               where TDocument : class, IDocument, new()
    {
        ServiceResult<string> ViewDocuments(IDocumentApiViewRequest<TDocument, ViewerToolbarSetting> viewRequest);
    }
}
