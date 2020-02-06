using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.Entities.DocumentType
{
    public class DocumentRequest : IDocumentRequest<Document>
    {
        public Document document { get; set; }
    }
}
