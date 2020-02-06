using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text;
using System.Threading.Tasks;

namespace AspCore.WebComponents.TagHelpers.ModalForm
{
    [HtmlTargetElement("addoredit-modalform")]
  
    public class AddOrEditModalFormTagHelper : TagHelper
    {
        private const string idAtrributeName = "id";
        [HtmlAttributeName(idAtrributeName)]
        public string id { get; set; }

        private const string formIdAtrributeName = "form-id";
        [HtmlAttributeName(formIdAtrributeName)]
        public string formId { get; set; }

        private const string titleAtrributeName = "header-title";
        [HtmlAttributeName(titleAtrributeName)]
        public string title { get; set; }

        private const string smallTitleAtrributeName = "header-smalltitle";
        [HtmlAttributeName(smallTitleAtrributeName)]
        public string smallTitle { get; set; }

        private const string cancelTextAtrributeName = "cancel-text";
        [HtmlAttributeName(cancelTextAtrributeName)]
        public string cancelText { get; set; }

        private const string okTextAtrributeName = "ok-text";
        [HtmlAttributeName(okTextAtrributeName)]
        public string okText { get; set; }

        private const string cancelClassAtrributeName = "cancel-class";
        [HtmlAttributeName(cancelClassAtrributeName)]
        public string cancelClass { get; set; } = "btn btn-danger";

        private const string okClassAtrributeName = "ok-class";
        [HtmlAttributeName(okClassAtrributeName)]
        public string okClass { get; set; } = "btn btn-success start";

        private const string formClassAtrributeName = "form-class";
        [HtmlAttributeName(formClassAtrributeName)]
        public string formClass { get; set; } = "form-horizontal form-bordered";

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var childContext = await output.GetChildContentAsync();
            var content = childContext.GetContent();

            output.TagName = "div";
            output.Attributes.Add("id", id);
            var header = new ModalFormHeaderTagHelper
            {
                title = title,
                smallTitle = smallTitle
            };
            header.Process(context,output);

            output.Content.SetHtmlContent(CreateContent(output, content));

            base.Process(context, output);
        }

        private string CreateContent(TagHelperOutput output, string content)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"<div id='{id}'>");
            builder.AppendLine(output.Content.GetContent());
            builder.AppendLine($"<form role='form' class='{formClass}' id='{formId}' method='post'>");
            builder.AppendLine(content);
            builder.AppendLine("</form>");
            builder.AppendLine("<div class='modal-footer'>");
            builder.AppendLine($"<button type='button' class='{cancelClass}' data-dismiss='modal'>{cancelText}</button>");
            builder.AppendLine(
                $"<button data-blockui='false' type='submit' form='{formId}' class='{okClass} entitysubmit'>{okText}</button>");
            builder.AppendLine("</div>");
            builder.AppendLine("</div>");
            return builder.ToString();
        }
    }
}
