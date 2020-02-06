using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.Entities.DocumentType
{
    public class DocumentViewRequest : IDocumentViewRequest<Document, ViewerToolbarSetting>
    {
        public List<Document> documents { get; set; }
        public ViewerToolbarSetting viewerToolbarSetting { get; set; }
        public bool validateFiles { get; set; }
    }
}
