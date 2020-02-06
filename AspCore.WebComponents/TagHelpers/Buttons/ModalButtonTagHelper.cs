using AspCore.WebComponents.HtmlHelpers.Button.Concrete;
using AspCore.WebComponents.HtmlHelpers.General.Enums;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AspCore.WebComponents.TagHelpers.Buttons
{
    [HtmlTargetElement("modal-button")]
    public class ModalButtonTagHelper : ButtonTagHelper
    {
        private const string modalSizeAtrributeName = "modal-size";
        [HtmlAttributeName(modalSizeAtrributeName)]
        public EnumModalSize modalSize { get; set; }

        private const string backDropStaticAtrributeName = "back-drop-static";
        [HtmlAttributeName(backDropStaticAtrributeName)]
        public bool backDropStatic { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            ModalHtmlActionButton modalHtmlActionButton = new ModalHtmlActionButton(id, text, iClass, cssClass,blockui, blockTarget, actionUrl, httpMethod, modalSize, backDropStatic);

            output.Content.SetHtmlContent(modalHtmlActionButton.ToHtml());
            base.Process(context, output);
        }
    }
}
