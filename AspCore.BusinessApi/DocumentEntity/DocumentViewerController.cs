using AspCore.Entities.DocumentType;
using System;

namespace AspCore.BusinessApi.DocumentEntity
{
    public class DocumentViewerController : BaseDocumentViewerController<Document, DocumentApiViewRequest>
    {
        public DocumentViewerController(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }
    }
}
