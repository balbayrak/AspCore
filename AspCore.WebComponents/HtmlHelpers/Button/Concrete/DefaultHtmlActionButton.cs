using Microsoft.AspNetCore.Html;
using System;
using AspCore.WebComponents.HtmlHelpers.Button.Abstract;
using AspCore.WebComponents.HtmlHelpers.General.Enums;

namespace AspCore.WebComponents.HtmlHelpers.Button.Concrete
{
    public class DefaultHtmlActionButton : DefaultActionButton
    {
        public DefaultHtmlActionButton(string id) : base(id)
        {

        }
        public DefaultHtmlActionButton(string id, string text, string iClass, string cssClass, bool blockui, string blockTarget, string actionUrl, EnumHttpMethod httpMethod = EnumHttpMethod.GET)
           : base(id, text, iClass, cssClass, blockui, blockTarget, actionUrl, httpMethod)
        {
        }
        public override IHtmlContent ToHtml()
        {
            return new HtmlString(CreateLink());
        }
    }
}
