using AspCore.WebComponents.HtmlHelpers.General.Enums;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace AspCore.WebComponents.TagHelpers.Buttons
{
    public class ButtonTagHelper : TagHelper
    {
        private const string idAtrributeName = "id";
        [HtmlAttributeName(idAtrributeName)]
        public string id { get; set; }

        private const string textAtrributeName = "text";
        [HtmlAttributeName(textAtrributeName)]
        public string text { get; set; }

        private const string iClassAtrributeName = "iClass";
        [HtmlAttributeName(iClassAtrributeName)]
        public string iClass { get; set; }

        private const string cssClassAtrributeName = "class";
        [HtmlAttributeName(cssClassAtrributeName)]
        public string cssClass { get; set; }

        private const string blockUIAtrributeName = "block-ui";
        [HtmlAttributeName(blockUIAtrributeName)]
        public bool blockui { get; set; }

        private const string blockTargetAtrributeName = "block-target-id";
        [HtmlAttributeName(blockTargetAtrributeName)]
        public string blockTarget { get; set; }

        private const string actionUrlAtrributeName = "action-url";
        [HtmlAttributeName(actionUrlAtrributeName)]
        public string actionUrl { get; set; }

        private const string httpMethodAtrributeName = "http-method";
        [HtmlAttributeName(httpMethodAtrributeName)]
        public EnumHttpMethod httpMethod { get; set; }

    }
}
