using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.Entities.DocumentType
{
    public interface IDocumentRequest<T>
        where T : IDocument
    {
        T document { get; set; }
    }
}
