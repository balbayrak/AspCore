using AspCore.Entities.DocumentType;
using System;

namespace AspCore.BusinessApi.DocumentEntity
{
    public class DocumentUploaderController : BaseDocumentUploaderController<Document, DocumentRequest>
    {
        public DocumentUploaderController(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }
    }
}
