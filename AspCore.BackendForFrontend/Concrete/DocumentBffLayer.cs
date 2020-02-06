using AspCore.BackendForFrontend.Abstract;
using AspCore.Entities.DocumentType;

namespace AspCore.BackendForFrontend.Concrete
{
    public class DocumentBffLayer : BaseDocumentBffLayer<Document, DocumentApiViewRequest>, IDocumentBffLayer<Document>
    {
        public DocumentBffLayer(string uploaderRoute, string viewerRoute, string signerRoute) : base(uploaderRoute, viewerRoute, signerRoute)
        {
        }
    }
}
