using System;
using System.Collections.Generic;
using System.Text;
using AspCore.Entities.Configuration;

namespace AspCore.Entities.DocumentType
{
    public class Document : IDocument
    {
        public string name { get; set; }
        public string url { get; set; }
        public byte[] content { get; set; }
        public string project { get; set; }
        public string projectFolder { get; set; }
        public List<DocumentMetaData> documentMetaDatas { get; set; }
    }
}
