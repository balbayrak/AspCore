using Microsoft.AspNetCore.Html;
using AspCore.WebComponents.HtmlHelpers.Button.Abstract;
using AspCore.WebComponents.HtmlHelpers.General;
using AspCore.WebComponents.HtmlHelpers.General.Enums;

namespace AspCore.WebComponents.HtmlHelpers.Button.Concrete
{
    public class ModalHtmlActionButton : ModalActionButton
    {
        public ModalHtmlActionButton(string id) : base(id)
        {

        }

        public ModalHtmlActionButton(string id, string text, string iClass, string cssClass,bool blockui, string blockTarget, string actionUrl, EnumHttpMethod httpMethod, EnumModalSize modalSize, bool backDropStatic = false)
           : base(id, text, iClass, cssClass, blockui, blockTarget, actionUrl, httpMethod, modalSize, backDropStatic)
        {
        }
        public override IHtmlContent ToHtml()
        {
            this.modalui.id = this.id + HelperConstant.DataTable.LINK_MODAL_ID;
            return new HtmlString(CreateLink() + ModalDialog());
        }
    }
}
