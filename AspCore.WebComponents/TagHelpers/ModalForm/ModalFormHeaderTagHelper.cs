using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;

namespace AspCore.WebComponents.TagHelpers.ModalForm
{
    [HtmlTargetElement("form-header")]
    public class ModalFormHeaderTagHelper : TagHelper
    {
        private const string titleAtrributeName = "title";
        [HtmlAttributeName(titleAtrributeName)]
        public string title { get; set; }

        private const string smallTitleAtrributeName = "smalltitle";
        [HtmlAttributeName(smallTitleAtrributeName)]
        public string smallTitle { get; set; }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Content.SetHtmlContent(CreateContent());
            base.Process(context, output);
        }

        private string CreateContent()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<div class='modal-header'>");
            builder.AppendLine("<h3 class='page-title'>");
            builder.AppendLine(title);
            builder.AppendLine("<small>" + smallTitle + "</small>");
            builder.AppendLine("</h3></div>");
            return builder.ToString();
        }
    }
}

