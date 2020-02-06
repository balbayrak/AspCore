using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.Entities.DocumentType
{
    public interface IViewerToolbarSetting
    {
        /// <summary>
        ///This provides toolbar signer button enabled or disable
        /// </summary>
        bool enabledSign { get; set; }

        /// <summary>
        ///This provides toolbar download button enabled or disable
        /// </summary>
        bool enabledDownload { get; set; }

        /// <summary>
        ///This provides toolbar download all button enabled or disable
        /// </summary>
        bool enabledDownloadAll { get; set; }
    }
}
