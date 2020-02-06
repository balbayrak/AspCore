using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.Entities.DocumentType
{
    public interface IDocumentViewRequest<TDocument, TToolbarSetting>
        where TDocument : class, IDocument, new()
        where TToolbarSetting : class, IViewerToolbarSetting, new()
    {
        /// <summary>
        /// Document urls
        /// </summary>
        List<TDocument> documents { get; set; }

        /// <summary>
        /// Document viewer toolbar buttons configuration
        /// </summary>
        TToolbarSetting viewerToolbarSetting { get; set; }

        bool validateFiles { get; set; }
    }
}
