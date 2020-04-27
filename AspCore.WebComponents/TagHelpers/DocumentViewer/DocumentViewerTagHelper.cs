using AspCore.BackendForFrontend.Abstract;
using AspCore.Dependency.Concrete;
using AspCore.Entities.DocumentType;
using AspCore.Entities.General;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Collections.Generic;

namespace AspCore.WebComponents.TagHelpers.DocumentViewer
{
    [HtmlTargetElement("documentviewer")]
    public class DocumentViewerTagHelper : TagHelper
    {
        private readonly IDocumentBffLayer<Document> _documentBffLayer;
        public DocumentViewerTagHelper(IDocumentBffLayer<Document> documentBffLayer)
        {
            _documentBffLayer = documentBffLayer;
        }
        private const string validateSignAtrributeName = "isSignedDocuments";
        [HtmlAttributeName(validateSignAtrributeName)]
        public bool isSignedDocuments { get; set; }

        private const string enableSignAtrributeName = "enable-sign";
        [HtmlAttributeName(enableSignAtrributeName)]
        public bool enabledSign { get; set; }

        private const string enabledDownloadAtrributeName = "enable-download";
        [HtmlAttributeName(enabledDownloadAtrributeName)]
        public bool enabledDownload { get; set; }

        private const string enabledDownloadAllAtrributeName = "enable-download-all";
        [HtmlAttributeName(enabledDownloadAllAtrributeName)]
        public bool enabledDownloadAll { get; set; }


        private const string heightAtrributeName = "height";
        [HtmlAttributeName(heightAtrributeName)]
        public int height { get; set; }

        private const string idAtrributeName = "id";
        [HtmlAttributeName(idAtrributeName)]
        public string id { get; set; }

        private const string classAtrributeName = "class";
        [HtmlAttributeName(classAtrributeName)]
        public string classText { get; set; }


        private const string itemsValueAtrributeName = "document-list";
        [HtmlAttributeName(itemsValueAtrributeName)]
        public List<Document> documents { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {

            if (documents != null && documents.Count > 0)
            {
                ServiceResult<string> serviceResult = _documentBffLayer.ViewDocuments(new DocumentViewRequest
                {
                    documents = documents,
                    validateFiles = isSignedDocuments,
                    viewerToolbarSetting = new ViewerToolbarSetting
                    {
                        enabledDownload = enabledDownload,
                        enabledDownloadAll = enabledDownloadAll,
                        enabledSign = enabledSign
                    }
                });

                if (serviceResult.IsSucceededAndDataIncluded())
                {
                    output.TagName = "iframe";
                    output.Attributes.Add("id", id);
                    output.Attributes.Add("class", classText);
                    output.Attributes.Add("src", serviceResult.Result);

                    output.Attributes.Add("style", $"overflow:hidden;min-height:{(height <= 0 ? 600 : height)}px;width:100%");
                }
            }
            base.Process(context, output);
        }
    }
}
