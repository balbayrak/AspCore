using AspCore.WebComponents.HtmlHelpers.Button.Concrete;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AspCore.WebComponents.TagHelpers.Buttons
{
    [HtmlTargetElement("default-button")]
    public class DefaultButtonTagHelper : ButtonTagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            DefaultHtmlActionButton modalHtmlActionButton = new DefaultHtmlActionButton(id, text, iClass, cssClass, blockui, blockTarget, actionUrl, httpMethod);

            output.Content.SetHtmlContent(modalHtmlActionButton.ToHtml());
            base.Process(context, output);
        }
    }
}