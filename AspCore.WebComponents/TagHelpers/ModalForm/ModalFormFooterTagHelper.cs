using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AspCore.WebComponents.TagHelpers.ModalForm
{
    [HtmlTargetElement("form-footer", ParentTag = "modal-form")]
    public class ModalFormFooterTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.Add("class", "modal-footer");
            output.TagMode = TagMode.StartTagAndEndTag;

            base.Process(context, output);
        }
    }
}

