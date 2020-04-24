using AspCore.BackendForFrontend.Abstract;
using AspCore.Entities.DocumentType;
using System;

namespace AspCore.BackendForFrontend.Concrete
{
    public class DocumentBffLayer : BaseDocumentBffLayer<Document, DocumentApiViewRequest>, IDocumentBffLayer<Document>
    {
        public DocumentBffLayer(IServiceProvider serviceProvider, string uploaderRoute, string viewerRoute, string signerRoute) : base(serviceProvider,uploaderRoute, viewerRoute, signerRoute)
        {
        }
    }
}
