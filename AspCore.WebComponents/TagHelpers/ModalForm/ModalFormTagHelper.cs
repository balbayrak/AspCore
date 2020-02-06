using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;

namespace AspCore.WebComponents.TagHelpers.ModalForm
{
    [HtmlTargetElement("modal-form")]
    [RestrictChildren("form-body", "form-header", "form-footer")]
    public class ModalFormTagHelper : TagHelper
    {
        private const string idAtrributeName = "id";
        [HtmlAttributeName(idAtrributeName)]
        public string id { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContext = await output.GetChildContentAsync();
            var content = childContext.GetContent();

            output.TagName = "div";
            output.Attributes.Add("id", id);
            output.Content.SetHtmlContent(content);
            base.Process(context, output);
        }
    }
}
