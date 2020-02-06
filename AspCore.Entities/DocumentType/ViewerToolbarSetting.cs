using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.Entities.DocumentType
{
    public class ViewerToolbarSetting : IViewerToolbarSetting
    {
        public bool enabledSign { get; set; }
        public bool enabledDownload { get; set; }
        public bool enabledDownloadAll { get; set; }
    }
}
