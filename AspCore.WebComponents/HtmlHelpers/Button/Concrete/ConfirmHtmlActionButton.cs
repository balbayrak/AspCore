using Microsoft.AspNetCore.Html;
using System;
using AspCore.WebComponents.HtmlHelpers.Button.Abstract;
using AspCore.WebComponents.HtmlHelpers.ConfirmBuilder;
using AspCore.WebComponents.HtmlHelpers.General.Enums;

namespace AspCore.WebComponents.HtmlHelpers.Button.Concrete
{
    public class ConfirmHtmlActionButton : ConfirmActionButton
    {
        public ConfirmHtmlActionButton(string id) : base(id)
        {

        }
        public ConfirmHtmlActionButton(string id, string text, string iClass, string cssClass, bool blockui, string blockTarget, string actionUrl, EnumHttpMethod httpMethod, string confirmTitle, string confirmMessage, string confirmCallbackFunc = null)
          : base(id, text, iClass, cssClass, blockui, blockTarget, actionUrl, httpMethod, confirmTitle, confirmMessage, confirmCallbackFunc)
        {
        }
        public override IHtmlContent ToHtml()
        {
            return new HtmlString(CreateLink());
        }

        public override IConfirmActionButton ConfirmOption(ConfirmOption confirmoption)
        {
            this.confirmOption = confirmoption;
            if (this.action != null)
            {
                this.confirmOption.confirmAction = new ActionInfo();
                if (!string.IsNullOrEmpty(this.action.actionUrl)) this.confirmOption.confirmAction.actionUrl = this.action.actionUrl;
            }

            this.confirmOption.confirmAction.methodType = action.methodType;
            return _instance;
        }
    }
}
