using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.Entities.DocumentType
{
    public class DocumentApiViewRequest : DocumentViewRequest, IDocumentApiViewRequest<Document,ViewerToolbarSetting>
    {
        public string clientIp { get; set; }
    }
}
