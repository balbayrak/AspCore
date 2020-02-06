using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AspCore.WebComponents.TagHelpers.ModalForm
{
    [HtmlTargetElement("form-body")]
    public class ModalFormBodyTagHelper : TagHelper
    {
        private const string formClassAtrributeName = "form-class";
        [HtmlAttributeName(formClassAtrributeName)]
        public string formClass { get; set; } = "form-horizontal form-bordered";

        private const string idAtrributeName = "id";
        [HtmlAttributeName(idAtrributeName)]
        public string id { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "form";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.Add("id", id);
            output.Attributes.Add("role", "form");
            output.Attributes.Add("class", formClass);
            output.Attributes.Add("method", "post");


            base.Process(context, output);
        }
    }
}
