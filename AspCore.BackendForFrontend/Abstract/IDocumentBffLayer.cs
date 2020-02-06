using AspCore.Entities.General;
using AspCore.Entities.DocumentType;

namespace AspCore.BackendForFrontend.Abstract
{
    public interface IDocumentBffLayer<TDocument>
       where TDocument : class, IDocument, new()
    {
        ServiceResult<TDocument> GetDocument(IDocumentRequest<TDocument> documentRequest);

        ServiceResult<string> ViewDocuments(IDocumentViewRequest<TDocument, ViewerToolbarSetting> documentViewRequest);
    }
}
