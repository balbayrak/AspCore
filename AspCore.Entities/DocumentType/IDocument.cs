using System;
using System.Collections.Generic;
using System.Text;
using AspCore.Entities.Configuration;

namespace AspCore.Entities.DocumentType
{
    public interface IDocument
    {
        string name { get; set; }
        string url { get; set; }
        byte[] content { get; set; }
        string project { get; set; }
        string projectFolder { get; set; }
        List<DocumentMetaData> documentMetaDatas { get; set; }
    }
}
