
using AspCore.WebComponents.HtmlHelpers.Button.Concrete;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AspCore.WebComponents.TagHelpers.Buttons
{
    [HtmlTargetElement("confirm-button")]
    public class ConfirmButtonTagHelper : ButtonTagHelper
    {
        private const string confirmTitleAtrributeName = "confirm-title";
        [HtmlAttributeName(confirmTitleAtrributeName)]
        public string confirmTitle { get; set; }

        private const string confirmMessageAtrributeName = "confirm-message";
        [HtmlAttributeName(confirmMessageAtrributeName)]
        public string confirmMessage { get; set; }

        private const string confirmCallbackFuncAtrributeName = "confirm-callback-func";
        [HtmlAttributeName(confirmCallbackFuncAtrributeName)]
        public string confirmCallbackFunc { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            ConfirmHtmlActionButton modalHtmlActionButton = new ConfirmHtmlActionButton(id, text, iClass, cssClass, blockui, blockTarget, actionUrl, httpMethod, confirmTitle, confirmMessage, confirmCallbackFunc);

            output.Content.SetHtmlContent(modalHtmlActionButton.ToHtml());
            base.Process(context, output);
        }
    }
}
