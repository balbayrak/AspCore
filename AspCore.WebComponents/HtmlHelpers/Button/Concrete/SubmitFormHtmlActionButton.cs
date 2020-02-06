using Microsoft.AspNetCore.Html;
using System;
using AspCore.WebComponents.HtmlHelpers.Button.Abstract;

namespace AspCore.WebComponents.HtmlHelpers.Button.Concrete
{
    public class SubmitFormHtmlActionButton : SubmitFormActionButton
    {
        public SubmitFormHtmlActionButton(string id) : base(id)
        {

        }
        public override IHtmlContent ToHtml()
        {
            return new HtmlString(CreateLink());
        }
    }
}
