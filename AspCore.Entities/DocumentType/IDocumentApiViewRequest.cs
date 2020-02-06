using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.Entities.DocumentType
{
    public interface IDocumentApiViewRequest<TDocument, TToolbarSetting> : IDocumentViewRequest<TDocument, TToolbarSetting>
           where TToolbarSetting : class, IViewerToolbarSetting, new()
        where TDocument : class, IDocument, new()
    {
        string clientIp { get; set; }
    }
}
